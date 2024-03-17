using System;
using Game.Components;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Behaviours
{
    public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
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
            DispatchEvent(
                new InventorySlotClickedEvent
                {
                    itemView = _assignedEntity,
                    slotIndex = transform.GetSiblingIndex()
                }
            );
        }

        void DispatchEvent<T>(T eventComponent) where T : unmanaged, IComponentData
        {
            if (_assignedEntity == Entity.Null)
            {
                throw new Exception(
                    "Inventory slot has no entity assigned to it. Normally this means that there aren't enough ItemViewAuthoring components in the inventory, or that the inventory is not initialized properly.");
            }

            var targetWorld = GetTargetWorld<T>();
            var entityManager = targetWorld.EntityManager;
            var entity = entityManager.CreateEntity();
            entityManager.AddComponentData(entity, eventComponent);
        }

        private static World GetTargetWorld<T>() where T : unmanaged, IComponentData
        {
            foreach (var world in World.All)
            {
                return world;
            }

            throw new Exception("Unable to find a world to dispatch event to.");
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            DispatchEvent(new InventorySlotDragBeginEvent
            {
                itemView = _assignedEntity,
                slotIndex = transform.GetSiblingIndex()
            });
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            DispatchEvent(new InventorySlotDragEndEvent
                {
                    itemView = _assignedEntity,
                    slotIndex = transform.GetSiblingIndex()
                }
            );
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            DispatchEvent(new InventorySlotHoveredEvent
                {
                    itemView = _assignedEntity,
                    slotIndex = transform.GetSiblingIndex()
                }
            );
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DispatchEvent(new InventorySlotUnhoveredEvent
                {
                    itemView = _assignedEntity,
                    slotIndex = transform.GetSiblingIndex()
                }
            );
        }

        public void OnDrag(PointerEventData eventData) { }
    }
}