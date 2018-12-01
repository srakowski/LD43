using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LD43.Engine
{
    public class BehaviorManager : GameComponent
    {
        private InputManager _inputManager;
        private SceneManager _sceneManager;
        
        public BehaviorManager(Game game) : base(game)
        {
            game.Components.Add(this);
            game.Services.AddService(this);
        }

        public override void Initialize()
        {
            base.Initialize();
            _inputManager = Game.Services.GetService<InputManager>();
            _sceneManager = Game.Services.GetService<SceneManager>();
        }

        public override void Update(GameTime gameTime)
        {
            if (_sceneManager.ActiveScene == null) return;

            var delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            var behaviors = _sceneManager
                .ActiveScene
                .Entities
                .SelectMany(e => e.Components)
                .OfType<Behavior>()
                .ToList();

            behaviors.ForEach(b => b.Update(_inputManager, delta));
            behaviors.ForEach(b => b.UpdateCoroutines(gameTime));
        }
    }

    public abstract class Behavior : Component
    {
        private List<Coroutine> _pendingCoroutines = new List<Coroutine>();

        private List<Coroutine> _coroutines = new List<Coroutine>();

        protected InputManager Input { get; private set; }

        protected float Delta { get; private set; }

        public void Update(InputManager input, float delta)
        {
            Input = input;
            Delta = delta;
            Update();
        }

        public virtual void Update() { }

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            var coroutine = new Coroutine(routine);
            _pendingCoroutines.Add(coroutine);
            return coroutine;
        }

        public void UpdateCoroutines(GameTime gameTime)
        {
            _coroutines.RemoveAll(c => c.IsComplete);
            _coroutines.AddRange(_pendingCoroutines);
            _pendingCoroutines.Clear();
            _coroutines.ForEach(c => c.Update(gameTime));
        }
    }
}
