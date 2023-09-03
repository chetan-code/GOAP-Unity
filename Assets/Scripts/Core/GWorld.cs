using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace GOAP
{
    /// <summary>
    /// a sealed class is a class that cannot be inherited 
    /// or used as a base class for other classes.
    /// </summary>
    public sealed class GWorld
    {
        private static readonly GWorld instance = new GWorld();

        private static WorldStates world;

        //constructor
        static GWorld()
        {
            world = new WorldStates();
        }

        private GWorld()
        {

        }

        public static GWorld Instance
        {
            get { return instance; }
        }


        public WorldStates GetWorld()
        {
            return world;
        }

    }
}
