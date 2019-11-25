using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel;
using Assets.Scripts.GameManager;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel.ForwardModelActions
{
    public class Teleport : Action
    {
        public AutonomousCharacter Character { get; private set; }

        public Teleport(AutonomousCharacter character) : base("Teleport")
        {
            this.Character = character;
        }

        public override bool CanExecute()
        {
            var level = this.Character.GameManager.characterData.Level;
            var mana = this.Character.GameManager.characterData.Mana;

            return mana >= 5 && level >= 2;
        }


        public override bool CanExecute(WorldModel worldModel)
        {
            int mana = (int)worldModel.GetProperty(Properties.MANA);
            int level = (int)worldModel.GetProperty(Properties.LEVEL);

            return mana >= 5 && level >= 2;
        }

        public override void Execute()
        {
            this.Character.GameManager.Teleport();
        }

        public override void ApplyActionEffects(WorldModel worldModel)
        {
            int mana = (int)worldModel.GetProperty(Properties.MANA);

            worldModel.SetProperty(Properties.MANA, mana - 5);
        }

        public override float GetGoalChange(Goal goal)
        {
            float change = 0.0f;
            return change;
        }

        public override float GetHValue(WorldModel worldModel)
        {
			return 5.0f;
        }
    }
}
