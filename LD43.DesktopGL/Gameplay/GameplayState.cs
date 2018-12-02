using LD43.Gameplay.Models;
using System;

namespace LD43.Gameplay
{
    public class GameplayState
    {
        public Random Random { get; private set; }

        public float SacrificeRequiredInMilleseconds { get; private set; }

        public Sacrifice Sacrifice { get; private set; }

        public Room Room { get; set; }
        
        public Player Player { get; set; }

        public void DecrementSacrficeTimer(float delta)
        {
            if (SacrificeRequiredInMilleseconds <= 0f) return;
            SacrificeRequiredInMilleseconds -= delta;
            if (SacrificeRequiredInMilleseconds <= 0f) SacrificeRequiredInMilleseconds = 0f;
        }

        public static GameplayState Create()
        {
            var newGame = new GameplayState();

            newGame.Random = new Random();
            newGame.Sacrifice = Sacrifice.CreateFirst(newGame);
            newGame.SacrificeRequiredInMilleseconds = newGame.Sacrifice.TimeRequiredInMilleseconds;
            newGame.Player = new Player();

            return newGame;
        }
    }
}
