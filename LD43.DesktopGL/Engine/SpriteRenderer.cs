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
                color: Color.White,
                origin: _textureCache.Value.Bounds.Center.ToVector2()
            );
        }
    }
}
