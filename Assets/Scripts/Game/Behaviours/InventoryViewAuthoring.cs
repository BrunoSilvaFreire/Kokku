using Unity.Entities;
using UnityEngine;

namespace Game.Behaviours
{
    public class InventoryViewAuthoring : MonoBehaviour
    {
        public class InventoryViewBaker : Baker<InventoryViewAuthoring>
        {
            public override void Bake(InventoryViewAuthoring authoring)
            {
                DependsOnComponentsInChildren<ItemViewAuthoring>();
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponentObject(entity, new InventoryView());
            }
        }
    }
}