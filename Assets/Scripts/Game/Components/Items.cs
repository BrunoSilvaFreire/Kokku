using System;
using Game.Behaviours;
using Unity.Entities;

namespace Game.Components
{
    public static class Items
    {

        public static Hash128 NullHash = new Hash128(0, 0, 0, 0);

        public static bool IsEmpty(this ItemElement element)
        {
            return element.type == NullHash;
        }

        public static ItemElement GetItemElementOfView( EntityManager entityManager, Entity viewEntity)
        {
            var view = entityManager.GetComponentData<ItemView>(viewEntity);
            return GetItemElementOfView(entityManager, view);
        }

        public static ItemElement GetItemElementOfView( EntityManager entityManager, ItemView view)
        {
            return GetItemElementAt(entityManager, view.inventoryEntity, view.slotIndex);
        }

        public static ItemElement GetItemElementAt( EntityManager entityManager, Entity inventory, int index)
        {
            var buffer = entityManager.GetBuffer<ItemElement>(inventory);
            return buffer[index];
        }
        public static ItemElement GetItemElementAt( BufferLookup<ItemElement> entityManager, Entity inventory, int index)
        {
            var buffer = entityManager[inventory];
            return buffer[index];
        }

        public static ItemDefinition FindDefinition(this ItemElement element)
        {
            if (!ItemRegistry.Instance.TryGet(element.type, out var definition))
            {
                throw new ArgumentException(
                    $"Item element {element} doesn't have a matching item definition with id {element.type}"
                );
            }

            return definition;
        }
    }
}