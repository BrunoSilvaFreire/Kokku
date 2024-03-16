using UnityEngine;

namespace Game.Behaviours
{
    [CreateAssetMenu]
    public class ItemDefinition : ScriptableObject
    {
        [SerializeField] private Sprite _thumbnail;
        [SerializeField] private string _description;

        public Sprite Thumbnail => _thumbnail;
        public string Description => _description;
    }
}