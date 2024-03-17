using Game.Components;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Behaviours
{
    public class ItemView : IComponentData
    {
        public const string ItemEnteringKey = "ItemEnter";
        public const string ItemExitingKey = "ItemExit";
        public const string InvalidItemKey = "InvalidItem";
        public const string DraggingKey = "Dragging";

        public int slotIndex;
        public Entity inventoryEntity;
        public Image thumbnail;
        public Image oldThumbnail;
        public Animator animator;
    }

    public struct HasItemTag : IComponentData
    {
    }

    public struct NeedsItemUpdate : IComponentData
    {
        public Entity entityInventory;
        public int index;
    }

    public class NeedsInventoryInitialization : IComponentData
    {
        public InventoryType type;
    }
}