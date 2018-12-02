using Microsoft.Xna.Framework;
using System;

namespace LD43.Gameplay.Models
{
    public class Player
    {
        public Vector2 Position { get; set; }

        public Rectangle Bounds { get; set; }

        public FacingDirection FacingDirection { get; set; } = FacingDirection.Right;

        public int GoldCollected { get; set; }

        public int SoulsCollected { get; set; }

        public int HP { get; set; } = 300;

        public int MaxHP { get; set; } = 300;

        public bool IsInvulnerable { get; set; } = false;

        public bool GotHit { get; set; } = false;

        public void Pickup(int goldToAdd, int soulsToAdd)
        {
            GoldCollected += goldToAdd;
            SoulsCollected += soulsToAdd;
        }

        internal void Hit(int dmg)
        {
            if (IsInvulnerable) return;
            HP -= dmg;
            if (HP < 0) HP = 0;
            GotHit = true;
        }

        internal void Hit(object damage)
        {
            throw new NotImplementedException();
        }
    }
}
