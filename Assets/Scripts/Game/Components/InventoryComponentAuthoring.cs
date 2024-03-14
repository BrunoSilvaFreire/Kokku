using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Components
{
    public class InventoryComponentAuthoring : MonoBehaviour
    {
        [SerializeField]
        private int _inventorySize;

        [SerializeField] private ItemElement[] _preExistingItems;
        public class InventoryComponentBaker : Baker<InventoryComponentAuthoring>
        {
            public override void Bake(InventoryComponentAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new InventoryComponent { inventorySize = authoring._inventorySize });
                var buf = AddBuffer<ItemElement>(entity);
                buf.Length = authoring._inventorySize;
                for (var i = 0; i < authoring._preExistingItems.Length; i++)
                {
                    var item = authoring._preExistingItems[i];
                    buf[i] = item;
                }
            }
        }
    }
}