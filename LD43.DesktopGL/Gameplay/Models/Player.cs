using System;

namespace LD43.Gameplay.Models
{
    public class Player
    {
        public FacingDirection FacingDirection { get; set; } = FacingDirection.Right;

        public int GoldCollected { get; set; }

        public int SoulsCollected { get; set; }

        public int HP { get; set; } = 300;

        public int MaxHP { get; set; } = 300;

        public void Pickup(int goldToAdd, int soulsToAdd)
        {
            GoldCollected += goldToAdd;
            SoulsCollected += soulsToAdd;
        }
    }
}
