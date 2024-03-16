using System;
using Game.Behaviours;
using Game.Components;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

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
                var element = Items.GetItemElementAt(lookup, update.entityInventory, update.index);
                var hasItemNow = !element.IsEmpty();
                var hadItemLastFrame = EntityManager.HasComponent<HasItemTag>(entity);
                var isSwappingItems = hadItemLastFrame && hasItemNow;

                if (hasItemNow)
                {
                    var definition = element.FindDefinition();
                    ReplaceSprite(view, definition.Thumbnail);
                }
                else
                {
                    ReplaceSprite(view, null);
                }

                if (!hasItemNow || isSwappingItems)
                {
                    view.animator.SetTrigger(ItemView.ItemExitingKey);
                }

                if (hasItemNow)
                {
                    view.animator.SetTrigger(ItemView.ItemEnteringKey);
                }

                if (!hasItemNow)
                {
                    buffer.RemoveComponent<HasItemTag>(entity);
                }

                view.animator.SetBool(ItemView.DraggingKey, false);
                view.animator.Update(0);
                if (!hadItemLastFrame && hasItemNow)
                {
                    buffer.AddComponent<HasItemTag>(entity);
                }

                buffer.RemoveComponent<NeedsItemUpdate>(entity);
            }).WithoutBurst().Run();

            buffer.Playback(EntityManager);
            buffer.Dispose();
        }

        private static void ReplaceSprite(ItemView view, Sprite newSprite)
        {
            Sprite oldSprite = view.thumbnail.sprite;
            view.thumbnail.sprite = newSprite;
            view.oldThumbnail.sprite = oldSprite;
        }
    }
}