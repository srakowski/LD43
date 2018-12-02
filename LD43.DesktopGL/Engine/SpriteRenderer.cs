#pragma warning disable CS0618 // Type or member is obsolete

using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LD43.Engine
{
    public class SpriteRenderer : Renderer
    {
        private KeyValuePair<string, Texture2D> _textureCache;

        public string TextureName { get; set; }

        public Color Color { get; set; } = Color.White;

        public bool Center { get; set; } = true;

        public SpriteRenderer(string textureName)
        {
            TextureName = textureName;
        }

        public override void Draw(SpriteBatch spriteBatch, Dictionary<string, object> assetCatalog)
        {
            if (_textureCache.Key != TextureName)
            {
                _textureCache = new KeyValuePair<string, Texture2D>(
                    TextureName,
                    assetCatalog[TextureName] as Texture2D
                );
            }

            var transform = Entity.Transform;

            spriteBatch.Draw(
                texture: _textureCache.Value,
                position: transform.Position,
                color: Color,
                rotation: transform.Rotation,
                origin: Center ? _textureCache.Value.Bounds.Center.ToVector2() : Vector2.Zero
            );
        }
    }

    public class SpriteTextRenderer : Renderer
    {
        private KeyValuePair<string, SpriteFont> _fontCache;

        public string SpriteFontName { get; set; }

        public string Text { get; set; }

        public Color Color { get; set; } = Color.White;

        public bool Center { get; set; } = false;

        public SpriteTextRenderer(string spriteFontName)
        {
            SpriteFontName = spriteFontName;
        }

        public override void Draw(SpriteBatch spriteBatch, Dictionary<string, object> assetCatalog)
        {
            if (_fontCache.Key != SpriteFontName)
            {
                _fontCache = new KeyValuePair<string, SpriteFont>(
                    SpriteFontName,
                    assetCatalog[SpriteFontName] as SpriteFont
                );
            }

            var transform = Entity.Transform;

            var bounds = _fontCache.Value.MeasureString(Text);

            spriteBatch.DrawString(
                spriteFont: _fontCache.Value,
                text: Text,
                position: transform.Position,
                color: Color,
                rotation: transform.Rotation,
                scale: transform.Scale,
                origin: Center ? (bounds / 2f) : Vector2.Zero,
                effects: SpriteEffects.None,
                layerDepth: 0f
            );
        }
    }
}
