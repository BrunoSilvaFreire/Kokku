using Game.Behaviours;
using Unity.Entities;

namespace Game.Components
{
    public static class ItemExtensions
    {

        public static Hash128 NullHash = new Hash128(0, 0, 0, 0);

        public static bool IsEmpty(this ItemElement element)
        {
            return element.type == NullHash;
        }

        public static ItemElement GetItemElementOfView(this EntityManager entityManager, Entity viewEntity)
        {
            var view = entityManager.GetComponentData<ItemView>(viewEntity);
            return entityManager.GetItemElementOfView(view);
        }

        public static ItemElement GetItemElementOfView(this EntityManager entityManager, ItemView view)
        {
            return entityManager.GetItemElementAt(view.inventoryEntity, view.slotIndex);
        }

        public static ItemElement GetItemElementAt(this EntityManager entityManager, Entity inventory, int index)
        {
            var buffer = entityManager.GetBuffer<ItemElement>(inventory);
            return buffer[index];
        }
        public static ItemElement GetItemElementAt(this BufferLookup<ItemElement> entityManager, Entity inventory, int index)
        {
            var buffer = entityManager[inventory];
            return buffer[index];
        }
    }
}