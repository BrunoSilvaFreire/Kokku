using TMPro;
using Unity.Entities;

namespace Game.Components
{
    public struct NeedsItemDescriptionUpdate : IComponentData
    {
        public Entity viewEntity;
        public Entity inventoryEntity;
        public int index;
    }

    public struct NeedsItemDescriptionClearTag : IComponentData
    {
    }

    public struct NeedsItemDescriptionInitialization : IComponentData
    {
        public InventoryType type;
    }

    public struct DescriptionSelection : IComponentData
    {
        public Entity describedItemView;
        public ItemElement describedItem;
    }

    public class ItemDescriptionView : IComponentData
    {
        public TMP_Text label;
        public TMP_Text description;
    }
}