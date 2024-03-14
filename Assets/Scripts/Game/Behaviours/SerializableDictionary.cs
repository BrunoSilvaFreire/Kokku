using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Behaviours
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector] private List<TKey> keys;

        [SerializeField, HideInInspector] private List<TValue> values;

        public void OnBeforeSerialize()
        {
            Copy(Keys, ref keys);
            Copy(Values, ref values);
        }

        private void Copy<T>(IEnumerable<T> from, ref List<T> to)
        {
            var collection = from.ToArray();
            if (to == null)
            {
                to = new List<T>(collection.Length);
            }
            else
            {
                to.Clear();
                to.Capacity = collection.Length;
            }

            to.AddRange(collection);
        }

        public void OnAfterDeserialize()
        {
            for (var i = 0; i < keys.Count; i++)
            {
                this[keys[i]] = values[i];
            }

            keys.Clear();
            values.Clear();
        }
    }
}