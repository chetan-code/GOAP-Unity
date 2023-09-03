using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/// <summary>
/// Class take care of multiple goals 
/// NOTE : an agent can have a single goal - but this class will
/// help us manage multiple goals
/// </summary>
/// namespace GOAP
namespace GOAP
{
    public class SubGoal
    {
        public Dictionary<string, int> sgoals; //sub goals that may exist
        public bool remove;//do we remove sub goals if they are done ?

        public SubGoal(string key, int value, bool remove)
        {
            this.sgoals = new Dictionary<string, int>();
            this.sgoals.Add(key, value);
            this.remove = remove;
        }
    }
}

namespace GOAP
{
    public class GAgent : MonoBehaviour
    {
        public List<GAction> actions = new List<GAction>();//actions that agent need to achieve
        public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();//goals that agent need to achieve
        public WorldStates beliefs = new WorldStates();
        GPlanner planner;
        Queue<GAction> actionQueue;//queue of actions - agent will perform sequentially
        public GAction currentAction;//actions currently performed by agent
        public SubGoal currentGoal;
        bool invoked = false;
        protected virtual void Start()
        {
            //all the action added to agent will be cached
            GAction[] acts = this.GetComponents<GAction>();
            foreach (GAction act in acts)
            {
                actions.Add(act);
            }
        }

        private void LateUpdate()
        {
            //if we are already in actions 
            if (currentAction != null && currentAction.running)
            {
                float distanceToTarget = Vector3.Distance(currentAction.target.transform.position, this.transform.position);
                if (currentAction.agent.hasPath && distanceToTarget < 2f)
                {
                    //Debug.Log("running current action");
                    if (!invoked)
                    {
                        Invoke(nameof(CompleteAction), currentAction.duration);
                        invoked = true;
                    }
                }
                return;
            }


            //add planner
            if (planner == null || actionQueue == null)
            {
                //Debug.Log("Creating new GPlannner");
                planner = new GPlanner();
                //sort order - and find most important
                var sortedGoals = from entry in goals orderby entry.Value descending select entry;
                //Debug.Log("Sorted goal entry is : " + sortedGoals);
                foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
                {
                    actionQueue = planner.Plan(actions, sg.Key.sgoals, null);
                    //Debug.Log("Creating new Action queue : " + actionQueue);
                    if (actionQueue != null)
                    {
                        currentGoal = sg.Key;
                        //Debug.Log("Set new goal to : " + sg.Key);
                        break;
                    }
                }
            }

            //goal is removable and now we need a new plan
            if (actionQueue != null && actionQueue.Count == 0)
            {
                //Debug.Log("Removing goal");
                if (currentGoal.remove)
                {
                    goals.Remove(currentGoal);
                    Debug.Log("Goal removed");
                }
                planner = null;
            }

            //performing actions - just moving to destination in this case
            if (actionQueue != null && actionQueue.Count > 0)
            {
                //Debug.Log("Performing action");
                currentAction = actionQueue.Dequeue();
                if (currentAction.PrePerform())
                {
                    //Debug.Log("Preperform");
                    if (currentAction.target == null && currentAction.targetTag != "")
                    {
                        currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
                        // Debug.Log("found target with tag");
                    }
                    if (currentAction.target != null)
                    {
                        currentAction.running = true;
                        currentAction.agent.SetDestination(currentAction.target.transform.position);
                        //Debug.Log("Seting new destination to : " + currentAction.target.name);
                    }
                }
                else
                {
                    actionQueue = null;
                    //Debug.Log("Action Queue is null --- lol");
                }
            }

        }

        private void CompleteAction()
        {
            currentAction.running = false;
            currentAction.PostPerform();
            invoked = false;
            Debug.Log("Action Completed : " + currentAction.actionName);
        }
    }
}
