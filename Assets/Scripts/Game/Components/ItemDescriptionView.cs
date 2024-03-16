using TMPro;
using Unity.Entities;

namespace Game.Components
{
    public struct NeedsItemDescriptionUpdate : IComponentData
    {
        public Entity entityInventory;
        public int index;
    }

    public struct NeedsItemDescriptionInitialization : IComponentData
    {
        public InventoryType type;
    }

    public class ItemDescriptionView : IComponentData
    {
        public TMP_Text label;
        public TMP_Text description;
    }
}