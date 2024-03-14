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
                         .WithEntityAccess())
            {
                var transfer = transferRef.ValueRO;
                if (!lookup.TryGetBuffer(transfer.fromInventory, out var from))
                {
                    return;
                }

                if (!lookup.TryGetBuffer(transfer.fromInventory, out var to))
                {
                    return;
                }

                var source = from[transfer.fromIndex];
                var destination = to[transfer.toIndex];

                bool isSwap = !source.IsEmpty() && !destination.IsEmpty();
                if (isSwap)
                {
                    
                }
                else
                {
                    
                }
                
                commandBuffer.DestroyEntity(entity);
            }
            commandBuffer.Playback(EntityManager);
            commandBuffer.Dispose();
        }
    }
}