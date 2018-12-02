using LD43.Engine;
using LD43.Gameplay.Models;

namespace LD43.Gameplay.Behaviors
{
    internal class DropController : Behavior
    {
        private Drop _drop;
        private SceneManager _sceneManager;

        public DropController(Drop drop)
        {
            _drop = drop;
        }

        public override void Initialize()
        {
            base.Initialize();
            _sceneManager = Services.GetService<SceneManager>();
        }

        public override void Update()
        {
            if (!_drop.IsPickedUp) return;
            _sceneManager.ActiveScene.RemoveEntity(Entity);
        }
    }
}