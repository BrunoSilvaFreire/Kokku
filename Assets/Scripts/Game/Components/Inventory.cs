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
    }

    /// <summary>
    /// Points to entities that contain <see cref="ItemElement"/> buffers 
    /// </summary>
    public struct TransferItemEventComponent : IComponentData
    {
        public Entity fromInventory;
        public int fromIndex;
        public Entity toInventory;
        public int toIndex;
    }
}