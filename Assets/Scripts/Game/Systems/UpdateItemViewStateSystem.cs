using Game.Behaviours;
using Game.Components;
using Unity.Entities;

namespace Game.Systems
{
    /// <summary>
    /// Updates the state of an ItemView, such as whether it is previewing a swap, or being described on the UI
    /// </summary>
    public partial class UpdateItemViewStateSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var hasTether = SystemAPI.TryGetSingleton<DragTether>(out var tether);
            var hasDragEntity = SystemAPI.ManagedAPI.TryGetSingleton<DragSource>(out var draggedItem);
            var hasDescription = SystemAPI.TryGetSingleton<DescriptionSelection>(out var description);
            Entities.ForEach((Entity entity, ItemView view) =>
            {
                var itemElement = Items.GetItemElementOfView(EntityManager, view);

                var isDraggingSource = hasDragEntity && draggedItem.sourceItemView == entity;
                var isDraggingDestination = hasTether && entity == tether.destinationItemView;
                var isSwapping = hasDragEntity && isDraggingSource ^ isDraggingDestination;
                var isSameItem = itemElement == description.describedItem;
                var isSameView = entity == description.describedItemView;
                
                var isDescribing = hasDescription && !itemElement.IsEmpty() && isSameItem && isSameView;

                view.animator.SetBool(ItemView.SwappingKey, isSwapping);
                view.animator.SetBool(ItemView.DescribingKey, isDescribing);
            }).WithoutBurst().Run();
        }
    }
}