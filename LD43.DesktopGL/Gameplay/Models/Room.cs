using LD43.Engine;
using Microsoft.Xna.Framework;
using System.Linq;
using System;
using System.Collections.Generic;

namespace LD43.Gameplay.Models
{
    public class Room
    {
        public Room(RoomConfig config)
        {
            PlayerStartPosition = config.PlayerStartPosition;

            Bounds = new Rectangle(
                Point.Zero,
                new Point(
                    config.SizeX * config.TileSize,
                    config.SizeY * config.TileSize
                )
            );

            TileSize = config.TileSize;

            var tm = new Tile[config.SizeX, config.SizeY];
            for (var x = 0; x < config.SizeX; x++)
                for (var y = 0; y < config.SizeY; y++)
                {
                    var roomTile = config.Tiles.First(t => t.Position.X == x && t.Position.Y == y);
                    tm[x, y] = new Tile(new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize), roomTile.TextureName);
                }
            Tilemap = tm;
        }

        public Point PlayerStartPosition { get; }

        public Rectangle Bounds { get; }

        public int TileSize { get; }

        public Tile[,] Tilemap { get; }

        public IEnumerable<Tile> GetTilesNear(Vector2 position)
        {
            List<Tile> tiles = new List<Tile>();
            var tilePoint = (position / TileSize).ToPoint();
            for (int x = tilePoint.X - 1; x < tilePoint.X + 2; x++)
                for (int y = tilePoint.Y - 1; y < tilePoint.Y + 2; y++)
                {
                    if (x < 0 || x >= Tilemap.GetLength(0)) continue;
                    if (y < 0 || y >= Tilemap.GetLength(1)) continue;
                    tiles.Add(Tilemap[x, y]);
                }
            return tiles;
        }
    }
}
