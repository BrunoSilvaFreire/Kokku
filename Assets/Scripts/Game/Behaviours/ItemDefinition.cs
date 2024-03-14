using UnityEngine;

namespace Game.Behaviours
{
    [CreateAssetMenu]
    public class ItemDefinition : ScriptableObject
    {
        [SerializeField] private Sprite _thumbnail;

        public Sprite Thumbnail => _thumbnail;
    }
}