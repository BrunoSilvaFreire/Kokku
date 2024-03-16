using Game.Behaviours;
using Game.Components;
using Unity.Collections;
using Unity.Entities;

namespace Game.Systems
{
    public partial class InventoryDescriptionInitializationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var buffer = new EntityCommandBuffer(Allocator.Temp);
            Entities.ForEach((
                Entity entity,
                ItemDescriptionView view,
                in NeedsItemDescriptionInitialization initialization
            ) =>
            {
                var inventoryView = InventoryView.FindViewOfType(initialization.type);
                view.label = inventoryView.ItemLabel;
                view.description = inventoryView.ItemDescription;
                buffer.RemoveComponent<NeedsItemDescriptionInitialization>(entity);
            }).WithoutBurst().Run();
            buffer.Playback(EntityManager);
            buffer.Dispose();
        }
    }
}