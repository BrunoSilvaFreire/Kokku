using UnityEngine;
using UnityEngine.UI;

namespace Game.Behaviours
{
    [CreateAssetMenu]
    public class UIAssetsConfiguration : ScriptableSingleton<UIAssetsConfiguration>
    {
        [SerializeField]  private Image _draggingImagePrefab;

        public Image DraggingImagePrefab => _draggingImagePrefab;
    }
}