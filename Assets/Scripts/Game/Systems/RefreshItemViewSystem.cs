using System;
using Game.Behaviours;
using Game.Components;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Game.Systems
{
    public partial class RefreshItemViewSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var buffer = new EntityCommandBuffer(Allocator.TempJob);
            Entities.ForEach((Entity entity, ItemView view, in NeedsItemRefreshTag _) =>
            {
                if (view.inventoryEntity ==  Entity.Null)
                {
                    Debug.LogWarning($"View {entity} references inventory {view.inventoryEntity}");
                    return;
                }
                var itemBuffer = EntityManager.GetBuffer<ItemElement>(view.inventoryEntity);
                var element = itemBuffer[view.slotIndex];
                var hasItemNow = !element.IsEmpty();
                if (hasItemNow)
                {
                 
                    if (!ItemRegistry.Instance.TryGet(element.type, out var definition))
                    {
                        throw new ArgumentException(
                            $"Item element {element} doesn't have a matching item definition with id {element.type}"
                        );
                    }

                    view.thumbnail.sprite = definition.Thumbnail;
                    if (hasItemNow)
                    {
                        view.animator.SetTrigger(ItemExtensions.ItemEnteringKey);
                    }

                    if (hasItemNow)
                    {
                        buffer.AddComponent<HasItemTag>(entity);
                    }
   
                }
                else
                {
                    view.thumbnail.sprite = null;
                    view.animator.SetTrigger(ItemExtensions.ItemExitingKey);
                }
                EntityManager.RemoveComponent<NeedsItemRefreshTag>(entity);
            }).WithStructuralChanges().WithoutBurst().Run();
            buffer.Playback(EntityManager);
            buffer.Dispose();
        }
    }
}