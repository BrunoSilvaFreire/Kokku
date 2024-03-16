using Unity.Entities;
using UnityEngine;

namespace Game.Components
{
    public struct NeedsInventoryToggleRefreshTag : IComponentData
    {
    }

    public struct InventoryToggle : IComponentData
    {
        public InventoryType type;
        public KeyCode keyCode;
        public bool isOpen;
    }
}