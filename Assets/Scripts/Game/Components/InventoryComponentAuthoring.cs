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

        [SerializeField] private bool _addOneOfEachItem = true;

        public class InventoryComponentBaker : Baker<InventoryComponentAuthoring>
        {
            public override void Bake(InventoryComponentAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new InventoryComponent { inventorySize = authoring._inventorySize });
                var buf = AddBuffer<ItemElement>(entity);
                var size = authoring._inventorySize;
                buf.Length = size;
                var empty = new ItemElement
                {
                    type = ItemExtensions.NullHash
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