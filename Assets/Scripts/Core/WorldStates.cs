using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldState{
    public string key;
    public int value;
}

/// <summary>
/// World state keeps track of of world states and related funtions
/// </summary>

namespace GOAP
{
    public class WorldStates
    {
        public Dictionary<string, int> states;

        public WorldStates()
        {
            states = new Dictionary<string, int>();
        }

        //Methods to add/remove/modify world states

        public bool HasState(string key)
        {
            return states.ContainsKey(key);
        }

        public void AddState(string key, int value)
        {
            states.Add(key, value);
        }

        public void ModifyState(string key, int value)
        {
            if (HasState(key))
            {
                states[key] += value;
                //remove the state if its value is less/equal to zero
                if (states[key] <= 0)
                {
                    RemoveState(key);
                }
            }
            else
            {
                //if the state doesnt exist - we add it to states
                states.Add(key, value);
            }
        }


        public void RemoveState(string key)
        {
            if (HasState(key))
            {
                states.Remove(key);
            }
        }

        public void SetState(string key, int value)
        {
            if (HasState(key))
            {
                states[key] = value;
            }
            else
            {
                AddState(key, value);
            }
        }

        public Dictionary<string, int> GetStates()
        {
            return states;
        }

    }
}
