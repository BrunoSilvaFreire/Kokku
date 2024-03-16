using System;
using UnityEngine;

namespace Game.Behaviours
{

    public class ScriptableSingletonNotFoundException<T> : Exception
    {
        public ScriptableSingletonNotFoundException() : base($"Unable to load scriptable singleton of type {typeof(T).FullName}")
        {
        }
    }

    public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableSingleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<T>(typeof(T).Name);
                    if (_instance == null)
                    {
                        throw new ScriptableSingletonNotFoundException<T>();
                    }
                }

                return _instance;
            }
        }

    }

}