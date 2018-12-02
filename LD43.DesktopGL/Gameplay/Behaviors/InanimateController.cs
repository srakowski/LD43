using LD43.Engine;
using LD43.Gameplay.Models;
using System;

namespace LD43.Gameplay.Behaviors
{
    public class InanimateController : Behavior
    {
        private Inanimate _inanimate;
        private SceneManager _sceneManager;

        public InanimateController(GameplayState gs, Inanimate inanimate)
        {
            _inanimate = inanimate;
        }

        public override void Initialize()
        {
            base.Initialize();
            _sceneManager = Services.GetService<SceneManager>();
        }

        public override void Update()
        {
            if (_inanimate.IsDestroyed)
            {
                _sceneManager.ActiveScene.RemoveEntity(Entity);
            }
        }
    }
}
