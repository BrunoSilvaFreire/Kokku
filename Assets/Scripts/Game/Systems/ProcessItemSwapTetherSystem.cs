using Game.Components;
using Unity.Entities;

namespace Game.Systems
{
    public struct DragTether : IComponentData
    {
        public Entity destinationItemView;
    }

    public partial class ProcessItemSwapTetherSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var hasSingleton = SystemAPI.TryGetSingletonEntity<DragTether>(out var singletonEntity);
            Entities.ForEach((Entity entity, in InventorySlotHoveredEvent hoveredEvent) =>
            {
                var tether = new DragTether
                {
                    destinationItemView = hoveredEvent.itemView
                };
                if (hasSingleton)
                {
                    EntityManager.SetComponentData(singletonEntity, tether);
                }
                else
                {
                    EntityManager.CreateSingleton(tether);
                    hasSingleton = true;
                }

                EntityManager.DestroyEntity(entity);
            }).WithoutBurst().WithStructuralChanges().Run();

            if (hasSingleton)
            {
                Entities.ForEach((Entity entity, in InventorySlotUnhoveredEvent hoveredEvent) =>
                {
                    EntityManager.DestroyEntity(singletonEntity);
                    EntityManager.DestroyEntity(entity);
                }).WithoutBurst().WithStructuralChanges().Run();
            }
        }
    }
}