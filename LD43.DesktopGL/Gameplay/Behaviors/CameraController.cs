using LD43.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace LD43.Gameplay.Behaviors
{
    public class CameraController : Behavior
    {
        private Entity _player;
        private GameplayState _gs;
        private GraphicsDevice _graphicsDevice;

        public CameraController(Entity player, GameplayState gs)
        {
            _player = player;
            _gs = gs;
        }

        public override void Initialize()
        {
            _graphicsDevice = Services.GetService<GraphicsDevice>();
            Services.GetService<RenderingManager>().Layers.ToList().ForEach(l => l.Show = true);
            Update();
        }

        public override void Update()
        {
            var distanceToCenterScreen = _graphicsDevice.Viewport.Bounds.Center;
            var b = _gs.CurrentRoom.Bounds; 
            Entity.Transform.Position = new Vector2(
                MathHelper.Clamp(_player.Transform.Position.X, b.Left + distanceToCenterScreen.X, b.Right - distanceToCenterScreen.X),
                MathHelper.Clamp(_player.Transform.Position.Y, b.Top + distanceToCenterScreen.Y, b.Bottom - distanceToCenterScreen.Y)
            );
        }
    }
}
