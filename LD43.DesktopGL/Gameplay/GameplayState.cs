using LD43.Gameplay.Models;

namespace LD43.Gameplay
{
    public class GameplayState
    {
        public float SacrificeRequiredInMilleseconds { get; private set; }

        public Room Room { get; set; }

        public void DecrementSacrficeTimer(float delta)
        {
            if (SacrificeRequiredInMilleseconds <= 0f) return;
            SacrificeRequiredInMilleseconds -= delta;
            if (SacrificeRequiredInMilleseconds <= 0f) SacrificeRequiredInMilleseconds = 0f;
        }

        public static GameplayState Create()
        {
            var newGame = new GameplayState();

            newGame.SacrificeRequiredInMilleseconds = 3 * 60 * 1000;

            return newGame;
        }
    }
}
