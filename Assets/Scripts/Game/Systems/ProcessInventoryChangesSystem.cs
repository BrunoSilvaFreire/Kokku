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
            foreach (var (transferRef, entity) in SystemAPI.Query<RefRO<TransferItemEventComponent>>()
                         .WithNone<NeedsInventoryInitialization>()
                         .WithEntityAccess())
            {
                var transfer = transferRef.ValueRO;
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
                    entityInventory = transfer.from.inventory
                });
                commandBuffer.AddComponent(transfer.to.itemView, new NeedsItemUpdate
                {
                    index = transfer.to.index,
                    entityInventory = transfer.to.inventory
                });
                commandBuffer.DestroyEntity(entity);
            }
            commandBuffer.Playback(EntityManager);
            commandBuffer.Dispose();
        }
    }
}