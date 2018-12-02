using LD43.Engine;
using LD43.Gameplay.Models;
using System;

namespace LD43.Gameplay.Behaviors
{
    public class InanimateController : Behavior
    {
        private Inanimate _inanimate;
        private SceneManager _sceneManager;
        private GameplayState _gs;

        public InanimateController(GameplayState gs, Inanimate inanimate)
        {
            _inanimate = inanimate;
            _gs = gs;
        }

        public override void Initialize()
        {
            base.Initialize();
            _sceneManager = Services.GetService<SceneManager>();
        }

        public override void Update()
        {
            if (_gs.IsGameOver) return;
            if (_inanimate.IsDestroyed)
            {
                _sceneManager.ActiveScene.RemoveEntity(Entity);
                if (_inanimate.GoldValue > 0)
                {
                    var drop = new GoldDrop(Entity.Transform.Position, _inanimate.GoldValue);
                    _gs.CurrentRoom.AddDrop(drop);

                    var e = new Entity();
                    e.AddComponent(new SpriteRenderer("GoldDrop"));
                    e.AddComponent(new DropController(drop));
                    e.Transform.Position = Entity.Transform.Position;
                    _sceneManager.ActiveScene.AddEntity(e);                    
                }
            }
        }
    }
}
