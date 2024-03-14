using System;
using Unity.Entities;
using Hash128 = Unity.Entities.Hash128;

namespace Game.Components
{
    public struct InventoryComponent : IComponentData
    {
        public int inventorySize;
    }

    [Serializable]
    public struct ItemElement : IBufferElementData
    {
        public Hash128 type;
        public byte count;
    }

    /// <summary>
    /// Points to entities that contain <see cref="InventoryContentComponent"/> 
    /// </summary>
    public struct TransferItemEventComponent : IComponentData
    {
        public Entity from;
        public int fromIndex;
        public Entity to;
        public int toIndex;
        public byte count;
    }
}