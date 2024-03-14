using Unity.Entities;
using Unity.Mathematics;

namespace Game.Components
{
    public struct UserInput : IComponentData
    {
        public float2 mousePosition;
        public bool clicking;
    }

    public struct InventorySlotClickedEvent : IComponentData
    {
        public Entity slot;
        public int slotIndex;
    }
}