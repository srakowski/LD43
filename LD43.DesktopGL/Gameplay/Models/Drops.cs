using Microsoft.Xna.Framework;
using System;

namespace LD43.Gameplay.Models
{
    public abstract class Drop
    {
        public Drop(Vector2 pos)
        {
            Position = pos;
        }

        public Vector2 Position { get; }
        public abstract int Value { get; }
        public bool IsPickedUp { get; set; } = false;

        public T Match<T>(
            Func<GoldDrop, T> goldDrop,
            Func<SoulDrop, T> soulDrop
        )
        {
            return this is GoldDrop gd ? goldDrop(gd) :
                this is SoulDrop sd ? soulDrop(sd) :
                default(T);
        }
    }

    public class GoldDrop : Drop
    {
        public GoldDrop(Vector2 pos, int value) : base(pos) => Value = value;
        public override int Value { get; }
    }

    public class SoulDrop : Drop
    {
        public SoulDrop(Vector2 pos) : base(pos) { }
        public override int Value => 1;
    }
}
