using LD43.Gameplay.Models;
using System.Collections.Generic;

namespace LD43.Gameplay
{
    public class GameplayState
    {
        public Room Room { get; set; }

        public static GameplayState Create() => new GameplayState();
    }
}
