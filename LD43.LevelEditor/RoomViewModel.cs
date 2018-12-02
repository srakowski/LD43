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

        public int Width { get; private set; } = 31;

        public int Height { get; private set; } = 31;
        
        public int TileSize { get; set; } = 128;

        public string LeftClickTexture { get; set; } = "Tile_FG";
        public string RightClickTexture { get; set; } = "Tile_BG";
        public string Mode { get; set; } = "Tile";

        public Point PlayerStartPosition { get; set; } = Point.Zero;

        public IEnumerable<RoomTileViewModel> Tiles => _tiles;

        public List<InanimateTypeViewModel> Inanimates { get; set; } = new List<InanimateTypeViewModel>();

        public List<EnemyViewModel> Enemies { get; set; } = new List<EnemyViewModel>();

        public RoomViewModel()
        {
            for (int r= 0; r < Width; r++)
                for (int c = 0; c < Height; c++)
                {
                    var mx = Width / 2;
                    var my = Height / 2;
                    _tiles.Add(new RoomTileViewModel
                    {
                        Position = new Point(r, c),
                        TextureName = (r == 0 || c == 0 || r == Height - 1 || c == Width - 1) 
                            && !((r == 0 || r == Height - 1) && (c == mx - 1 || c == mx || c == mx + 1))
                            && !((r == my - 1 || r == my || r == (my + 1)) && (c == 0 || c == Width - 1))
                            ?  "Tile_FG" 
                            : "Tile_BG",
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
                SpawnGroup = t.SpawnGroup
            });
            room.Inanimates = Inanimates.Select(i => new RoomConfig.Inanimate
            {
                Position = i.Position,
                Type = (InanimateType)Enum.Parse(typeof(InanimateType), i.Type)
            });
            room.Enemies = Enemies.Select(i => new RoomConfig.Enemy
            {
                Position = i.Position,
                Type = (EnemyType)Enum.Parse(typeof(EnemyType), i.Type)
            });
            File.WriteAllText(path, JsonConvert.SerializeObject(room));
        }

        public RoomTileViewModel SelectedTile { get; set; }

        public string InanimateType { get; internal set; }

        public string EnemyType { get; internal set; }

        public int SnapTo { get; set; } = 64;

        public int SpawnGroup { get; internal set; }

        public void Select(RoomTileViewModel roomTileViewModel)
        {
            SelectedTile = roomTileViewModel;
        }

        internal void Load(RoomConfig c)
        {
            _tiles = c.Tiles.Select(t => new RoomTileViewModel
            {
                Position = t.Position,
                TextureName = t.TextureName,
                SpawnGroup = t.SpawnGroup
            }).ToList();
            PlayerStartPosition = c.PlayerStartPosition;
        }
    }

    public class RoomTileViewModel
    {
        public Point Position { get; set; }
        public string TextureName { get; set; } = "Tile_FG";
        public int SpawnGroup { get; set; } = 0;
    }

    public class InanimateTypeViewModel
    {
        public string Type { get; set; }
        public Point Position { get; set; }
    }

    public class EnemyViewModel
    {
        public string Type { get; set; }
        public Point Position { get; set; }
    }
}
