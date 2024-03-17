using Unity.Entities;
using UnityEngine;

namespace Game.Components
{
    public class InventoryToggleAuthoring : MonoBehaviour
    {
        public InventoryType Type;
        public KeyCode KeyCode;
        public bool initiallyOpen;

        public class InventoryToggleBaker : Baker<InventoryToggleAuthoring>
        {
            public override void Bake(InventoryToggleAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new InventoryToggle
                {
                    type = authoring.Type, 
                    keyCode = authoring.KeyCode,
                    isOpen = authoring.initiallyOpen
                });
                AddComponent<NeedsInventoryToggleRefreshTag>(entity);
            }
        }
    }
}