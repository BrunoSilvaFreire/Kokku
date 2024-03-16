using System;
using Unity.Entities;
using Hash128 = Unity.Entities.Hash128;

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

    [Serializable]
    public struct ItemElement : IBufferElementData
    {
        public Hash128 type;
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