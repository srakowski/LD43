using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace LD43.Engine
{
    public struct Tile
    {
        public Tile(bool hasTile, string texture = null)
        {
            HasTile = hasTile;
            TextureName = texture;
        }
        public bool HasTile { get; }
        public string TextureName { get; }
    }

    public class TilemapRenderer : Renderer
    {
        private readonly int _tileWidth;
        private readonly int _tileHeight;
        private readonly Tile[,] _tilemap;

        private Dictionary<string, Texture2D> _textureCache;        

        public TilemapRenderer(int tileWidth, int tileHeight, Tile[,] tilemap)
        {
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;
            _tilemap = tilemap;
        }

        public override void Draw(SpriteBatch spriteBatch, Dictionary<string, object> assetCatalog)
        {
            if (_textureCache == null)
            {
                _textureCache = Flatten(_tilemap)
                    .Where(t => t.HasTile)
                    .Select(t => t.TextureName)
                    .Distinct()
                    .Select(tn => new { Key = tn, Value = assetCatalog[tn] as Texture2D })
                    .ToDictionary(kn => kn.Key, kn => kn.Value);
            }

            for (var r = 0; r < _tilemap.GetLength(0); r++)
            {
                for (var c = 0; c < _tilemap.GetLength(1); c++)
                {
                    var tile = _tilemap[r, c];
                    if (!tile.HasTile) continue;
                    spriteBatch.Draw(
                        texture: _textureCache[tile.TextureName],
                        position: new Vector2(r * _tileWidth, r * _tileHeight),
                        color: Color.White
                    );
                }
            }
        }

        private static IEnumerable<Tile> Flatten<Tile>(Tile[,] map)
        {
            for (var r = 0; r < map.GetLength(0); r++)
            {
                for (var c = 0; c < map.GetLength(1); c++)
                {
                    yield return map[r, c];
                }
            }
        }
    }
}
