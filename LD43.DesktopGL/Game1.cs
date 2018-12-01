#pragma warning disable CS0618 // Type or member is obsolete

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD43
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920,
                PreferredBackBufferHeight = 1080
            };

            Content.RootDirectory = "Content";

            new Engine.InputManager(this, Gameplay.Controls.Create());
            new Engine.SceneManager(this, Gameplay.Scenes.Catalog.Create());
            new Engine.RenderingManager(this,
                Gameplay.Content.GetAssetCatalog,
                Gameplay.Layers.Create()
            );
            new Engine.BehaviorManager(this);
        }

        protected override void Initialize()
        {
            base.Initialize();
            var sceneManager = Services.GetService<Engine.SceneManager>();
            sceneManager.Load(typeof(Gameplay.Scenes.Dungeon), Gameplay.GameState.Create());
        }

        protected override void LoadContent()
        {
            Gameplay.Content.Load(Content);
            base.LoadContent();
        }
    }
}
