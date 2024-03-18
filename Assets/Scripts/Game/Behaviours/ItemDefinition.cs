using UnityEngine;

namespace Game.Behaviours
{
    /// <summary>
    /// ItemDefinition is a ScriptableObject that holds references to the thumbnail and description of an item.
    /// <seealso cref="ItemRegistry"/>
    /// </summary>
    [CreateAssetMenu]
    public class ItemDefinition : ScriptableObject
    {
        [SerializeField] private Sprite _thumbnail;
        [SerializeField] private string _description;

        public Sprite Thumbnail => _thumbnail;
        public string Description => _description;
    }
}