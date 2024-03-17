using System.Text;
using Game.Behaviours;
using Game.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Systems
{
    public partial class UpdateItemViewStateSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var hasTether = SystemAPI.TryGetSingleton<DragTether>(out var tether);
            var hasDragEntity = SystemAPI.ManagedAPI.TryGetSingleton<DragSource>(out var draggedItem);
            Entities.ForEach((Entity entity, ItemView view) =>
            {
                DescriptionSelection description = default;
                var hasDescription = EntityManager.HasComponent<DescriptionSelection>(view.inventoryEntity);
                if (hasDescription)
                {
                    description = EntityManager.GetComponentData<DescriptionSelection>(view.inventoryEntity);
                }

                var isDraggingSource = hasDragEntity && draggedItem.sourceItemView == entity;
                var itemElement = Items.GetItemElementOfView(EntityManager, view);

                var isDraggingDestination = hasTether && entity == tether.destinationItemView;
                var isSwapping = hasDragEntity && isDraggingSource ^ isDraggingDestination;

                var isDescribing = hasDescription && !itemElement.IsEmpty() && entity == description.describedItemView;
                view.animator.SetBool(ItemView.SwappingKey, isSwapping);
                view.animator.SetBool(ItemView.DescribingKey, isDescribing);
            }).WithoutBurst().Run();
        }
    }
}