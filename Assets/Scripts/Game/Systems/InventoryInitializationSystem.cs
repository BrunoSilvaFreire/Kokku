using System;
using Game.Behaviours;
using Game.Components;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Game.Systems
{
    public partial class InventoryInitializationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var buffer = new EntityCommandBuffer(Allocator.TempJob);
            Entities.ForEach((Entity entity, ItemView view, in NeedsInventoryInitialization init) =>
            {
                if (view.inventoryEntity == Entity.Null)
                {
                    Debug.LogError($"View {entity} references null {view.inventoryEntity}");
                    buffer.DestroyEntity(entity);
                    return;
                }

                var inventoryView = InventoryView.FindViewOfType(init.type);
                if (inventoryView == null)
                {
                    Debug.LogError($"Unable to find InventoryView of type {init.type:G}");
                    return;
                }

                var slot = view.slotIndex;
                if (slot < 0 || slot >= inventoryView.transform.childCount)
                {
                    Debug.LogError($"Inventory slot {slot} is out of bounds, inventory {inventoryView} only supports {inventoryView.transform.childCount} slots.");
                    buffer.DestroyEntity(entity);
                    return;
                }

                var child = inventoryView.transform.GetChild(slot);
                if (!child.TryGetComponent<InventorySlot>(out var component))
                {
                    Debug.LogWarning($"Unable to find InventorySlot #{slot} within InventoryView {inventoryView}");
                    buffer.DestroyEntity(entity);
                    return;
                }

                component.SetEntity(entity);
                view.thumbnail = component.Thumbnail;
                view.animator = component.Animator;

                var itemBuffer = EntityManager.GetBuffer<ItemElement>(view.inventoryEntity);
                if (slot >= itemBuffer.Length)
                {
                    Debug.LogWarning($"Inventory {view.inventoryEntity} only has {itemBuffer.Length} items.");
                    buffer.DestroyEntity(entity);
                    return;
                }
                var element = itemBuffer[slot];
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

                EntityManager.RemoveComponent<NeedsInventoryInitialization>(entity);
            }).WithStructuralChanges().WithoutBurst().Run();
            buffer.Playback(EntityManager);
            buffer.Dispose();
        }
    }
}