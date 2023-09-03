using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP
{
    public abstract class GAction : MonoBehaviour
    {
        public string actionName = "Action";  //name of actions
        public float cost = 1.0f; //cost of action - agent will use most cost effective plan
        public GameObject target; //any target for angent to travel to
        public string targetTag;//if not a taget - tag of taget can be used
        public float duration = 0;//time taken to complete the action

        //Conditionsor states that need to be fullfilled before this action is started
        public WorldState[] preConditions;
        //Conditions or states that the world will be in after action is complete
        public WorldState[] afterEffects;
        public NavMeshAgent agent; //we use namvesh to transverse the area

        //above preConditions and afterEffects are fed into dictionary for better performance  
        //this is used as dictionary are not serializable and wont appear in inspector
        public Dictionary<string, int> preConditionsDict;
        public Dictionary<string, int> effectsDict;

        public WorldStates agentBeliefs;

        public bool running = false;//running an action or not

        public GAction()
        {
            //intializing dict
            preConditionsDict = new Dictionary<string, int>();
            effectsDict = new Dictionary<string, int>();
        }

        private void Awake()
        {
            agent = this.gameObject.GetComponent<NavMeshAgent>();
            //we have defined some condtion/effects in inspector -> add it to dict
            if (preConditions != null)
            {
                for (int i = 0; i < preConditions.Length; i++)
                {
                    var cond = preConditions[i];
                    preConditionsDict.Add(cond.key, cond.value);
                }
            }

            if (afterEffects != null)
            {
                for (int i = 0; i < afterEffects.Length; i++)
                {
                    var effect = afterEffects[i];
                    effectsDict.Add(effect.key, effect.value);
                }
            }
        }

        /// <summary>
        /// If we can achieve the goal?
        /// </summary>
        /// <returns></returns>
        public bool IsAchievable()
        {
            return true;  //TODO
        }

        /// <summary>
        /// Is goal achievable with given precondiitons?
        /// </summary>
        /// <param name="conditions"> provided conditions</param>
        /// <returns>is goal achieable or not?</returns>
        public bool IsAchievableGiven(Dictionary<string, int> conditions)
        {
            for (int i = 0; i < preConditionsDict.Count; i++)
            {
                KeyValuePair<string, int> pair = preConditionsDict.ElementAt(i);
                if (!conditions.ContainsKey(pair.Key))
                {
                    //goal is NOT achievable as preconditions dont match with provided conditions
                    return false;
                }
            }
            //we have matched all preconditions - goal is achievable
            return true;
        }

        /// <summary>
        /// To do checks before an action starts
        /// </summary>
        /// <returns></returns>
        public abstract bool PrePerform();
        /// <summary>
        /// To do checks after an action ends.
        /// </summary>
        /// <returns></returns>
        public abstract bool PostPerform();

    }
}