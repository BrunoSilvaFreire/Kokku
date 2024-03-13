using Unity.Collections;
using Unity.Entities;

namespace Systems
{
    public partial class ProcessInventoryChangesSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            var lookup = GetComponentLookup<InventoryComponent>();
            Entities.ForEach((Entity entity, in TransferItemEventComponent transfer) =>
            {
                if (!lookup.TryGetComponent(transfer.from, out var from))
                {
                    return;
                }

                if (!lookup.TryGetComponent(transfer.from, out var to))
                {
                    return;
                }

                var source = from.items[transfer.fromIndex];
                source.count -= transfer.count;
                from.items[transfer.fromIndex] = source;


                var destination = to.items[transfer.toIndex];
                destination.count += transfer.count;
                to.items[transfer.fromIndex] = destination;

                commandBuffer.DestroyEntity(entity);
            }).Schedule();
        }
    }
}