using Game.Behaviours;
using Unity.Entities;
using UnityEngine;

namespace Game.Components
{
    public class ItemDescriptionViewAuthoring : MonoBehaviour
    {
        [SerializeField] private InventoryType _type;

        public class ItemDescriptionViewBaker : Baker<ItemDescriptionViewAuthoring>
        {
            public override void Bake(ItemDescriptionViewAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponentObject(entity, new ItemDescriptionView());
                AddComponent(entity, new NeedsItemDescriptionInitialization
                {
                    type = authoring._type
                });
            }
        }
    }
}