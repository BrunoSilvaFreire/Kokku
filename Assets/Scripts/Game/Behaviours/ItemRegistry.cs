using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Hash128 = Unity.Entities.Hash128;

namespace Game.Behaviours
{
    [Serializable]
    public class ItemDictionary : SerializableDictionary<Hash128, ItemDefinition>
    {
    }

    [CreateAssetMenu]
    public class ItemRegistry : ScriptableSingleton<ItemRegistry>
    {
        [SerializeField] private ItemDictionary _items;
        public bool TryGet(Hash128 hash, out ItemDefinition itemDefinition)
        {
            return _items.TryGetValue(hash, out itemDefinition);
        }

        public IReadOnlyCollection<ItemDefinition> AllItems => _items.Values;
        public IEnumerable<KeyValuePair<Hash128, ItemDefinition>> All => _items;
#if UNITY_EDITOR
        public void EditorOnlyRegenerateHashes()
        {
            var newDict = new ItemDictionary();
            foreach (var item in _items.Values)
            {
                var hash = new Hash128(GUID.Generate().ToString(), true);
                newDict[hash] = item;
            }

            _items = newDict;
        }

        public void EditorOnlySet(Hash128 hash, ItemDefinition itemDefinition)
        {
            _items[hash] = itemDefinition;
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ItemRegistry))]
    public class ItemRegistryEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Button to regenerate hashes
            if (GUILayout.Button("Generate New Hashes"))
            {
                ((ItemRegistry)target).EditorOnlyRegenerateHashes();
            }

            // New button to add all instances of ItemDefinition
            if (GUILayout.Button("Add All ItemDefinitions"))
            {
                AddAllItemDefinitions();
            }
        }
        private void AddAllItemDefinitions()
        {
            // Ensure changes are registered in the undo system
            Undo.RecordObject(target, "Add All ItemDefinitions");

            var registry = (ItemRegistry)target;
            var guids = AssetDatabase.FindAssets("t:ItemDefinition"); // Find all assets of type ItemDefinition

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var itemDefinition = AssetDatabase.LoadAssetAtPath<ItemDefinition>(path);

                if (itemDefinition != null)
                {
                    // Generate a new hash for the itemDefinition
                    var hash = new Hash128(GUID.Generate().ToString(), true);

                    // Add the itemDefinition to the registry if not already present
                    registry.EditorOnlySet(hash, itemDefinition);
                }
            }

            // Save changes to the registry
            EditorUtility.SetDirty(target);
        }
    }
#endif
}