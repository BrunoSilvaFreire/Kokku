using UnityEngine;
using UnityEngine.UI;

namespace Game.Behaviours
{
    /// <summary>
    /// UIConfiguration is a singleton scriptable object that holds references to the main assets used by the UI.
    /// Most notably the dragging image prefab, and the sound effects used by the UI.
    /// </summary>
    [CreateAssetMenu]
    public class UIConfiguration : ScriptableSingleton<UIConfiguration>
    {
        [SerializeField] private Image _draggingImagePrefab;
        [SerializeField] private AudioClip _itemEnterSound;
        [SerializeField] private AudioClip _itemExitSound;
        [SerializeField] private AudioClip _itemSwapSound;
        [SerializeField] private AudioClip _invalidClickSound;
        [SerializeField] private AudioClip _itemClickSound;
        [SerializeField] private float _itemSFXPitchRangeMin = 0.9F;
        [SerializeField] private float _itemSFXPitchRangeMax = 1.1F;

        public Image DraggingImagePrefab => _draggingImagePrefab;
        public AudioClip ItemEnterSound => _itemEnterSound;
        public AudioClip ItemExitSound => _itemExitSound;
        public AudioClip ItemSwapSound => _itemSwapSound;
        public AudioClip InvalidClickSound => _invalidClickSound;
        public AudioClip ItemClickSound => _itemClickSound;
        public float ItemSfxPitchRangeMin => _itemSFXPitchRangeMin;
        public float ItemSfxPitchRangeMax => _itemSFXPitchRangeMax;
    }
}