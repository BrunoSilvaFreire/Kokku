using Game.Behaviours;
using Game.Components;
using Unity.Collections;
using Unity.Entities;

namespace Game.Systems
{
    public struct Selection : IComponentData
    {
        public Entity currentlyDragging;
    }

    public partial class ProcessInputChanges : SystemBase
    {
        protected override void OnUpdate()
        {
            if (!SystemAPI.TryGetSingleton<Selection>(out var selection))
            {
                EntityManager.CreateSingleton(new Selection());
                selection = SystemAPI.GetSingleton<Selection>();
            }

            var commandBuffer = new EntityCommandBuffer(Allocator.TempJob);
            foreach (var (slotClickedEvent, entity) in SystemAPI.Query<InventorySlotClickedEvent>()
                         .WithEntityAccess())
            {
                if (selection.currentlyDragging == Entity.Null)
                {
                    selection.currentlyDragging = slotClickedEvent.slot;
                }
                else
                {
                    var src = EntityManager.GetComponentObject<ItemView>(selection.currentlyDragging);
                    var dest = EntityManager.GetComponentObject<ItemView>(slotClickedEvent.slot);


                    var transferEntity = commandBuffer.CreateEntity();
                    commandBuffer.AddComponent(transferEntity, new TransferItemEventComponent
                        {
                            from = new TransferReference
                            {
                                inventory = src.inventoryEntity,
                                index = src.slotIndex,
                                itemView = selection.currentlyDragging
                            },
                            to = new TransferReference
                            {
                                inventory = dest.inventoryEntity,
                                index = dest.slotIndex,
                                itemView = slotClickedEvent.slot
                            }
                        }
                    );
                    selection.currentlyDragging = Entity.Null;
                }

                SystemAPI.SetSingleton(selection);

                commandBuffer.DestroyEntity(entity);
            }

            commandBuffer.Playback(EntityManager);
            commandBuffer.Dispose();
        }
    }
}