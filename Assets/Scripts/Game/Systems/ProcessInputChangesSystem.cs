using System;
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

    public partial class ProcessInputChangesSystem : SystemBase
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
                var viewEntity = slotClickedEvent.itemView;
                if (selection.currentlyDragging == Entity.Null)
                {
                    var view = EntityManager.GetComponentData<ItemView>(viewEntity);
                    var selectedItem = EntityManager.GetItemElementOfView(view);
                    if (selectedItem.IsEmpty())
                    {
                        view.animator.SetTrigger(ItemView.InvalidItemKey);
                        
                    }else
                    {
                        selection.currentlyDragging = viewEntity;
                        view.animator.SetBool(ItemView.DraggingKey, true);
                    }
                }
                else
                {
                    var src = EntityManager.GetComponentObject<ItemView>(selection.currentlyDragging);
                    var dest = EntityManager.GetComponentObject<ItemView>(viewEntity);


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
                                itemView = viewEntity
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