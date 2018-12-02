using LD43.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD43.Gameplay.Behaviors
{
    public class CameraController : Behavior
    {
        private Entity _player;
        private Rectangle _roomBounds;
        private GraphicsDevice _graphicsDevice;

        public CameraController(Entity player, Rectangle roomBounds)
        {
            _player = player;
            _roomBounds = roomBounds;
        }

        public override void Initialize()
        {
            _graphicsDevice = Services.GetService<GraphicsDevice>();
        }

        public override void Update()
        {
            var distanceToCenterScreen = _graphicsDevice.Viewport.Bounds.Center;
            Entity.Transform.Position = new Vector2(
                MathHelper.Clamp(_player.Transform.Position.X, distanceToCenterScreen.X, _roomBounds.Width - distanceToCenterScreen.X),
                MathHelper.Clamp(_player.Transform.Position.Y, distanceToCenterScreen.Y, _roomBounds.Height - distanceToCenterScreen.Y)
            );
        }
    }
}
