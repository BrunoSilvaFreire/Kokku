using Game.Components;
using Unity.Collections;
using Unity.Entities;

namespace Game.Systems
{
    public partial class UpdateItemDescriptionSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            var lookup = SystemAPI.GetBufferLookup<ItemElement>();
            var hasDescription = SystemAPI.TryGetSingletonEntity<DescriptionSelection>(out var descriptionEntity);
            Entities
                .WithNone<NeedsItemDescriptionInitialization>()
                .ForEach((Entity entity, ItemDescriptionView descriptionView, in NeedsItemDescriptionUpdate update) =>
                {
                    var itemElement = Items.GetItemElementAt(lookup, update.inventoryEntity, update.index);
                    if (itemElement.IsEmpty())
                    {
                        commandBuffer.AddComponent<NeedsItemDescriptionClearTag>(entity);
                    }
                    else
                    {
                        var definition = Items.FindDefinition(itemElement);
                        descriptionView.label.text = definition.name;
                        descriptionView.description.text = definition.Description;

                        var selection = new DescriptionSelection
                        {
                            describedItemView = update.viewEntity,
                            describedItem = itemElement
                        };
                        if (hasDescription)
                        {
                            commandBuffer.SetComponent(
                                descriptionEntity,
                                selection
                            );
                        }
                        else
                        {
                            EntityManager.CreateSingleton(selection);
                        }
                    }


                    commandBuffer.RemoveComponent<NeedsItemDescriptionUpdate>(entity);
                }).WithStructuralChanges().WithoutBurst().Run();

            commandBuffer.Playback(EntityManager);
            commandBuffer.Dispose();
        }
    }
}