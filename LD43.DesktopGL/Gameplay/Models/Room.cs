using LD43.Engine;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Collections.Generic;
using System;

namespace LD43.Gameplay.Models
{
    public class Room
    {
        private Point _offset;
        private List<Enemy> _enemies;
        private List<Inanimate> _inanimates;

        private List<Drop> _drops = new List<Drop>();

        public bool IsThroneRoom {get;}

        private int _intensity;

        public Room(RoomConfig config, Point offset, GameplayState gs, bool throneRoom, int intensity)
        {
            IsThroneRoom = throneRoom;
            _intensity = intensity;

            var worldOffset = (offset.ToVector2() * config.TileSize);

            PlayerStartPosition = config.PlayerStartPosition.ToVector2() + worldOffset;

            _offset = offset;

            Bounds = new Rectangle(
                offset.X * config.TileSize,
                offset.Y * config.TileSize,
                config.SizeX * config.TileSize,
                config.SizeY * config.TileSize
            );

            TileSize = config.TileSize;

            var tm = new Tile[config.SizeX, config.SizeY];
            for (int x = offset.X, xt = 0; x < offset.X + config.SizeX; x++, xt++)
                for (int y = offset.Y, yt = 0; y < offset.Y + config.SizeY; y++, yt++)
                {
                    var roomTile = config.Tiles.First(t => t.Position.X == xt && t.Position.Y == yt);
                    tm[xt, yt] = new Tile(
                        new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize),
                        roomTile.TextureName, 
                        new TileTag
                        {
                            RoomPosition = new Point(xt, yt),
                            IsImpassable = roomTile.TextureName == "Tile_FG",
                            IsPlatform = roomTile.TextureName == "Tile_PF",
                            SpawnGroup = roomTile.SpawnGroup,
                        });
                }
            Tilemap = tm;

            _inanimates = config.Inanimates.Select(i => new Inanimate(
                i.Position.ToVector2() + worldOffset,
                i.Type,
                gs)).ToList();

            _enemies = config.Enemies.Select(e => new Enemy(
                e.Position.ToVector2() + worldOffset,
                e.Type,
                gs)).ToList();
        }

        public Vector2 PlayerStartPosition { get; set; }

        public Rectangle Bounds { get; }

        public int TileSize { get; }

        public Tile[,] Tilemap { get; }

        public IEnumerable<Inanimate> Inanimates => _inanimates.ToArray();

        public IEnumerable<Enemy> Enemies => _enemies.ToArray();

        public IEnumerable<Drop> Drops => _drops.ToArray();

        internal void Enter()
        {
            _drops.Clear();
        }

        public IEnumerable<Tile> GetTilesNear(Vector2 position)
        {
            List<Tile> tiles = new List<Tile>();
            var tilePoint = (position / TileSize).ToPoint() - _offset;
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

        public void CloseEntries(string directionFlags)
        {
            var tiles = Flatten(Tilemap);
            var tilesToClose = Enumerable.Empty<Tile>();
            if (directionFlags.Contains("N"))
            {
                tilesToClose = tilesToClose.Concat(tiles.Where(t => (t.Tag as TileTag).RoomPosition.Y == 0));
            }
            if (directionFlags.Contains("E"))
            {
                tilesToClose = tilesToClose.Concat(tiles.Where(t => (t.Tag as TileTag).RoomPosition.X == 30));
            }
            if (directionFlags.Contains("S"))
            {
                tilesToClose = tilesToClose.Concat(tiles.Where(t => (t.Tag as TileTag).RoomPosition.Y == 30));
            }
            if (directionFlags.Contains("W"))
            {
                tilesToClose = tilesToClose.Concat(tiles.Where(t => (t.Tag as TileTag).RoomPosition.X == 0));
            }
            tilesToClose
                .Select(t =>
                {
                    var tag = t.Tag as TileTag;
                    tag.RoomPosition = tag.RoomPosition;
                    tag.IsImpassable = true;
                    tag.IsPlatform = false;
                    tag.SpawnGroup = 0;
                    return new { Pos = tag.RoomPosition, Tile = new Tile(t.Bounds, "Tile_FG", tag) };
                })
                .ToList()
                .ForEach(t => Tilemap[t.Pos.X, t.Pos.Y] = t.Tile);
        }

        public void Populate(GameplayState gs)
        {
            var seChance =
                _intensity < 4 ? 20 :
                _intensity < 8 ? 30 :
                _intensity < 12 ? 50 :
                100;

            var spawnGroups = Flatten(Tilemap)
                .GroupBy(t => (t.Tag as TileTag).SpawnGroup)
                .Where(t => t.Key != 0);

            foreach (var spawnGroup in spawnGroups)
            {
                var si = gs.Random.Next(100) < 20;
                var se = gs.Random.Next(100) < seChance;
                if (!spawnGroup.Any()) continue;
                var tileQueue = new Queue<Tile>(spawnGroup.OrderBy(sg => gs.Random.Next(100)));
                if (si)
                {
                    var tile = tileQueue.Dequeue();
                    _inanimates.Add(new Inanimate(tile.Bounds.Location.ToVector2(), InanimateType.Vase, gs));
                }
                if (se && tileQueue.Count() > 0)
                {
                    var tile = tileQueue.Dequeue();
                    _enemies.Add(new Enemy(tile.Bounds.Location.ToVector2(), EnemyType.StarEnemy, gs));
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

    public class TileTag
    {
        public Point RoomPosition { get; set; }
        public bool IsImpassable { get; set; }
        public bool IsPlatform { get; set; }
        public int SpawnGroup { get; set; }
    }

    public class Inanimate
    {
        public Inanimate(Vector2 pos, InanimateType type, GameplayState gs)
        {
            Position = pos;
            Type = type;
            GoldValue = 1;
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
            GoldValue = 0;
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
