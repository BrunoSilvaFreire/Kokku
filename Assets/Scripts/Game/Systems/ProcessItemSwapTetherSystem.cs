using Game.Components;
using Unity.Entities;
using UnityEngine;

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
            Entities.ForEach((Entity eventEntity, in InventorySlotHoveredEvent hoveredEvent) =>
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
                    singletonEntity = EntityManager.CreateSingleton(tether);
                    hasSingleton = true;
                }

                EntityManager.DestroyEntity(eventEntity);
            }).WithoutBurst().WithStructuralChanges().Run();
            
            // Here we get the singleton entity again because it may have changed in the previous loop
            hasSingleton = SystemAPI.TryGetSingletonEntity<DragTether>(out singletonEntity);
            if (hasSingleton)
            {
                var tether = SystemAPI.GetSingleton<DragTether>();
                Entities.ForEach((Entity entity, in InventorySlotUnhoveredEvent unhoveredEvent) =>
                {
                    if (unhoveredEvent.itemView == tether.destinationItemView)
                    {
                        EntityManager.DestroyEntity(singletonEntity);
                    }

                    EntityManager.DestroyEntity(entity);
                }).WithoutBurst().WithStructuralChanges().Run();
            }
        }
    }
}