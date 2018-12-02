using LD43.Engine;
using LD43.Gameplay.Models;
using Microsoft.Xna.Framework;
using System;

namespace LD43.Gameplay.Behaviors
{
    public class EnemyController : Behavior
    {
        private Enemy _enemy;
        private SceneManager _sceneManager;
        private GameplayState _gs;

        public EnemyController(GameplayState gs, Enemy enemy)
        {
            _enemy = enemy;
            _gs = gs;
        }

        public override void Initialize()
        {
            base.Initialize();
            _sceneManager = Services.GetService<SceneManager>();
        }

        public override void Update()
        {
            if (_enemy.IsDead)
            {
                _sceneManager.ActiveScene.RemoveEntity(Entity);
                if (_enemy.GoldValue > 0)
                {
                    var drop = new GoldDrop(Entity.Transform.Position, _enemy.GoldValue);
                    _gs.Room.AddDrop(drop);
                    var ge = new Entity();
                    ge.AddComponent(new SpriteRenderer("GoldDrop"));
                    ge.AddComponent(new DropController(drop));
                    ge.Transform.Position = Entity.Transform.Position;
                    _sceneManager.ActiveScene.AddEntity(ge);                    
                }

                var soulDrop = new SoulDrop(Entity.Transform.Position);
                _gs.Room.AddDrop(soulDrop);
                var se = new Entity();
                se.AddComponent(new SpriteRenderer("SoulDrop"));
                se.AddComponent(new DropController(soulDrop));
                se.Transform.Position = Entity.Transform.Position;
                _sceneManager.ActiveScene.AddEntity(se);
                return;
            }
            
            if (_gs.Player.Bounds.Intersects(CalculateBounds(Entity.Transform.Position)))
            {
                _gs.Player.Hit(_enemy.Damage);
            }
        }

        private static Rectangle CalculateBounds(Vector2 position)
        {
            return new Rectangle(
                (position - new Vector2(32, 32)).ToPoint(),
                new Point(64, 64)
            );
        }
    }
}
