using System;
using Game.Components;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Behaviours
{
    public class ItemViewAuthoring : MonoBehaviour
    {
        public Image thumbnail;
        public Animator animator;

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
                        thumbnail = authoring.thumbnail,
                        animator = authoring.animator
                    }
                );
                AddComponent<NeedsItemRefreshTag>(entity);
            }
        }
    }
}