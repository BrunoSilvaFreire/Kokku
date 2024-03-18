using System;
using Unity.Entities;

namespace Game.Components
{
    [Serializable]
    public struct ItemElement : IBufferElementData
    {
        public bool Equals(ItemElement other)
        {
            return type.Equals(other.type);
        }

        public override bool Equals(object obj)
        {
            return obj is ItemElement other && Equals(other);
        }

        public override int GetHashCode()
        {
            return type.GetHashCode();
        }

        public Hash128 type;
        public static bool operator ==(ItemElement a, ItemElement b) => a.type.Equals(b.type);

        public static bool operator !=(ItemElement a, ItemElement b) => !(a == b);
    }
}