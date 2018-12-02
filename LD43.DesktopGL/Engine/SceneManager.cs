using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace LD43.Engine
{
    public class SceneManager : GameComponent
    {
        private Dictionary<Type, Func<object, Scene>> _sceneCatalog;

        public Scene ActiveScene { get; set; }

        public SceneManager(Game game, Dictionary<Type, Func<object, Scene>> sceneCatalog) : base(game)
        {
            _sceneCatalog = sceneCatalog;

            game.Services.AddService(this);
        }

        public void Load(Type sceneType, object state)
        {
            ActiveScene?.Deactivate();
            ActiveScene = _sceneCatalog[sceneType](state);
            ActiveScene.Activate(Game.Services);            
        }
    }
}
