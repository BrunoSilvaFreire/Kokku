using System;
using System.Collections.Generic;
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
    public class ItemRegistry : ScriptableObject
    {
        [SerializeField] private ItemDictionary _items;
        private static ItemRegistry _instance;

        public static ItemRegistry Instance
        {
            get
            {
                if (_instance != null)
                {
                    _instance = Resources.Load<ItemRegistry>(nameof(ItemRegistry));
                    if (_instance == null)
                    {
                        throw new RegistryNotFoundException<ItemRegistry>();
                    }
                }

                return _instance;
            }
        }

        public bool TryGet(Hash128 hash, out ItemDefinition itemDefinition)
        {
            return _items.TryGetValue(hash, out itemDefinition);
        }

        public IReadOnlyCollection<ItemDefinition> AllItems => _items.Values;
#if UNITY_EDITOR
        public void EditorOnlyRegenerateHashes()
        {
        }

#endif
    }

    public class RegistryNotFoundException<T> : Exception
    {
        public RegistryNotFoundException() : base($"Unable to load registry of type {nameof(T)}")
        {
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(ItemRegistry))]
    public class ItemRegistryEditor : UnityEditor.Editor
    {
        private ReorderableList _list;
        private SerializedProperty _listProperty;

        private void OnEnable()
        {
            _listProperty = serializedObject.FindProperty("_items").FindPropertyRelative("values");
            _list = new ReorderableList(
                serializedObject,
                _listProperty,
                true,
                false,
                true,
                true
            )
            {
                drawElementCallback = (rect, index, active, focused) =>
                {
                    EditorGUI.PropertyField(rect, _listProperty.GetArrayElementAtIndex(index));
                },
                onChangedCallback = _ =>
                {
                    serializedObject.ApplyModifiedProperties();
                } 
            };
        }

        public override void OnInspectorGUI()
        {
            _list.DoLayoutList();
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Generate New Hashes"))
                {
                    ((ItemRegistry)target).EditorOnlyRegenerateHashes();
                }
            }
        }
    }
#endif
}