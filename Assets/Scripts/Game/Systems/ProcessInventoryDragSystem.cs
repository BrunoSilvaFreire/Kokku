using Game.Behaviours;
using Game.Components;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Game.Systems
{
    public partial class ProcessInventoryDragSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var commandBuffer = new EntityCommandBuffer(Allocator.TempJob);
            var hasDragEntity = SystemAPI.TryGetSingletonEntity<IsItemBeingDraggedTag>(out var dragEntity);
            // Handle drag begin events
            Entities.WithAll<InventorySlotDragBeginEvent>().ForEach(
                (Entity entity, ref InventorySlotDragBeginEvent dragBeginEvent) =>
                {
                    var element = Items.GetItemElementOfView(EntityManager, dragBeginEvent.itemView);
                    EntityManager.CreateSingleton<IsItemBeingDraggedTag>();
                    if (Items.TryFindDefinition(element, out var definition))
                    {
                        var wisp = Object.Instantiate(UIConfiguration.Instance.DraggingImagePrefab);
                        wisp.sprite = definition.Thumbnail;
                        wisp.transform.SetParent(GlobalStage.Instance.Canvas.transform);

                        commandBuffer.AddComponent(
                            dragBeginEvent.itemView,
                            new DraggingItem
                            {
                                thumbnail = wisp,
                            }
                        );
                    }

                    commandBuffer.DestroyEntity(entity);
                }).WithStructuralChanges().WithoutBurst().Run();
            // Handle drag end events
            var hasDestination = SystemAPI.TryGetSingleton<DragTether>(out var destination);
            Entities.ForEach(
                (Entity entity, ref InventorySlotDragEndEvent dragEndEvent) =>
                {
                    var srcEntity = dragEndEvent.itemView;
                    var destEntity = destination.destinationItemView;
                    if (hasDragEntity)
                    {
                        commandBuffer.DestroyEntity(dragEntity);
                    }


                    if (EntityManager.HasComponent<DraggingItem>(srcEntity))
                    {
                        var draggingItem = EntityManager.GetComponentObject<DraggingItem>(srcEntity);
                        Object.Destroy(draggingItem.thumbnail.gameObject);

                        if (hasDestination)
                        {
                            var transferEntity = commandBuffer.CreateEntity();
                            var src = EntityManager.GetComponentData<ItemView>(srcEntity);
                            var dest = EntityManager.GetComponentData<ItemView>(destEntity);
                            commandBuffer.AddComponent(
                                transferEntity,
                                new TransferItemEventComponent
                                {
                                    from = new TransferReference
                                    {
                                        inventory = src.inventoryEntity,
                                        index = src.slotIndex,
                                        itemView = srcEntity
                                    },
                                    to = new TransferReference
                                    {
                                        inventory = dest.inventoryEntity,
                                        index = dest.slotIndex,
                                        itemView = destEntity
                                    }
                                }
                            );
                        }

                        commandBuffer.RemoveComponent<DraggingItem>(srcEntity);
                    }

                    commandBuffer.DestroyEntity(entity);
                }).WithoutBurst().Run();

            commandBuffer.Playback(EntityManager);
            commandBuffer.Dispose();
        }
    }
}