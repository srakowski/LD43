namespace LD43.Gameplay
{
    using LD43.Engine;
    using System.Collections.Generic;
    using System.Linq;

    public static class Layers
    {
        public static IEnumerable<Layer> Create()
        {
            return new[]
            {
                new Layer("Inanimates", 25),

                new Layer("Enemies", 35),

                new Layer("Player", 50),

                new Layer("Projectiles", 90),

                new Layer("Hud", 100, stickToCamera: true),
            };
        }
    }
}
