using Game.Behaviours;
using Game.Components;
using Unity.Collections;
using Unity.Entities;

namespace Game.Systems
{
    public partial class ProcessInventoryClickSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            Entity inventory = Entity.Null;
            const int invalidIndex = -1;
            int index = invalidIndex;
            Entities.ForEach((Entity entity, in InventorySlotClickedEvent clicked) =>
            {
                var view = EntityManager.GetComponentObject<ItemView>(clicked.itemView);

                inventory = view.inventoryEntity;
                index = view.slotIndex;
                commandBuffer.DestroyEntity(entity);
            }).WithoutBurst().Run();

            if (inventory != null && index > invalidIndex)
            {
                Entities.ForEach((Entity entity, ItemDescriptionView view) =>
                {
                    commandBuffer.AddComponent(entity, new NeedsItemDescriptionUpdate
                    {
                        index = index,
                        entityInventory = inventory
                    });
                }).WithoutBurst().Run();
            }

            commandBuffer.Playback(EntityManager);
            commandBuffer.Dispose();
        }
    }
}