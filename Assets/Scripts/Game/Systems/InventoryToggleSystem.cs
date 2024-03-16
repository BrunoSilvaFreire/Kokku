using Game.Behaviours;
using Game.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Systems
{
    public partial class InventoryToggleSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, ref InventoryToggle toggle) =>
            {
                if (Input.GetKeyDown(toggle.keyCode))
                {
                    toggle.isOpen = !toggle.isOpen;
                    EntityManager.AddComponent<NeedsInventoryToggleRefreshTag>(entity);
                }
            }).WithoutBurst().WithStructuralChanges().Run();

            Entities.WithAll<NeedsInventoryToggleRefreshTag>().ForEach((in InventoryToggle toggle) =>
            {
                var animator = InventoryView.FindViewOfType(toggle.type).Animator;
                if (animator == null)
                {
                    Debug.LogWarning($"Inventory {toggle.type} was toggled, but it's animator isn't set.");
                    return;
                }

                animator.SetBool(InventoryView.IsOpenKey, toggle.isOpen);
            }).WithoutBurst().Run();
        }
    }
}