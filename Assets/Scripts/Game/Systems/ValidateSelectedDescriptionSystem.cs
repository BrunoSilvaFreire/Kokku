using Game.Behaviours;
using Game.Components;
using Unity.Collections;
using Unity.Entities;

namespace Game.Systems
{
    public partial class ValidateSelectedDescriptionSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var hasDescription = SystemAPI.TryGetSingletonEntity<DescriptionSelection>(out var descriptionEntity);
            if (!hasDescription)
            {
                return;
            }

            var selection = EntityManager.GetComponentData<DescriptionSelection>(descriptionEntity);
            var selectedItemView = EntityManager.GetComponentObject<ItemView>(selection.describedItemView);
            var selectedItem = Items.GetItemElementOfView(EntityManager, selectedItemView);
            if (!selectedItem.IsEmpty() && selectedItem == selection.describedItem)
            {
                return;
            }

            var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            Entities
                .WithNone<NeedsItemDescriptionInitialization>()
                .WithAll<ItemDescriptionView>()
                .ForEach(
                    (Entity entity) =>
                    {
                        commandBuffer.AddComponent(entity, new NeedsItemDescriptionClearTag());
                    }).WithStructuralChanges().WithoutBurst().Run();

            commandBuffer.DestroyEntity(descriptionEntity);
            commandBuffer.Playback(EntityManager);
            commandBuffer.Dispose();
        }
    }
}