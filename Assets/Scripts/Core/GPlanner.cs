using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.Build;

namespace GOAP
{
    public class Node
    {
        public Node parent;
        public float cost;
        public Dictionary<string, int> state;
        public GAction action;

        public Node(Node parent, float cost, Dictionary<string, int> allStates, GAction action)
        {
            this.parent = parent;
            this.cost = cost;
            //we need to create a new copy of states
            this.state = new Dictionary<string, int>(allStates);
            this.action = action;
        }
    }
}

namespace GOAP
{
    public class GPlanner
    {
        public Queue<GAction> Plan(List<GAction> actions, Dictionary<string, int> desiredGoals, WorldStates worldStates)
        {
            //we find usable actions
            List<GAction> usableActions = new List<GAction>();
            foreach (GAction a in actions)
            {
                if (a.IsAchievable())
                {
                    usableActions.Add(a);
                }
            }

            //First leaves of our graph
            List<Node> leaves = new List<Node>();
            //First node
            Node start = new Node(null, 0, GWorld.Instance.GetWorld().GetStates(), null);
            //building a graph and looking to find a plan
            bool success = BuildGraph(start, leaves, usableActions, desiredGoals);
            //if we dont find a plan
            if (!success)
            {
                Debug.Log("NO PLAN");
                return null;
            }

            //checking for the cheapest path - at bottom node of graph
            //we added up cost in BuildGraph() method so the last/bottom node has combined cost of all nodes - hence we can compare the cost of plan
            Node cheapest = null;
            foreach (Node leaf in leaves)
            {
                if (cheapest == null)
                {
                    cheapest = leaf;
                }
                else
                {
                    if (leaf.cost < cheapest.cost)
                    {
                        cheapest = leaf;
                    }
                }
            }
            List<GAction> result = new List<GAction>();
            Node n = cheapest;
            //we are moving up the graph
            while (n != null)
            {
                //will be null when we hit starting node
                if (n.action != null)
                {
                    result.Insert(0, n.action);
                }
                n = n.parent;
            }

            Queue<GAction> queue = new Queue<GAction>();
            foreach (GAction action in result)
            {
                queue.Enqueue(action);
            }

            //Only for debug purpose
            Debug.Log("The Plan is : ");
            foreach (var item in queue)
            {
                Debug.Log("Q : " + item.actionName);
            }

            return queue;
        }

        //Building an graph of Node(contains actions , cost etc) - to make a viable plan to complete a goal
        private bool BuildGraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<string, int> goals)
        {
            bool foundPath = false;
            //for each action in useable
            foreach (GAction action in usableActions)
            {
                //if action is achievable with parent state
                if (action.IsAchievableGiven(parent.state))
                {
                    Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                    foreach (KeyValuePair<string, int> eff in action.effectsDict)
                    {
                        //Adding effect of current node in currentState - if they are not already present
                        if (!currentState.ContainsKey(eff.Key))
                        {
                            currentState.Add(eff.Key, eff.Value);
                        }
                    }

                    //we are adding up cost - as we move form one to next node
                    Node nextNode = new Node(parent, parent.cost + action.cost, currentState, action);

                    //terminate if we have achieved goal
                    if (GoalAchieved(goals, currentState))
                    {
                        leaves.Add(nextNode);
                        foundPath = true;
                    }
                    else
                    {
                        //move to next recursion loop
                        //we are removing the action from list of useable actions
                        List<GAction> subset = ActionSubset(usableActions, action);
                        bool found = BuildGraph(nextNode, leaves, subset, goals);
                        if (found)
                        {
                            foundPath = true;
                        }
                    }

                }

            }
            return foundPath;
        }


        private bool GoalAchieved(Dictionary<string, int> goals, Dictionary<string, int> state)
        {
            //we just wanna make sure both matches
            foreach (KeyValuePair<string, int> g in goals)
            {
                if (!state.ContainsKey(g.Key))
                {
                    return false;//we 
                }
            }
            return true;
        }

        private List<GAction> ActionSubset(List<GAction> actions, GAction removeMe)
        {
            List<GAction> subset = new List<GAction>();
            foreach (GAction action in actions)
            {
                //ignore one and add all others
                if (!action.Equals(removeMe))
                {
                    subset.Add(action);
                }
            }
            return subset;
        }

    }
}
