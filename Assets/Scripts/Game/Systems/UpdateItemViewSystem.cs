using System;
using Game.Behaviours;
using Game.Components;
using Unity.Collections;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class UpdateItemViewSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var lookup = GetBufferLookup<ItemElement>();
            var buffer = new EntityCommandBuffer(Allocator.Temp);
            Entities.ForEach((Entity entity, ItemView view, in NeedsItemUpdate update) =>
            {
                var buf = lookup[update.entityInventory];
                var element = buf.ElementAt(update.index);
                if (!ItemRegistry.Instance.TryGet(element.type, out var definition))
                {
                    throw new ArgumentException(
                        $"Item element {element} doesn't have a matching item definition with id {element.type}"
                    );
                }

                var hadItemLastFrame = EntityManager.HasComponent<HasItemTag>(entity);
                var hasItemNow = !element.IsEmpty();
                var isSwappingItems = hadItemLastFrame && hasItemNow;
                view.thumbnail.sprite = definition.Thumbnail;
                if (!hasItemNow || isSwappingItems)
                {
                    view.animator.SetTrigger(ItemExtensions.ItemExitingKey);
                    buffer.RemoveComponent<HasItemTag>(entity);
                }

                if (hasItemNow)
                {
                    view.animator.SetTrigger(ItemExtensions.ItemEnteringKey);
                }

                if (!hadItemLastFrame && hasItemNow)
                {
                    buffer.AddComponent<HasItemTag>(entity);
                }

                buffer.RemoveComponent<NeedsItemUpdate>(entity);
            }).WithoutBurst().Run();

            buffer.Playback(EntityManager);
            buffer.Dispose();
        }
    }
}