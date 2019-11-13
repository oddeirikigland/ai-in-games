using Assets.Scripts.GameManager;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions
{
    public class DivineSmite : WalkToTargetAndExecuteAction
    {
        private int xpChange;
        private int manaChange;

        public DivineSmite(AutonomousCharacter character, GameObject target) : base("DivineSmite", character,target)
        {
            this.xpChange = 3;
            this.manaChange = 2;
        }

        public override float GetGoalChange(Goal goal)
        {
            var change = base.GetGoalChange(goal);

            if (goal.Name == AutonomousCharacter.GAIN_LEVEL_GOAL)
            {
                change += -this.xpChange;
            }
            
            return change;
        }

        public override void Execute()
        {
            base.Execute();
            this.Character.GameManager.DivineSmite(this.Target);
        }

        public override bool CanExecute()
        {
            if (!base.CanExecute()) return false;
            return true;
        }

        public override bool CanExecute(WorldModel worldModel)
        {
            var mana = (int)worldModel.GetProperty(Properties.MANA);
            if (mana < this.manaChange) return false;
            if (!base.CanExecute(worldModel)) return false;
            return true;
        }

        public override void ApplyActionEffects(WorldModel worldModel)
        {
            base.ApplyActionEffects(worldModel);

            int xp = (int)worldModel.GetProperty(Properties.XP);
            int mana = (int)worldModel.GetProperty(Properties.MANA);

            //there was an hit, enemy is destroyed, gain xp
            //disables the target object so that it can't be reused again
            worldModel.SetProperty(this.Target.name, false);

            worldModel.SetProperty(Properties.XP, xp + this.xpChange);
            var xpValue = worldModel.GetGoalValue(AutonomousCharacter.GAIN_LEVEL_GOAL);
            worldModel.SetGoalValue(AutonomousCharacter.GAIN_LEVEL_GOAL, xpValue - this.xpChange);

            worldModel.SetProperty(Properties.MANA, mana - this.manaChange);
        }

        public override float GetHValue(WorldModel worldModel)
        {
            var mana = (int)worldModel.GetProperty(Properties.MANA);

            if (mana > this.manaChange)
            {
                return base.GetHValue(worldModel) / 1.5f;
            }
            return 10.0f;
        }
    }
}
