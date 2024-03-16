using Unity.Entities;
using UnityEngine.UI;

namespace Game.Components
{
    public class DraggingItem : IComponentData
    {
        public Entity itemView;
        public Image thumbnail;
        public int originalSlotIndex;
    }
}