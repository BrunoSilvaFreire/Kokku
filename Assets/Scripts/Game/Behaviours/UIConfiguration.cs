using UnityEngine;
using UnityEngine.UI;

namespace Game.Behaviours
{
    [CreateAssetMenu]
    public class UIConfiguration : ScriptableSingleton<UIConfiguration>
    {
        [SerializeField]  private Image _draggingImagePrefab;
        [SerializeField] private KeyCode _toggleKeyCode = KeyCode.Tab;
        public Image DraggingImagePrefab => _draggingImagePrefab;

        public KeyCode ToggleKeyCode => _toggleKeyCode;
    }
}