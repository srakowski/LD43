using LD43.Gameplay.Models;
using Microsoft.Xna.Framework;
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
            var loc = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var config1 = JsonConvert.DeserializeObject<RoomConfig>(File.ReadAllText(Path.Combine(loc, $"Content/Rooms/room.json")));
            var config2 = JsonConvert.DeserializeObject<RoomConfig>(File.ReadAllText(Path.Combine(loc, $"Content/Rooms/room2.json")));

            var configs = new[] { config1, config2 };

            var rooms = new Dictionary<Point, Room>();
            rooms.Add(Point.Zero, new Room(config1, Point.Zero, this));
            ExpandDungeon(rooms, configs, Point.Zero);
            CloseEntries(rooms);
            foreach (var room in rooms.Values) room.Populate(this);
            return rooms.Values.ToArray();
        }

        private void CloseEntries(Dictionary<Point, Room> rooms)
        {
            foreach (var key in rooms.Keys)
            {
                var value = rooms[key];
                var flags = "";
                if (!rooms.ContainsKey(key + new Point(0, -1))) flags += "N";
                if (!rooms.ContainsKey(key + new Point(1, 0))) flags += "E";
                if (!rooms.ContainsKey(key + new Point(0, 1))) flags += "S";
                if (!rooms.ContainsKey(key + new Point(-1, 0))) flags += "W";
                if (flags.Length > 0)
                {
                    value.CloseEntries(flags);
                }
            }
        }

        private void ExpandDungeon(Dictionary<Point, Room> rooms, RoomConfig[] configs, Point fromPoint)
        {
            var dirFlags = "NESW";
            if (CreateRoom(rooms, configs, fromPoint + new Point(0, -1))) dirFlags = dirFlags.Replace("N", "");
            if (CreateRoom(rooms, configs, fromPoint + new Point(1, 0))) dirFlags = dirFlags.Replace("E", "");
            if (CreateRoom(rooms, configs, fromPoint + new Point(0, 1))) dirFlags = dirFlags.Replace("S", "");
            //if (CreateRoom(rooms, configs, fromPoint + new Point(-1, 0))) dirFlags = dirFlags.Replace("W", "");
        }

        private bool CreateRoom(Dictionary<Point, Room> rooms, RoomConfig[] configs, Point point)
        {
            if (rooms.ContainsKey(point)) return true;
            
            var distanceFromCenter = Vector2.Distance(Vector2.Zero, point.ToVector2());
            var chanceToSpawn =
                distanceFromCenter < 2 ? 100 :
                distanceFromCenter < 3 ? 90 :
                distanceFromCenter < 5 ? 60 :
                distanceFromCenter < 8 ? 40 :
                10;

            bool shouldSpawn = Random.Next(100) < chanceToSpawn;
            if (!shouldSpawn) return false;

            var config = Random.Next(configs.Length);

            var room = new Room(configs[config], point * new Point(31, 31), this);
            rooms[point] = room;

            ExpandDungeon(rooms, configs, point);
            return true;
        }
    }
}
