using LD43.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD43.Gameplay.Behaviors
{
    public class CameraController : Behavior
    {
        private Entity _player;
        private int _levelWidthInPixels;
        private int _levelHeightInPixels;
        private GraphicsDevice _graphicsDevice;

        public CameraController(Entity player, int levelWidthInPixels, int levelHeightInPixels)
        {
            _player = player;
            _levelWidthInPixels = levelWidthInPixels;
            _levelHeightInPixels = levelHeightInPixels;
        }

        public override void Initialize()
        {
            _graphicsDevice = Services.GetService<GraphicsDevice>();
        }

        public override void Update()
        {
            var distanceToCenterScreen = _graphicsDevice.Viewport.Bounds.Center;
            Entity.Transform.Position = new Vector2(
                MathHelper.Clamp(_player.Transform.Position.X, distanceToCenterScreen.X, _levelWidthInPixels - distanceToCenterScreen.X),
                MathHelper.Clamp(_player.Transform.Position.Y, distanceToCenterScreen.Y, _levelHeightInPixels - distanceToCenterScreen.Y)
            );
        }
    }
}
