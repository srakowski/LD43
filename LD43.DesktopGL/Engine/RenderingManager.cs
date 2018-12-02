using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System;

namespace LD43.Engine
{
    public class RenderingManager : DrawableGameComponent
    {
        private Func<Dictionary<string, object>> _assetCatalogFactory;
        private Dictionary<string, object> _assetCatalog;

        private IEnumerable<Layer> _layers;

        private SceneManager _sceneManager;

        private SpriteBatch _spriteBatch;

        public IEnumerable<Layer> Layers => _layers.ToArray();

        public RenderingManager(Game game, Func<Dictionary<string, object>> assetCatalogFactory, IEnumerable<Layer> layers) : base(game)
        {
            _assetCatalogFactory = assetCatalogFactory;
            _layers = layers;
            if (!_layers.Any(l => l.Name == Layer.DefaultLayerName))
            {
                _layers = _layers.Concat(new[] { new Layer(Layer.DefaultLayerName, 0) });
            }
            _layers = _layers.OrderBy(l => l.ZIndex).ToArray();

            game.Components.Add(this);
            game.Services.AddService(this);
        }

        public override void Initialize()
        {
            base.Initialize();
            _sceneManager = Game.Services.GetService<SceneManager>();
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _assetCatalog = _assetCatalogFactory.Invoke();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (_sceneManager.ActiveScene == null) return;

            var renderers = _sceneManager
                .ActiveScene
                .Entities
                .SelectMany(e => e.Components)
                .OfType<Renderer>()
                .ToArray();

            var globalCamera = renderers
                .OfType<Camera>()
                .FirstOrDefault(c => c.Layer == null);

            var renderersByLayer = renderers
                .GroupBy(r => (r.Layer ?? Layer.DefaultLayerName));

            foreach (var layer in _layers)
            {
                if (!layer.Show) continue;
                var renderersThisLayer = renderersByLayer.FirstOrDefault(r => r.Key == layer.Name)?.ToList();
                if (renderersThisLayer == null || !renderersThisLayer.Any()) continue;
                var layerCamera = renderersThisLayer.OfType<Camera>().FirstOrDefault(c => c != globalCamera) ?? globalCamera;
                layer.Begin(_spriteBatch, globalCamera?.Entity?.Transform);
                renderersThisLayer.ForEach(r => r.Draw(_spriteBatch, _assetCatalog));
                layer.End(_spriteBatch);
            }
        }
    }

    public class Layer
    {
        public const string DefaultLayerName = "default";

        public Layer(string name, int zIndex, bool stickToCamera = false)
        {
            Name = name;
            ZIndex = zIndex;
            StickToCamera = stickToCamera;
        }

        public string Name { get; set; } = "";

        public int ZIndex { get; set; } = 0;

        public bool Show { get; set; } = true;

        public bool StickToCamera { get; set; }

        public void Begin(SpriteBatch spriteBatch, Transform cameraTransform)
        {
            spriteBatch.Begin(transformMatrix: StickToCamera ? (Matrix?)null : TransformationMatrix(spriteBatch.GraphicsDevice, cameraTransform));
        }
        
        public void End(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
        }

        private Matrix TransformationMatrix(GraphicsDevice graphicsDevice, Transform transform = null) =>
            Matrix.Identity *
            Matrix.CreateRotationZ(transform?.Rotation ?? 0f) *
            Matrix.CreateScale(transform?.Scale ?? 1f) *
            Matrix.CreateTranslation(-(transform?.Position.X ?? 0f), -(transform?.Position.Y ?? 0f), 0f) *
            Matrix.CreateTranslation(
                (graphicsDevice.Viewport.Width * 0.5f),
                (graphicsDevice.Viewport.Height * 0.5f),
                0f);
    }

    public abstract class Renderer : Component
    {
        public string Layer { get; set; }
        public virtual void Draw(SpriteBatch spriteBatch, Dictionary<string, object> assetCatalog) { }
    }
}
