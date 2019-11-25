using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using System;
using System.Collections.Generic;
using UnityEngine;
using Action = Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.Action;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS
{
    public class MCTS
    {
        public const float C = 1.4f;
        public bool InProgress { get; private set; }
        public int MaxIterations { get; set; }
        public int MaxIterationsProcessedPerFrame { get; set; }
        public int MaxPlayoutDepthReached { get; protected set; }
        public int MaxSelectionDepthReached { get; private set; }
        public float TotalProcessingTime { get; private set; }
        public MCTSNode BestFirstChild { get; set; }
        public List<Action> BestActionSequence { get; private set; }


        protected int CurrentIterations { get; set; }
        protected int CurrentIterationsInFrame { get; set; }
        protected int CurrentDepth { get; set; }

        protected CurrentStateWorldModel CurrentStateWorldModel { get; set; }
        protected MCTSNode InitialNode { get; set; }
        protected System.Random RandomGenerator { get; set; }
        
        

        public MCTS(CurrentStateWorldModel currentStateWorldModel)
        {
            this.InProgress = false;
            this.CurrentStateWorldModel = currentStateWorldModel;
            this.MaxIterations = 6000;
            this.MaxIterationsProcessedPerFrame = 200;
            this.RandomGenerator = new System.Random();
        }


        public void InitializeMCTSearch()
        {
            this.MaxPlayoutDepthReached = 0;
            this.MaxSelectionDepthReached = 0;
            this.CurrentIterations = 0;
            this.CurrentIterationsInFrame = 0;
            this.TotalProcessingTime = 0.0f;
            this.CurrentStateWorldModel.Initialize();
            this.InitialNode = new MCTSNode(this.CurrentStateWorldModel)
            {
                Action = null,
                Parent = null,
                PlayerID = 0
            };
            this.InProgress = true;
            this.BestFirstChild = null;
            this.BestActionSequence = new List<Action>();
        }

        public Action Run()
        {
            MCTSNode selectedNode;
            Reward reward;

            var startTime = Time.realtimeSinceStartup;

            this.CurrentIterationsInFrame = 0;

            while(this.CurrentIterationsInFrame < this.MaxIterationsProcessedPerFrame && this.CurrentIterations < this.MaxIterations)
            {
                this.CurrentIterationsInFrame++;
				this.CurrentIterations++;
                selectedNode = this.Selection(this.InitialNode);
				if (selectedNode.State.GetScore() != 0)
				{
					this.TotalProcessingTime += Time.realtimeSinceStartup - startTime;
					this.InProgress = false;
					return this.EarlyStop(selectedNode);
				}
                reward = this.Playout(selectedNode.State);
                this.Backpropagate(selectedNode, reward);
            }

			this.TotalProcessingTime += Time.realtimeSinceStartup - startTime;

			if (this.CurrentIterations >= this.MaxIterations)
			{
				this.InProgress = false;

				return BestFinalAction(this.InitialNode);
			}
			return this.InitialNode.Action;
		}

		private Action EarlyStop(MCTSNode selectedNode)
		{
			MCTSNode node = selectedNode;
			this.BestActionSequence = new List<Action>();
			while (node.Parent != InitialNode)
			{
				BestActionSequence.Add(node.Action);
				node = node.Parent;
			}
			this.BestActionSequence.Reverse();
			this.BestFirstChild = node;
			return this.BestFirstChild.Action;
		}

        protected MCTSNode Selection(MCTSNode initialNode)
        {
            Action nextAction;
            MCTSNode currentNode = initialNode;

			int depthCount = 0;
            while(!currentNode.State.IsTerminal())
            {
                nextAction = currentNode.State.GetNextAction();
				depthCount++;
                if (nextAction != null) {
					if (depthCount > MaxSelectionDepthReached) MaxSelectionDepthReached = depthCount;
					return this.Expand(currentNode, nextAction);
				}
                else currentNode = this.BestUCTChild(currentNode);
            }

			if (depthCount > MaxSelectionDepthReached) MaxSelectionDepthReached = depthCount;

            return currentNode;
        }

        protected virtual Reward Playout(WorldModel initialPlayoutState)
        {
			Action[] actions = initialPlayoutState.GetExecutableActions();
			float score = 0;

			foreach(Action action in actions)
			{
				WorldModel worldModel = initialPlayoutState.GenerateChildWorldModel();
				int depthCount = 0;
				while (!worldModel.IsTerminal())
				{
					Action randomAction = actions[RandomGenerator.Next(actions.Length)];
					randomAction.ApplyActionEffects(worldModel);
					depthCount++;
				}
				if (depthCount > MaxPlayoutDepthReached) MaxPlayoutDepthReached = depthCount;

				score += worldModel.GetScore();
			}

			return new Reward()
			{
				Value = score / actions.Length,
				PlayerID = initialPlayoutState.GetNextPlayer(),
			};
        }

        protected virtual void Backpropagate(MCTSNode node, Reward reward)
        {
			MCTSNode backNode = node;
			while(backNode != null)
			{
				backNode.N += 1;
				backNode.Q += reward.Value;
				backNode = backNode.Parent;
			}
        }

        protected MCTSNode Expand(MCTSNode parent, Action action)
        {
			WorldModel newState = parent.State.GenerateChildWorldModel();
			action.ApplyActionEffects(newState);

			MCTSNode child = new MCTSNode(newState)
			{
				Action = action,
				Parent = parent,
				PlayerID = newState.GetNextPlayer(),
			};
			parent.ChildNodes.Add(child);
			return child;
        }

        //gets the best child of a node, using the UCT formula
        protected virtual MCTSNode BestUCTChild(MCTSNode node)
        {
            double best = float.NegativeInfinity;
            MCTSNode bestNode = null;
            if (node.ChildNodes.Count > 0) {
                foreach (MCTSNode childnode in node.ChildNodes)
                {
                    double currentUCTvalue = (childnode.Q / childnode.N) + C * Math.Sqrt((Math.Log(childnode.Parent.N) / childnode.N));
                    if (currentUCTvalue > best)
                    {
                        best = currentUCTvalue;
                        bestNode = childnode;
                    }
                }
                return bestNode;
            }
            else
            {
                return bestNode;
            }
		}

        //this method is very similar to the bestUCTChild, but it is used to return the final action of the MCTS search, and so we do not care about
        //the exploration factor
        protected MCTSNode BestChild(MCTSNode node)
        {           
            float best = float.NegativeInfinity;
            MCTSNode bestNode = null;
            if (node.ChildNodes.Count > 0) {
                foreach (MCTSNode childnode in node.ChildNodes)
                {
                    // double currentValue = (childnode.Q / childnode.N) + Math.Sqrt((Math.Log(childnode.Parent.N) / childnode.N));
                    float currentValue = childnode.Q / childnode.N;
                    if (currentValue > best)
                    {
                        best = currentValue;
                        bestNode = childnode;
                    }
                }
                return bestNode;
            }
            else
            {
                return bestNode;
            }
		}

        protected Action BestFinalAction(MCTSNode node)
        {
            var bestChild = this.BestChild(node);
            if (bestChild == null) return null;

            this.BestFirstChild = bestChild;

            //this is done for debugging proposes only
            this.BestActionSequence = new List<Action>();
            this.BestActionSequence.Add(bestChild.Action);
            node = bestChild;

            while(!node.State.IsTerminal())
            {
                bestChild = this.BestChild(node);
                if (bestChild == null) break;
                this.BestActionSequence.Add(bestChild.Action);
                node = bestChild;    
            }

            return this.BestFirstChild.Action;
        }

    }
}
