using Game.Behaviours;
using Game.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Game.Systems
{
    [BurstCompile]
    public partial class ProcessInventoryChangesSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            var lookup = SystemAPI.GetBufferLookup<ItemElement>();
            Entities
                .WithNone<NeedsInventoryInitialization>()
                .ForEach((Entity entity, in TransferItemEventComponent transfer) =>
                {
                    if (!lookup.TryGetBuffer(transfer.from.inventory, out var from))
                    {
                        return;
                    }

                    if (!lookup.TryGetBuffer(transfer.to.inventory, out var to))
                    {
                        return;
                    }

                    var source = from[transfer.from.index];
                    var destination = to[transfer.to.index];

                    to[transfer.to.index] = source;
                    from[transfer.from.index] = destination;

                    commandBuffer.AddComponent(transfer.from.itemView, new NeedsItemUpdate
                    {
                        index = transfer.from.index,
                        entityInventory = transfer.from.inventory,
                        playSFX = false
                    });
                    commandBuffer.AddComponent(transfer.to.itemView, new NeedsItemUpdate
                    {
                        index = transfer.to.index,
                        entityInventory = transfer.to.inventory,
                        playSFX = true
                    });
                    commandBuffer.DestroyEntity(entity);
                }).WithoutBurst().Run();
            commandBuffer.Playback(EntityManager);
            commandBuffer.Dispose();
        }
    }
}