using LD43.Engine;
using Microsoft.Xna.Framework;

namespace LD43.Gameplay.Behaviors
{
    public class ProjectileController : Behavior
    {
        public Vector2 _velocity;
        private GameplayState _gs;
        private SceneManager _screenManager;
        private int _dmg;

        public ProjectileController(Vector2 direction, float speed, GameplayState gs, int dmg)
        {
            _velocity = direction;
            _velocity.Normalize();
            _velocity *= speed;
            _gs = gs;
        }

        public override void Initialize()
        {
            _screenManager = Services.GetService<SceneManager>();
            Entity.Transform.Rotation = _velocity.ToAngle();
        }

        public override void Update()
        {
            if (_gs.Player.Bounds.Contains(Entity.Transform.Position))
            {
                _gs.Player.Hit(_dmg);
            }

            Entity.Transform.Position += (_velocity * Delta);
            if (!_gs.CurrentRoom.Bounds.Contains(Entity.Transform.Position))
            {
                _screenManager.ActiveScene.RemoveEntity(Entity);
            }
        }
    }
}
