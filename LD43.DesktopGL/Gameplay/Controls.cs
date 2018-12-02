using System.Collections.Generic;
using LD43.Engine;
using Microsoft.Xna.Framework.Input;

namespace LD43.Gameplay
{
    public static class Controls
    {
        public const string MoveLeft = "Move Left";
        public const string Drop = "Drop";
        public const string MoveRight = "Move Right";
        public const string Jump = "Jump";
        public const string SwingWeapon = "Swing Weapon";
        public const string ResetPosition = "Reset Position";
        public const string Continue = "Continue";

        public static Dictionary<string, Control> Create() =>
            new Dictionary<string, Control>
            {
                { MoveLeft, new Button(Keys.A) },
                { Drop, new Button(Keys.S) },
                { MoveRight, new Button(Keys.D) },
                { Jump, new Button(Keys.Space) },
                { SwingWeapon, new Button(Keys.Enter) },
                { ResetPosition, new Button(Keys.F5) },
                { Continue, new Button(Keys.Enter) },
            };
    }
}
