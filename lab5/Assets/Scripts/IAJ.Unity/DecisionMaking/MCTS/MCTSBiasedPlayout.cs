using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using Action = Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.Action;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS
{
	class MCTSBiasedPlayout: MCTS
	{
		public MCTSBiasedPlayout(CurrentStateWorldModel currentStateWorldModel): base(currentStateWorldModel)
		{
		}

		protected override Reward Playout(WorldModel initialPlayoutState)
		{
			WorldModel worldModel = initialPlayoutState.GenerateChildWorldModel();

			int depthCount = 0;
			while (!worldModel.IsTerminal())
			{
				Action[] actions = worldModel.GetExecutableActions();
				Action biasedAction = actions.First();
				foreach (Action action in actions)
				{
					if (action.GetHValue(worldModel) < biasedAction.GetHValue(worldModel))
					{
						biasedAction = action;
					}
				}
				biasedAction.ApplyActionEffects(worldModel);
				depthCount++;
			}
			if (depthCount > MaxPlayoutDepthReached) base.MaxPlayoutDepthReached = depthCount;

			return new Reward()
			{
				Value = worldModel.GetScore(),
				PlayerID = initialPlayoutState.GetNextPlayer(),
			};
		}
	}
}
