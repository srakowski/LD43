using LD43.Gameplay.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LD43.Gameplay
{
    public class GameplayState
    {
        public Random Random { get; private set; }

        public float SacrificeRequiredInMilleseconds { get; private set; }

        public Sacrifice Sacrifice { get; private set; }

        public Room CurrentRoom { get; set; }

        public Room[] Rooms { get; set; }

        public Player Player { get; set; }

        public void DecrementSacrficeTimer(float delta)
        {
            if (SacrificeRequiredInMilleseconds <= 0f) return;
            SacrificeRequiredInMilleseconds -= delta;
            if (SacrificeRequiredInMilleseconds <= 0f) SacrificeRequiredInMilleseconds = 0f;
        }

        public static GameplayState Create()
        {
            var newGame = new GameplayState();

            newGame.Random = new Random();
            newGame.Sacrifice = Sacrifice.CreateFirst(newGame);
            newGame.SacrificeRequiredInMilleseconds = newGame.Sacrifice.TimeRequiredInMilleseconds;
            newGame.Player = new Player(newGame);
            newGame.Rooms = newGame.GenerateRooms();
            newGame.CurrentRoom = newGame.Rooms[0];

            return newGame;
        }

        private Room[] GenerateRooms()
        {
            var r1 = new RoomConfig();
            r1.SizeX = 20;
            r1.SizeY = 10;
            r1.TileSize = 128;
            r1.Inanimates = Enumerable.Empty<RoomConfig.Inanimate>();
            r1.Enemies = Enumerable.Empty<RoomConfig.Enemy>();
            r1.PlayerStartPosition = new Microsoft.Xna.Framework.Point(400, 400);
            var tiles = new List<RoomConfig.Tile>();
            for (int x = 0; x < r1.SizeX; x++)
                for (int y = 0; y < r1.SizeY; y++)
                {
                    tiles.Add(x == 0 || y == 0 || y == r1.SizeY - 1
                        ? new RoomConfig.Tile { Position = new Microsoft.Xna.Framework.Point(x, y), TextureName = "Tile_BG" }
                        : new RoomConfig.Tile { Position = new Microsoft.Xna.Framework.Point(x, y), TextureName = "Tile_FG" }
                    );
                }
            r1.Tiles = tiles;

            var r2 = new RoomConfig();
            r2.SizeX = 20;
            r2.SizeY = 10;
            r2.StartX = 20;
            r2.StartY = 0;
            r2.TileSize = 128;
            r2.Inanimates = Enumerable.Empty<RoomConfig.Inanimate>();
            r2.Enemies = Enumerable.Empty<RoomConfig.Enemy>();
            tiles = new List<RoomConfig.Tile>();
            for (int x = r2.StartX; x < r2.StartX + r2.SizeX; x++)
                for (int y = r2.StartY; y < r2.StartY + r2.SizeY; y++)
                {
                    tiles.Add(x == r2.StartX + r2.SizeX - 1 || y == 0 || y == r2.StartY + r2.SizeY - 1
                        ? new RoomConfig.Tile { Position = new Microsoft.Xna.Framework.Point(x, y), TextureName = "Tile_BG" }
                        : new RoomConfig.Tile { Position = new Microsoft.Xna.Framework.Point(x, y), TextureName = "Tile_FG" }
                    );
                }
            r2.Tiles = tiles;

            return new[]
            {
                new Room(r1, this),
                new Room(r2, this)
            };

            //var loc = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //return new[] {
            //    new Room(JsonConvert.DeserializeObject<RoomConfig>(File.ReadAllText(Path.Combine(loc, $"Content/Rooms/room.json"))), this),
            //    new Room(JsonConvert.DeserializeObject<RoomConfig>(File.ReadAllText(Path.Combine(loc, $"Content/Rooms/room2.json"))), this)
            //};
        }
    }
}
