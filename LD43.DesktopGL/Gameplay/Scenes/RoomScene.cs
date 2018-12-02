using LD43.Engine;
using LD43.Gameplay.Behaviors;
using LD43.Gameplay.Models;
using Microsoft.Xna.Framework;
using System.Linq;

namespace LD43.Gameplay.Scenes
{
    public class RoomScene : Scene
    {
        public static RoomScene Create(object state)
        {
            var gameState = state as GameplayState;

            var s = new RoomScene();

            var room = new Room(Content.GetAssetCatalog()["Rooms/room"] as RoomConfig);
            gameState.Room = room;

            var map = new Entity();
            map.AddComponent(new TilemapRenderer(
                tileWidth: room.TileSize,
                tileHeight: room.TileSize,
                tilemap: room.Tilemap
            ));
            s.AddEntity(map);

            var player = new Entity();
            player.AddComponent(new SpriteRenderer("PlayerPlaceholder"));
            player.AddComponent(new PlayerController(gameState));
            player.Transform.Position = room.PlayerStartPosition.ToVector2();
            s.AddEntity(player);

            var camera = new Entity();
            camera.AddComponent(new Camera());
            camera.AddComponent(new CameraController(player, room.Bounds));
            s.AddEntity(camera);

            return s;
        }
    }
}
