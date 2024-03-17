using Unity.Entities;
using UnityEngine.UI;

namespace Game.Components
{
    public class DragSource : IComponentData
    {
        public Entity sourceItemView;
        public Image thumbnail;
    }

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