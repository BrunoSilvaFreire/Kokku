using System.Collections.Generic;
using System.Linq;
using Game.Components;
using TMPro;
using UnityEngine;

namespace Game.Behaviours
{
    public class InventoryView : MonoBehaviour
    {
        private static readonly List<InventoryView> _views = new List<InventoryView>();
        [SerializeField]
        private InventoryType _type;

        [SerializeField] private TMP_Text _itemLabel; 
        [SerializeField] private TMP_Text _itemDescription;

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