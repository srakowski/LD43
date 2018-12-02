using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LD43.Gameplay.Models
{
    public class RoomConfig
    {
        public int TileSize { get; set; }

        public int SizeX { get; set; }

        public int SizeY { get; set; }

        public IEnumerable<Tile> Tiles { get; set; }

        public IEnumerable<Inanimate> Inanimates { get; set; }

        public Point PlayerStartPosition { get; set; }

        public class Tile
        {
            public Point Position { get; set; }
            public string TextureName { get; set; }
        }

        public class Inanimate
        {
            public Point Position { get; set; }
            public InanimateType Type { get; set; }
        }

        public class Enemy
        {
            public Point Position { get; set; }
            public EnemyType Type { get; set; }
        }
    }
}
