using Game.Behaviours;
using Unity.Entities;

namespace Game.Components
{
    public static class ItemExtensions
    {
        
        public const string ItemEnteringKey = "ItemEnter";
        public const string ItemExitingKey = "ItemExit";

        public static Hash128 NullHash = new Hash128(0, 0, 0, 0);
        
        public static bool IsEmpty(this ItemElement element)
        {
            return element.type == NullHash;
        }

        public static void Refresh(this ItemView view, ItemElement element)
        {
            
        }
    }
}