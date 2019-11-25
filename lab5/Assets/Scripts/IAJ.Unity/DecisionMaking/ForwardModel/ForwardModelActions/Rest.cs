using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions
{
    public class Rest : Action
    {
        protected AutonomousCharacter Character { get; set; }
        public Rest(AutonomousCharacter character) : base("Rest")
        {
            this.Character = character;
        }

        public override bool CanExecute()
        {
            int HP = (int)this.Character.GameManager.characterData.HP;
            int MAXHP = (int)this.Character.GameManager.characterData.MaxHP;
            bool Resting = this.Character.Resting;

            // Rest can only be executed if character is missing more than 2 HP
            if (Mathf.Abs(MAXHP - HP) >= 2 && Resting != true)
                return true;
            else
                return false;
        }

        public override bool CanExecute(WorldModel worldModel)
        {
            int HP = (int)worldModel.GetProperty(Properties.HP);
            int MAXHP = (int)worldModel.GetProperty(Properties.MAXHP);
            bool Resting = this.Character.Resting;

            // Rest can only be executed if character is missing more than 2 HP
            if (Mathf.Abs(MAXHP - HP) >= 2 && Resting != true)
                return true;
            else
                return false;
        }

        public override void Execute()
        {
            this.Character.GameManager.Rest();
        }

        public override void ApplyActionEffects(WorldModel worldModel)
        {
            int HP = (int)worldModel.GetProperty(Properties.HP);
            int MAXHP = (int)worldModel.GetProperty(Properties.MAXHP);

            // New HP
            HP += 2;

            worldModel.SetProperty(Properties.HP, Mathf.Min(HP, MAXHP));

            // Update goal
            var CurrentSurviveGoal = worldModel.GetGoalValue(AutonomousCharacter.SURVIVE_GOAL);
            worldModel.SetGoalValue(AutonomousCharacter.SURVIVE_GOAL, CurrentSurviveGoal - 2);
        }

        public override float GetHValue(WorldModel worldModel)
        {
			var health = (int)worldModel.GetProperty(Properties.HP);
			var time = (float)worldModel.GetProperty(Properties.TIME);
            return health * time * 0.005f;
        }
    }
}
