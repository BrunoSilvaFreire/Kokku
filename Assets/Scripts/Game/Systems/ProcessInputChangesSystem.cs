using Game.Behaviours;
using Game.Components;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Game.Systems
{
    public partial class ProcessInputChangesSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

            // Handle drag begin events
            Entities.WithAll<InventorySlotDragBeginEvent>().ForEach(
                (Entity entity, ref InventorySlotDragBeginEvent dragBeginEvent) =>
                {
                    var wisp = Object.Instantiate(UIAssetsConfiguration.Instance.DraggingImagePrefab);
                    var element = EntityManager.GetItemElementOfView(dragBeginEvent.itemView);
                    wisp.sprite = element.FindDefinition().Thumbnail;
                    wisp.transform.SetParent(GlobalStage.Instance.Canvas.transform);

                    commandBuffer.AddComponent(dragBeginEvent.itemView, new DraggingItem
                    {
                        itemView = dragBeginEvent.itemView,
                        thumbnail = wisp,
                        originalSlotIndex = dragBeginEvent.slotIndex
                    });

                    commandBuffer.DestroyEntity(entity);
                }).WithoutBurst().Run();
            // Handle drag end events
            var hasDestination = SystemAPI.TryGetSingleton<DragTether>(out var destination);
            Entities.WithAll<InventorySlotDragEndEvent>().ForEach(
                (Entity entity, ref InventorySlotDragEndEvent dragEndEvent) =>
                {
                    var srcEntity = dragEndEvent.itemView;
                    var destEntity = destination.destinationItemView;

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
                    commandBuffer.DestroyEntity(entity);
                }).WithoutBurst().Run();

            commandBuffer.Playback(EntityManager);
            commandBuffer.Dispose();
        }
    }
}