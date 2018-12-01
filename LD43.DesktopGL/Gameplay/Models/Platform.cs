using Microsoft.Xna.Framework;

namespace LD43.Gameplay.Models
{
    public class Platform
    {
        public Platform(int x, int y, int width, int height)
        {
            Bounds = new Rectangle(x, y, width, height);
        }

        public Rectangle Bounds { get; }
    }
}
