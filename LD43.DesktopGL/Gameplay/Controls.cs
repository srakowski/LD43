using System.Collections.Generic;
using LD43.Engine;
using Microsoft.Xna.Framework.Input;

namespace LD43.Gameplay
{
    public static class Controls
    {
        public const string MoveLeft = "Move Left";
        public const string MoveRight = "Move Right";

        public static Dictionary<string, Control> Create() =>
            new Dictionary<string, Control>
            {
                { MoveLeft, new Button(Keys.A) },
                { MoveRight, new Button(Keys.D) },
            };
    }
}
