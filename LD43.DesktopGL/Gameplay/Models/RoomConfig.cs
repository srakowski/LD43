using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LD43.Gameplay.Models
{
    public class RoomConfig
    {
        public int TileSize => 128;

        public int SizeX { get; set; }

        public int SizeY { get; set; }

        public IEnumerable<Tile> Tiles { get; set; }

        public Point PlayerStartPosition { get; set; }

        public class Tile
        {
            public Point Position { get; set; }
            public string TextureName { get; set; }
        }
    }
}
