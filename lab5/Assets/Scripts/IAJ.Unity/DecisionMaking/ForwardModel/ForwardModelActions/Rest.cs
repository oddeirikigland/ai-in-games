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

            // Rest can only be executed if character is missing more than 2 HP
            if (MAXHP > HP)
                return true;
            else
                return false;
        }

        public override bool CanExecute(WorldModel worldModel)
        {
            int HP = (int)worldModel.GetProperty(Properties.HP);
            int MAXHP = (int)worldModel.GetProperty(Properties.MAXHP);

            // Rest can only be executed if character is missing more than 2 HP
            if (MAXHP > HP)
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
			worldModel.SetProperty(Properties.TIME, (float)worldModel.GetProperty(Properties.TIME) + 5.0f);

            // Update goal
            var CurrentSurviveGoal = worldModel.GetGoalValue(AutonomousCharacter.SURVIVE_GOAL);
            worldModel.SetGoalValue(AutonomousCharacter.SURVIVE_GOAL, CurrentSurviveGoal - 2);
        }

        public override float GetHValue(WorldModel worldModel)
        {
			var health = (int)worldModel.GetProperty(Properties.HP);
			var time = (float)worldModel.GetProperty(Properties.TIME);
            return health * time;
        }
    }
}
