using System;
using Game.Components;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Behaviours
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] private Image _thumbnail;
        [SerializeField] private Image _oldThumbnail;
        [SerializeField] private Animator _animator;
        [SerializeField] private Button _button;

        private Entity _assignedEntity;

        public Image Thumbnail => _thumbnail;
        public Image OldThumbnail => _oldThumbnail;
        public Animator Animator => _animator;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        public void SetEntity(Entity entity)
        {
            if (_assignedEntity != Entity.Null)
            {
                throw new ArgumentException(
                    $"Inventory slot {this} has had it's entity set more than once (Was {_assignedEntity} previously, attemped to set {entity})."
                );
            }

            _assignedEntity = entity;
        }

        void OnClicked()
        {
            foreach (var world in World.All)
            {
                bool isTargetWorld = true;
                if (isTargetWorld)
                {
                    DispatchClickedEvent(world.EntityManager);
                    break;
                }
            }
        }

        void DispatchClickedEvent(EntityManager entityManager)
        {
            if (_assignedEntity == Entity.Null)
            {
                throw new Exception("Inventory slot has no entity assigned to it. Normally this means that there aren't enough ItemViewAuthoring components in the inventory, or that the inventory is not initialized properly.");
            }
            var entity = entityManager.CreateEntity();
            var component = new InventorySlotClickedEvent
            {
                itemView = _assignedEntity,
                slotIndex = transform.GetSiblingIndex()
            };
            entityManager.AddComponentData(entity, component);
        }
    }
}