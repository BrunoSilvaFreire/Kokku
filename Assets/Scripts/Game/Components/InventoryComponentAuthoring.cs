using System;
using Game.Behaviours;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Components
{
    public class InventoryComponentAuthoring : MonoBehaviour
    {
        [SerializeField] private int _inventorySize;

        [SerializeField] private InventoryType _type = InventoryType.Main;
        [SerializeField] private bool _addOneOfEachItem = true;

        public InventoryType Type => _type;

        public class InventoryComponentBaker : Baker<InventoryComponentAuthoring>
        {
            public override void Bake(InventoryComponentAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new InventoryComponent
                {
                    inventorySize = authoring._inventorySize,
                    type = authoring._type
                });
                var slots = GetComponentsInChildren<ItemViewAuthoring>();
                var buf = AddBuffer<ItemElement>(entity);
                var size = authoring._inventorySize;
                if (slots.Length != size)
                {
                    throw new Exception(
                        $"Inventory size ({size}) does not match number of slots allocated to it ({slots.Length}). " +
                        $"Please make sure that {authoring} has exactly {slots.Length} ItemViewAuthoring components to it. " +
                        "Otherwise this will need to undefined behaviour in the UI."
                    );
                }

                buf.Length = size;
                var empty = new ItemElement
                {
                    type = Items.NullHash
                };
                for (var i = 0; i < size; i++)
                {
                    buf[i] = empty;
                }

                if (authoring._addOneOfEachItem)
                {
                    var i = 0;
                    var registry = ItemRegistry.Instance;
                    if (registry == null)
                    {
                        throw new Exception("Unable to get ItemRegistry");
                    }

                    if (registry.All == null)
                    {
                        throw new Exception("Unable to access all items in registry");
                    }

                    foreach (var (hash, _) in registry.All)
                    {
                        buf[i++] = new ItemElement
                        {
                            type = hash
                        };
                    }
                }
            }
        }
    }
}