using LD43.Engine;
using LD43.Gameplay.Behaviors;
using LD43.Gameplay.Models;
using Microsoft.Xna.Framework;

namespace LD43.Gameplay.Scenes
{
    public class Dungeon : Scene
    {
        public static Dungeon Create(object state)
        {
            var gameState = state as GameplayState;

            gameState.Platforms = new[]
            {
                new Platform(0, 0, 128, 32),
            };

            var d = new Dungeon();

            var map = new Entity();
            map.AddComponent(new TilemapRenderer(
                tileWidth: 128,
                tileHeight: 128,
                tilemap: new Tile[,]
                {
                    {
                        new Tile(true, "PlatformPlaceholder"),
                    },
                }
            ));
            d.AddEntity(map);

            var player = new Entity();
            player.AddComponent(new SpriteRenderer("PlayerPlaceholder"));
            player.AddComponent(new PlayerController(gameState));
            player.Transform.Position = new Vector2(64, -95);
            d.AddEntity(player);

            var camera = new Entity();
            camera.AddComponent(new Camera());
            camera.AddComponent(new CameraController(player, 2000, 2000));
            d.AddEntity(camera);

            return d;
        }
    }
}
