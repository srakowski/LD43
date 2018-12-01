using LD43.Engine;
using Microsoft.Xna.Framework;
using System.Linq;

namespace LD43.Gameplay.Behaviors
{
    public class PlayerController : Behavior
    {        
        private const float _gravity = 0.009f;
        private readonly GameplayState _gs;
        private float _verticalSpeed = 0f;

        public PlayerController(GameplayState gameplayState)
        {
            _gs = gameplayState;
        }

        public override void Update()
        {
            var playerBounds = new Rectangle(
                (Entity.Transform.Position - new Vector2(64, 96)).ToPoint(),
                new Point(128, 192)
            );

            var platform = _gs.Platforms.FirstOrDefault(p => p.Bounds.Intersects(playerBounds));

            if (platform == null)
            {
                _verticalSpeed += (_gravity * Delta);
            }

            if (_verticalSpeed != 0f)
            {
                Entity.Transform.Position += (new Vector2(0, _verticalSpeed) * Delta);
                platform = _gs.Platforms.FirstOrDefault(p => p.Bounds.Intersects(playerBounds));
                if (platform != null && _verticalSpeed > 0f)
                {
                    _verticalSpeed = 0f;
                    Entity.Transform.Position = new Vector2(
                        Entity.Transform.Position.X,
                        platform.Bounds.Y - 95
                    );
                }
            }

            if (platform != null && Input.GetControl<Button>(Controls.Jump).IsDown())
            {
                _verticalSpeed = -3f;
            }

            if (Input.GetControl<Button>(Controls.MoveLeft).IsDown())
            {
                Entity.Transform.Position += (new Vector2(-1, 0) * Delta);
            }

            if (Input.GetControl<Button>(Controls.MoveRight).IsDown())
            {
                Entity.Transform.Position += (new Vector2(1, 0) * Delta);
            }
        }
    }
}
