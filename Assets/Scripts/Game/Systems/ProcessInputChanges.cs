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
            Entities.ForEach((in InventorySlotClickedEvent slotClickedEvent) =>
            {
                if (selection.currentlyDragging == Entity.Null)
                {
                    selection.currentlyDragging = slotClickedEvent.slot;
                }
                else
                {
                    var transferEntity = commandBuffer.CreateEntity();
                    var src = EntityManager.GetComponentObject<ItemView>(selection.currentlyDragging);
                    var dest = EntityManager.GetComponentObject<ItemView>(slotClickedEvent.slot);


                    commandBuffer.AddComponent(transferEntity, new TransferItemEventComponent
                        {
                            fromInventory = src.inventoryEntity,
                            fromIndex = src.slotIndex,
                            toInventory = dest.inventoryEntity,
                            toIndex = dest.slotIndex
                        }
                    );
                }
            }).WithoutBurst().Run();
            commandBuffer.Playback(EntityManager);
            commandBuffer.Dispose();
        }
    }
}