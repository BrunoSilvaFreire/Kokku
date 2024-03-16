using Game.Behaviours;
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
            Entities
                .WithNone<NeedsItemDescriptionInitialization>()
                .ForEach((Entity entity, ItemDescriptionView descriptionView, in NeedsItemDescriptionUpdate update) =>
                {
                    var itemElement = lookup.GetItemElementAt(update.entityInventory, update.index);
                    if (itemElement.IsEmpty())
                    {
                        descriptionView.label.text = string.Empty;
                        descriptionView.description.text = string.Empty;
                    }
                    else
                    {
                        var definition = itemElement.FindDefinition();
                        descriptionView.label.text = definition.name;
                        descriptionView.description.text = definition.Description;
                    }

                    commandBuffer.RemoveComponent<NeedsItemDescriptionUpdate>(entity);
                }).WithoutBurst().Run();
            commandBuffer.Playback(EntityManager);
            commandBuffer.Dispose();
        }
    }
}