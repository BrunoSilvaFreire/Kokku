using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Behaviours
{
    public class ItemView : IComponentData
    {
        public int slotIndex;
        public Entity inventoryEntity;
        public Image thumbnail;
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
    public struct NeedsItemRefreshTag : IComponentData
    {
    }
}