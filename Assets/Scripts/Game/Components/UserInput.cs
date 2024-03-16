using Unity.Entities;

namespace Game.Components
{
    public struct InventorySlotClickedEvent : IComponentData
    {
        public Entity itemView;
        public int slotIndex;
    }

    public struct InventorySlotDragBeginEvent : IComponentData
    {
        public Entity itemView;
        public int slotIndex;
    }

    public struct InventorySlotDragEndEvent : IComponentData
    {
        public Entity itemView;
        public int slotIndex;
    }
    public struct InventorySlotHoveredEvent : IComponentData
    {
        public Entity itemView;
        public int slotIndex;
    }
    public struct InventorySlotUnhoveredEvent : IComponentData
    {
        public Entity itemView;
        public int slotIndex;
    }
}