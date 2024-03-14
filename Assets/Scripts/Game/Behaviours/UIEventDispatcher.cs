using System;
using Game.Components;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Behaviours
{
    public class UIEventDispatcher : MonoBehaviour
    {
        [SerializeField]
        private Button _button;
        private int _index;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClicked);
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
            var entity = entityManager.CreateEntity();
            var component = new InventorySlotClickedEvent
            {
                slotIndex = _index
            };
            entityManager.AddComponentData(entity, component);
        }
    }
}