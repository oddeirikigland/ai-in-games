using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions
{
    public class GetHealthPotion : WalkToTargetAndExecuteAction
    {

        public GetHealthPotion(AutonomousCharacter character, GameObject target) : base("GetHealthPotion", character,target)
        {
        }

        public override float GetGoalChange(Goal goal)
        {
            var change = base.GetGoalChange(goal);
            if (goal.Name == AutonomousCharacter.SURVIVE_GOAL) change = 0;
            return change;
        }

        public override bool CanExecute()
        {
            if (!base.CanExecute()) return false;
            return true;
        }

        public override bool CanExecute(WorldModel worldModel)
        {
            if (!base.CanExecute(worldModel)) return false;
            return true;
        }

        public override void Execute()
        {
            base.Execute();
            this.Character.GameManager.GetHealthPotion(this.Target);
        }

        public override void ApplyActionEffects(WorldModel worldModel)
        {
            base.ApplyActionEffects(worldModel);
                
            worldModel.SetGoalValue(AutonomousCharacter.SURVIVE_GOAL, 0);
            worldModel.SetProperty(Properties.HP, (int)worldModel.GetProperty(Properties.MAXHP));

            //disables the target object so that it can't be reused again
            worldModel.SetProperty(this.Target.name, false);
        }

        public override float GetHValue(WorldModel worldModel)
        {
			var health = (int)worldModel.GetProperty(Properties.HP);
			var maxHealth = (int)worldModel.GetProperty(Properties.MAXHP);
			if (health < maxHealth)
				return base.GetHValue(worldModel) * (float)health/maxHealth;
			return 0.0f;
        }
    }
}
