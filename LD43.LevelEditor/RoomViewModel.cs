using LD43.Gameplay.Models;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace LD43.LevelEditor
{
    public class RoomViewModel
    {
        private List<RoomTileViewModel> _tiles = new List<RoomTileViewModel>();

        public string Name { get; set; }

        public int Width { get; private set; } = 20;

        public int Height { get; private set; } = 20;

        public int TileSize { get; set; } = 128;

        public string LeftClickTexture { get; set; } = "Tile_FG";
        public string RightClickTexture { get; set; } = "Tile_BG";
        public string Mode { get; set; } = "Tile";

        public Point PlayerStartPosition { get; set; } = Point.Zero;

        public IEnumerable<RoomTileViewModel> Tiles => _tiles;

        public List<InanimateTypeViewModel> Inanimates { get; set; } = new List<InanimateTypeViewModel>();

        public RoomViewModel()
        {
            for (int r= 0; r < Width; r++)
                for (int c = 0; c < Height; c++)
                {
                    _tiles.Add(new RoomTileViewModel
                    {
                        Position = new Point(r, c)
                    });
                }
        }

        public void Save(string path)
        {
            var room = new RoomConfig();
            room.SizeX = Width;
            room.SizeY = Height;
            room.PlayerStartPosition = PlayerStartPosition;
            room.TileSize = TileSize;
            room.Tiles = Tiles.Select(t => new RoomConfig.Tile
            {
                Position = t.Position,
                TextureName = t.TextureName,
            });
            room.Inanimates = Inanimates.Select(i => new RoomConfig.Inanimate
            {
                Position = i.Position,
                Type = (InanimateType)Enum.Parse(typeof(InanimateType), i.Type)
            });
            File.WriteAllText(path, JsonConvert.SerializeObject(room));
        }

        public RoomTileViewModel SelectedTile { get; set; }

        public string InanimateType { get; internal set; }
        public int SnapInanimateTo { get; set; } = 64;

        public void Select(RoomTileViewModel roomTileViewModel)
        {
            SelectedTile = roomTileViewModel;
        }
    }

    public class RoomTileViewModel
    {
        public Point Position { get; set; }
        public string TextureName { get; set; } = "Tile_BG";
    }

    public class InanimateTypeViewModel
    {
        public string Type { get; set; }
        public Point Position { get; set; }
    }
}
