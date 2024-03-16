using System.Collections.Generic;
using System.Linq;
using Game.Components;
using TMPro;
using UnityEngine;

namespace Game.Behaviours
{
    public class InventoryView : MonoBehaviour
    {
        public const string IsOpenKey = "IsOpen";
        private static readonly List<InventoryView> _views = new List<InventoryView>();
        [SerializeField]
        private InventoryType _type;
        [SerializeField] private Animator _animator;
        [SerializeField] private TMP_Text _itemLabel; 
        [SerializeField] private TMP_Text _itemDescription;

        public Animator Animator => _animator;

        public TMP_Text ItemLabel => _itemLabel;

        public TMP_Text ItemDescription => _itemDescription;

        public static InventoryView FindViewOfType(InventoryType inventoryType)
        {
            return _views.FirstOrDefault(view => view._type == inventoryType);
        }

        private void OnEnable()
        {
            _views.Add(this);
        }

        private void OnDisable()
        {
            _views.Remove(this);
        }
    }
}