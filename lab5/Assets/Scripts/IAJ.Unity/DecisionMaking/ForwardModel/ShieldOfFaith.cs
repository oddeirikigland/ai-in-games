using Assets.Scripts.GameManager;
using UnityEngine;
using System;


namespace Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions
{
    public class ShieldOfFaith : Action
    {
        private int maxShield;
        private int manaConsumption;
        protected AutonomousCharacter Character { get; set; }

        public ShieldOfFaith(AutonomousCharacter character) : base("ShieldOfFaith")
        {
            this.Character = character;
            this.maxShield = 5;
            this.manaConsumption = 5;
        }

        public override void Execute()
        {
            this.Character.GameManager.ShieldOfFaith();
        }


        public override bool CanExecute(WorldModel worldModel)
        {
            int mana = (int)worldModel.GetProperty(Properties.MANA);
            if (mana >= this.manaConsumption && (int)worldModel.GetProperty(Properties.ShieldHP) != this.maxShield)
                return true;
            else
                return false;
            
        }

        public override void ApplyActionEffects(WorldModel worldModel)
        {
            int currentMana = (int)worldModel.GetProperty(Properties.MANA);

            // calculate difference between possible current shield and maximum shield 
            var currentShieldValue = (int)worldModel.GetProperty(Properties.ShieldHP);
            var change = Math.Abs(currentShieldValue - this.maxShield);

            // Set shield HP to 5 regardless of previous shield
            worldModel.SetProperty(Properties.ShieldHP, this.maxShield);
            worldModel.SetProperty(Properties.MANA, currentMana - this.manaConsumption);

			// Update goal
            var CurrentSurviveGoal = worldModel.GetGoalValue(AutonomousCharacter.SURVIVE_GOAL);
            worldModel.SetGoalValue(AutonomousCharacter.SURVIVE_GOAL, CurrentSurviveGoal - change);

        }

		public override float GetHValue(WorldModel worldModel)
		{
			var mana = (int)worldModel.GetProperty(Properties.MANA);
			var shild = (int)worldModel.GetProperty(Properties.ShieldHP);
			if (mana > manaConsumption && shild != 0)
				return 3.0f;
			return base.GetHValue(worldModel);
		}
	}
}