/*using Assets.Scripts.IAJ.Unity.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel
{
    public class WorldModel
    {
       *//* private Dictionary<string, object> Properties { get; set; }
        private List<Action> Actions { get; set; }
        protected IEnumerator<Action> ActionEnumerator { get; set; } 

        
        private Dictionary<string, float> GoalValues { get; set; } 

        protected WorldModel Parent { get; set; }

        public WorldModel(List<Action> actions)
        {
            this.Properties = new Dictionary<string, object>();
            this.GoalValues = new Dictionary<string, float>();

            // Integrates more randomness in choosing actions
            this.Actions = new List<Action>(actions);
            RandomHelper.Shuffle(this.Actions);
            this.ActionEnumerator = actions.GetEnumerator();
        }

        public WorldModel(WorldModel parent)
        {
            this.Properties = new Dictionary<string, object>();
            this.GoalValues = new Dictionary<string, float>();
            this.Actions = parent.Actions;
            this.Parent = parent;
            this.ActionEnumerator = this.Actions.GetEnumerator();
        }

        public virtual object GetProperty(string propertyName)
        {
            //recursive implementation of WorldModel
            if (this.Properties.ContainsKey(propertyName))
            {
                return this.Properties[propertyName];
            }
            else if (this.Parent != null)
            {
                return this.Parent.GetProperty(propertyName);
            }
            else
            {
                return null;
            }
        }

        public virtual void SetProperty(string propertyName, object value)
        {
            this.Properties[propertyName] = value;
        }

        public virtual float GetGoalValue(string goalName)
        {
            //recursive implementation of WorldModel
            if (this.GoalValues.ContainsKey(goalName))
            {
                return this.GoalValues[goalName];
            }
            else if (this.Parent != null)
            {
                return this.Parent.GetGoalValue(goalName);
            }
            else
            {
                return 0;
            }
        }

        public virtual void SetGoalValue(string goalName, float value)
        {
            var limitedValue = value;
            if (value > 10.0f)
            {
                limitedValue = 10.0f;
            }

            else if (value < 0.0f)
            {
                limitedValue = 0.0f;
            }

            this.GoalValues[goalName] = limitedValue;
        }

        public virtual WorldModel GenerateChildWorldModel()
        {
            return new WorldModel(this);
        }

        public float CalculateDiscontentment(List<Goal> goals)
        {
            var discontentment = 0.0f;

            foreach (var goal in goals)
            {
                var newValue = this.GetGoalValue(goal.Name);

                discontentment += goal.GetDiscontentment(newValue);
            }

            return discontentment;
        }

        public virtual Action GetNextAction()
        {
            Action action = null;
            //returns the next action that can be executed or null if no more executable actions exist
            if (this.ActionEnumerator.MoveNext())
            {
                action = this.ActionEnumerator.Current;
            }

            while (action != null && !action.CanExecute(this))
            {
                if (this.ActionEnumerator.MoveNext())
                {
                    action = this.ActionEnumerator.Current;    
                }
                else
                {
                    action = null;
                }
            }

            return action;
        }

        public virtual Action[] GetExecutableActions()
        {
            return this.Actions.Where(a => a.CanExecute(this)).ToArray();
        }

        public virtual bool IsTerminal()
        {
            return true;
        }
        

        public virtual float GetScore()
        {
            return 0.0f;
        }

        public virtual int GetNextPlayer()
        {
            return 0;
        }

        public virtual void CalculateNextPlayer()
        {
        }*//*
    }
}
*/
using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.ForwardModel
{
    public class WorldModel
    {
        private List<Action> Actions { get; set; }
        protected IEnumerator<Action> ActionEnumerator { get; set; }
        private Dictionary<string, float> GoalValues { get; set; }

        private int MANA { get; set; }
        private int HP { get; set; }
        private int ShieldHP { get; set; }
        private int MAXHP { get; set; }
        private int XP { get; set; }
        private float TIME { get; set; }
        private int MONEY { get; set; }
        private int LEVEL { get; set; }
        private Vector3 POSITION { get; set; }

        private bool SKELETON1;
        private bool SKELETON2;
        private bool ORC1;
        private bool ORC2;
        private bool DRAGON;
        private bool CHEST1;
        private bool CHEST2;
        private bool CHEST3;
        private bool CHEST4;
        private bool CHEST5;
        private bool HEALTHPOTION1;
        private bool HEALTHPOTION2;
        private bool MANAPOTION1;
        private bool MANAPOTION2;

        public WorldModel(List<Action> actions)
        {
            this.GoalValues = new Dictionary<string, float>();

            // Integrates more randomness in choosing actions
            this.Actions = new List<Action>(actions);
            RandomHelper.Shuffle(this.Actions);
            this.ActionEnumerator = actions.GetEnumerator();
        }

        public WorldModel(WorldModel parent)
        {
            this.MANA =			(int)parent.GetProperty(Properties.MANA);
            this.HP =			(int)parent.GetProperty(Properties.HP);
            this.ShieldHP =		(int)parent.GetProperty(Properties.ShieldHP);
            this.MAXHP =		(int)parent.GetProperty(Properties.MAXHP);
            this.XP =			(int)parent.GetProperty(Properties.XP);
            this.TIME =			(float)parent.GetProperty(Properties.TIME);
            this.MONEY =		(int)parent.GetProperty(Properties.MONEY);
            this.LEVEL =		(int)parent.GetProperty(Properties.LEVEL);
            this.POSITION =		(Vector3)parent.GetProperty(Properties.POSITION);

            this.SKELETON1 =	(bool)parent.GetProperty(Properties.SKELETON1);
            this.SKELETON2 =	(bool)parent.GetProperty(Properties.SKELETON2);
            this.ORC1 =			(bool)parent.GetProperty(Properties.ORC1);
            this.ORC2 =			(bool)parent.GetProperty(Properties.ORC2);
            this.DRAGON =		(bool)parent.GetProperty(Properties.DRAGON);
            this.CHEST1 =		(bool)parent.GetProperty(Properties.CHEST1);
            this.CHEST2 =		(bool)parent.GetProperty(Properties.CHEST2);
            this.CHEST3 =		(bool)parent.GetProperty(Properties.CHEST3);
            this.CHEST4 =		(bool)parent.GetProperty(Properties.CHEST4);
            this.CHEST5 =		(bool)parent.GetProperty(Properties.CHEST5);
            this.HEALTHPOTION1= (bool)parent.GetProperty(Properties.HEALTHPOTION1);
            this.HEALTHPOTION2= (bool)parent.GetProperty(Properties.HEALTHPOTION2);
            this.MANAPOTION1 =	(bool)parent.GetProperty(Properties.MANAPOTION1);
            this.MANAPOTION2 =	(bool)parent.GetProperty(Properties.MANAPOTION2);

            this.Actions = parent.Actions;
            this.ActionEnumerator = this.Actions.GetEnumerator();
        }

        public virtual object GetProperty(string propertyName)
        {

            switch (propertyName)
            {
                case Properties.MANA:
                    return this.MANA;
                case Properties.HP:
                    return this.HP;
                case Properties.ShieldHP:
                    return this.ShieldHP;
                case Properties.MAXHP:
                    return this.MAXHP;
                case Properties.XP:
                    return this.XP;
                case Properties.TIME:
                    return this.TIME;
                case Properties.MONEY:
                    return this.MONEY;
                case Properties.LEVEL:
                    return this.LEVEL;
                case Properties.POSITION:
                    return this.POSITION;
                case Properties.SKELETON1:
                    return this.SKELETON1;
                case Properties.SKELETON2:
                    return this.SKELETON2;
                case Properties.ORC1:
                    return this.ORC1;
                case Properties.ORC2:
                    return this.ORC2;
                case Properties.DRAGON:
                    return this.DRAGON;
                case Properties.CHEST1:
                    return this.CHEST1;
                case Properties.CHEST2:
                    return this.CHEST2;
                case Properties.CHEST3:
                    return this.CHEST3;
                case Properties.CHEST4:
                    return this.CHEST4;
                case Properties.CHEST5:
                    return this.CHEST5;
                case Properties.HEALTHPOTION1:
                    return this.HEALTHPOTION1;
                case Properties.HEALTHPOTION2:
                    return this.HEALTHPOTION2;
                case Properties.MANAPOTION1:
                    return this.MANAPOTION1;
                case Properties.MANAPOTION2:
                    return this.MANAPOTION2;
                default:
                    Debug.Log(propertyName);
                    return null;
            }
        }

        public virtual void SetProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case Properties.MANA:
                    this.MANA = (int)value;
                    break;
                case Properties.HP:
                    this.HP = (int)value;
                    break;
                case Properties.ShieldHP:
                    this.ShieldHP = (int)value;
                    break;
                case Properties.MAXHP:
                    this.MAXHP = (int)value;
                    break;
                case Properties.XP:
                    this.XP = (int)value;
                    break;
                case Properties.TIME:
                    this.TIME = (float)value;
                    break;
                case Properties.MONEY:
                    this.MONEY = (int)value;
                    break;
                case Properties.LEVEL:
                    this.LEVEL = (int)value;
                    break;
                case Properties.POSITION:
                    this.POSITION = (Vector3)value;
                    break;
                case Properties.SKELETON1:
                    this.SKELETON1 = (bool)value;
                    break;
                case Properties.SKELETON2:
                    this.SKELETON2 = (bool)value;
                    break;
                case Properties.ORC1:
                    this.ORC1 = (bool)value;
                    break;
                case Properties.ORC2:
                    this.ORC2 = (bool)value;
                    break;
                case Properties.DRAGON:
                    this.DRAGON = (bool)value;
                    break;
                case Properties.CHEST1:
                    this.CHEST1 = (bool)value;
                    break;
                case Properties.CHEST2:
                    this.CHEST2 = (bool)value;
                    break;
                case Properties.CHEST3:
                    this.CHEST3 = (bool)value;
                    break;
                case Properties.CHEST4:
                    this.CHEST4 = (bool)value;
                    break;
                case Properties.CHEST5:
                    this.CHEST5 = (bool)value;
                    break;
                case Properties.HEALTHPOTION1:
                    this.HEALTHPOTION1 = (bool)value;
                    break;
                case Properties.HEALTHPOTION2:
                    this.HEALTHPOTION2 = (bool)value;
                    break;
                case Properties.MANAPOTION1:
                    this.MANAPOTION1 = (bool)value;
                    break;
                case Properties.MANAPOTION2:
                    this.MANAPOTION2 = (bool)value;
                    break;
                default:
                    Debug.Log(propertyName);
                    break;
            }
        }

        public virtual float GetGoalValue(string goalName)
        {
            /*//recursive implementation of WorldModel
            if (this.GoalValues.ContainsKey(goalName))
            {
                return this.GoalValues[goalName];
            }
            else if (this.Parent != null)
            {
                return this.Parent.GetGoalValue(goalName);
            }
            else
            {
                return 0;
            }*/
            return 0;
        }

        public virtual void SetGoalValue(string goalName, float value)
        {
            /*var limitedValue = value;
            if (value > 10.0f)
            {
                limitedValue = 10.0f;
            }

            else if (value < 0.0f)
            {
                limitedValue = 0.0f;
            }

            this.GoalValues[goalName] = limitedValue;*/
        }

        public virtual WorldModel GenerateChildWorldModel()
        {
            return new WorldModel(this);
        }

        public float CalculateDiscontentment(List<Goal> goals)
        {
            /*var discontentment = 0.0f;

            foreach (var goal in goals)
            {
                var newValue = this.GetGoalValue(goal.Name);

                discontentment += goal.GetDiscontentment(newValue);
            }

            return discontentment;*/
            return 0;
        }

        public virtual Action GetNextAction()
        {
            Action action = null;
            //returns the next action that can be executed or null if no more executable actions exist
            if (this.ActionEnumerator.MoveNext())
            {
                action = this.ActionEnumerator.Current;
            }

            while (action != null && !action.CanExecute(this))
            {
                if (this.ActionEnumerator.MoveNext())
                {
                    action = this.ActionEnumerator.Current;
                }
                else
                {
                    action = null;
                }
            }

            return action;
        }

        public virtual Action[] GetExecutableActions()
        {
            return this.Actions.Where(a => a.CanExecute(this)).ToArray();
        }

        public virtual bool IsTerminal()
        {
            return true;
        }


        public virtual float GetScore()
        {
			int money = this.MONEY;
			int HP = this.HP;
			float time = this.TIME;

			if (HP <= 0) return 0.0f;
			else if (money == 25)
			{
				return (1.0f - time * 0.005f);
			}
			else return 0.0f;
		}

        public virtual int GetNextPlayer()
        {
            return 0;
        }

        public virtual void CalculateNextPlayer()
        {
        }
    }
}

