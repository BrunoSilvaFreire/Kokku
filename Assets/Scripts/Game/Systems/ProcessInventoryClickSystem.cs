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
                if (!Items.GetItemElementOfView(EntityManager, view).IsEmpty())
                {
                    inventory = view.inventoryEntity;
                    index = view.slotIndex;
                    AudioUtility.PlayClipOnItemView(view, UIConfiguration.Instance.ItemClickSound);
                }
                else
                {
                    view.animator.SetTrigger(ItemView.InvalidItemKey);
                    AudioUtility.PlayClipOnItemView(view, UIConfiguration.Instance.InvalidClickSound);
                }
                commandBuffer.DestroyEntity(entity);
            }).WithoutBurst().Run();

            if (inventory != null && index > invalidIndex)
            {
                Entities.WithAll<ItemDescriptionView>().ForEach((Entity entity) =>
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