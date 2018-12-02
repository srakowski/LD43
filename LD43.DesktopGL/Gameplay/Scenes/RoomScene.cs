using LD43.Engine;
using LD43.Gameplay.Behaviors;
using LD43.Gameplay.Models;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace LD43.Gameplay.Scenes
{
    public class RoomScene : Scene
    {
        public static RoomScene Create(object state)
        {
            var gs = state as GameplayState;

            var s = new RoomScene();

            var room = new Room(Content.GetAssetCatalog()["Rooms/room"] as RoomConfig, gs);
            gs.Room = room;

            var map = new Entity();
            map.AddComponent(new TilemapRenderer(
                tileWidth: room.TileSize,
                tileHeight: room.TileSize,
                tilemap: room.Tilemap
            ));
            s.AddEntity(map);

            room.Inanimates
                .Select(i => InanimateEntity(gs, i))
                .ToList()
                .ForEach(i => s.AddEntity(i));

            var player = new Entity();
            player.AddComponent(new SpriteRenderer("PlayerPlaceholder")
            {
                Layer = "Player",
            });
            player.AddComponent(new PlayerController(gs));
            player.Transform.Position = room.PlayerStartPosition.ToVector2();
            s.AddEntity(player);            


            var camera = new Entity();
            camera.AddComponent(new Camera());
            camera.AddComponent(new CameraController(player, room.Bounds));
            s.AddEntity(camera);

            var sacrificeTimer = new Entity();
            var spriteTextRenderer = new SpriteTextRenderer("GenericFont")
            {
                Layer = "Hud",
            };
            sacrificeTimer.AddComponent(spriteTextRenderer);
            sacrificeTimer.AddComponent(new SacrificeUpdater(gs, spriteTextRenderer));
            sacrificeTimer.Transform.Position = new Vector2(100, 100);
            s.AddEntity(sacrificeTimer);

            return s;
        }

        private static Entity InanimateEntity(GameplayState gs, Inanimate inanimate) =>
            inanimate.Type.Match(
                vase: () =>
                {
                    var e = new Entity();
                    e.AddComponent(new SpriteRenderer("Vase") { Layer = "Inanimates" });
                    e.AddComponent(new InanimateController(gs, inanimate));
                    e.Transform.Position = inanimate.Position;
                    return e;
                }
            );
    }
}
