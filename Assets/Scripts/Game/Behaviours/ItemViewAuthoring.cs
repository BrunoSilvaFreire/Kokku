using System;
using Game.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Behaviours
{
    public class ItemViewAuthoring : MonoBehaviour
    {

        public class ItemViewBaker : Baker<ItemViewAuthoring>
        {
            public override void Bake(ItemViewAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var owner = GetComponentInParent<InventoryComponentAuthoring>();
                if (owner == null)
                {
                    throw new Exception($"Unable to find authoring inventory");
                }
                var inventoryEntity = GetEntity(owner, TransformUsageFlags.None);
                if (inventoryEntity == Entity.Null)
                {
                    throw new Exception($"Unable to get entity of authoring inventory {inventoryEntity} ({owner})");
                }
                AddComponentObject(
                    entity,
                    new ItemView
                    {
                        slotIndex = authoring.transform.GetSiblingIndex(),
                        inventoryEntity = inventoryEntity,
                    }
                );
                AddComponentObject(entity, new NeedsInventoryInitialization
                {
                    type = owner.Type
                });
            }
        }
    }
}