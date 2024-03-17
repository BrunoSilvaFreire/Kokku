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
        public const string SwappingKey = "Swapping";
        public const string DescribingKey = "Describing";

        public int slotIndex;
        public Entity inventoryEntity;
        public Image thumbnail;
        public Image oldThumbnail;
        public Animator animator;
        public AudioSource audioSource;
    }

    public struct HasItemTag : IComponentData
    {
    }

    public struct NeedsItemUpdate : IComponentData
    {
        public Entity entityInventory;
        public int index;
        public bool playSFX;
    }

    public class NeedsInventoryInitialization : IComponentData
    {
        public InventoryType type;
    }
}