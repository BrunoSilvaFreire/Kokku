using Game.Components;
using Unity.Collections;
using Unity.Entities;

namespace Game.Systems
{
    public partial class ClearSelectedDescriptionSystem : SystemBase
    {
        protected override void OnUpdate()
        {

            var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            Entities
                .WithNone<NeedsItemDescriptionInitialization>()
                .ForEach((Entity entity, ItemDescriptionView descriptionView, in NeedsItemDescriptionClearTag _) =>
                {
                    descriptionView.label.text = string.Empty;
                    descriptionView.description.text = string.Empty;
                    commandBuffer.RemoveComponent<NeedsItemDescriptionClearTag>(entity);
                }).WithoutBurst().Run();
            commandBuffer.Playback(EntityManager);
            commandBuffer.Dispose();
        }
    }
}