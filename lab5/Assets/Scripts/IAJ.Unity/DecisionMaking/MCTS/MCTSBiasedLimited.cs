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
	class MCTSBiasedLimited : MCTSBiasedPlayout
	{
		public int MaxPlayoutDepth = 10;

		public MCTSBiasedLimited(CurrentStateWorldModel currentStateWorldModel) : base(currentStateWorldModel)
		{
		}

		protected override Reward Playout(WorldModel initialPlayoutState)
		{
			WorldModel worldModel = initialPlayoutState.GenerateChildWorldModel();

			int depthCount = 0;
			while (!worldModel.IsTerminal() && depthCount <= MaxPlayoutDepth)
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
				Value = GetWorldModelScore(worldModel),
				PlayerID = initialPlayoutState.GetNextPlayer(),
			};
		}

		private float GetWorldModelScore(WorldModel worldModel)
		{
			int money = (int)worldModel.GetProperty(Properties.MONEY);
			int HP = (int)worldModel.GetProperty(Properties.HP);
			float time = (float)worldModel.GetProperty(Properties.TIME);

			if (HP <= 0) return 0.0f;
			else if (time >= 150) return 0.0f;
			else if (money == 25) return 1.0f;
			else
			{
				int maxHP = (int)worldModel.GetProperty(Properties.MAXHP);
				int mana = (int)worldModel.GetProperty(Properties.MANA);
				int shieldHP = (int)worldModel.GetProperty(Properties.ShieldHP);
				return (money / 25.0f + (float)HP / maxHP + (0.1f * (150.0f-time) / 150.0f) + mana / 10.0f + shieldHP / 5.0f) / 4.1f;
			}
		}
	}
}
