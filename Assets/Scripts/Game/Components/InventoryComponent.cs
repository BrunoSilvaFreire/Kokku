using Unity.Entities;

namespace Game.Components
{
    public enum InventoryType
    {
        Main,
        Hotbar
    }
    public struct InventoryComponent : IComponentData
    {
        public int inventorySize;
        public InventoryType type;
    }

    /// <summary>
    /// Points to entities that contain <see cref="ItemElement"/> buffers 
    /// </summary>
    public struct TransferItemEventComponent : IComponentData
    {
        public TransferReference from;
        public TransferReference to;
    }

    public struct TransferReference
    {
        public Entity inventory;
        public Entity itemView;
        public int index;
    }
}