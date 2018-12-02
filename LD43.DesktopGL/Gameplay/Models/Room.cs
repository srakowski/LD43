using LD43.Engine;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Collections.Generic;
using System;

namespace LD43.Gameplay.Models
{
    public class Room
    {
        private List<Enemy> _enemies;
        private List<Inanimate> _inanimates;

        private List<Drop> _drops = new List<Drop>();

        public Room(RoomConfig config, GameplayState gs)
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

            _inanimates = config.Inanimates.Select(i => new Inanimate(
                i.Position.ToVector2(),
                i.Type,
                gs)).ToList();

            _enemies = config.Enemies.Select(e => new Enemy(
                e.Position.ToVector2(),
                e.Type,
                gs)).ToList();
        }

        public Point PlayerStartPosition { get; }

        public Rectangle Bounds { get; }

        public int TileSize { get; }

        public Tile[,] Tilemap { get; }

        public IEnumerable<Inanimate> Inanimates => _inanimates.ToArray();

        public IEnumerable<Enemy> Enemies => _enemies.ToArray();

        public IEnumerable<Drop> Drops => _drops.ToArray();

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

        public void DestroyInanimates(IEnumerable<Inanimate> destroyedInanimates)
        {
            foreach (var i in destroyedInanimates)
            {
                i.IsDestroyed = true;
                _inanimates.Remove(i);
            }
        }

        public void AddDrop(Drop drop)
        {
            _drops.Add(drop);
        }

        public void PickupDrop(Drop drop)
        {
            drop.IsPickedUp = true;
            _drops.Remove(drop);
        }

        public void HitEnemies(IEnumerable<Enemy> hitEnemies)
        {
            foreach (var e in hitEnemies)
            {
                e.IsDead = true;
                _enemies.Remove(e);
            }
        }
    }

    public class Inanimate
    {
        public Inanimate(Vector2 pos, InanimateType type, GameplayState gs)
        {
            Position = pos;
            Type = type;
            GoldValue = gs.Random.Next(10);
        }

        public Vector2 Position { get; }

        public InanimateType Type { get; }

        public bool IsDestroyed { get; set; }

        public int GoldValue { get; }
    }

    public class Enemy
    {
        public Enemy(Vector2 pos, EnemyType type, GameplayState gs)
        {
            Position = pos;
            Type = type;
            GoldValue = gs.Random.Next(10);
            Damage = type.Match(
                star: () => 20
            );
        }

        public Vector2 Position { get; }

        public EnemyType Type { get; }

        public bool IsDead { get; set; }

        public int GoldValue { get; }

        public int Damage { get; set; }
    }
}
