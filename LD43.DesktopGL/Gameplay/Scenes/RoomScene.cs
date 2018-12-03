using LD43.Engine;
using LD43.Gameplay.Behaviors;
using LD43.Gameplay.Models;
using Microsoft.Xna.Framework;
using System.Linq;
using System;

namespace LD43.Gameplay.Scenes
{
    public class RoomScene : Scene
    {
        public static RoomScene Create(object state)
        {
            var gs = state as GameplayState;

            var s = new RoomScene();

            var room = gs.CurrentRoom;

            var roomEntity = new Entity();
            roomEntity.AddComponent(new RoomController(room, gs));
            roomEntity.AddComponent(new TilemapRenderer(
                tileWidth: room.TileSize,
                tileHeight: room.TileSize,
                tilemap: room.Tilemap
            ));
            s.AddEntity(roomEntity);

            room.Inanimates.Select(i => InanimateEntity(gs, i)).ToList().ForEach(i => s.AddEntity(i));
            room.Enemies.Select(i => EnemyEntity(gs, i)).ToList().ForEach(i => s.AddEntity(i));

            var player = gs.Player;
            if (player.Position == Vector2.Zero) player.Transform.Position = room.PlayerStartPosition;
            s.AddEntity(player);
            s.AddEntity(player.Hammer);            

            var camera = new Entity();
            camera.AddComponent(new Camera());
            camera.AddComponent(new CameraController(player, gs));
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
                    e.Transform.Position = inanimate.Position + new Vector2(64, 64);
                    return e;
                },
                sacrificialFirePit: () =>
                {
                    var e = new Entity();
                    e.AddComponent(new SpriteRenderer("SacrificialFirePit") { Layer = "Inanimates" });
                    e.AddComponent(new SacrificePitController(gs));
                    e.Transform.Position = inanimate.Position + new Vector2(128, 128);
                    return e;
                }
            );

        private static Entity EnemyEntity(GameplayState gs, Enemy enemy) =>
            enemy.Type.Match(
                star: () =>
                {
                    var e = new Entity();
                    e.Transform.Position = enemy.Position + new Vector2(64, 64);
                    e.AddComponent(new SpriteRenderer("StarEnemy") { Layer = "Enemies" });
                    e.AddComponent(new EnemyController(gs, enemy));
                    e.AddComponent(new ProjectileSpawner(gs,
                        new SequencedCircularProjectileSpawnerStrategy(12, gs.Random.Next(3000), 200, 10000),
                        (pos, dir) =>
                        {
                            var proj = new Entity();
                            proj.Transform.Position = pos;
                            proj.AddComponent(new SpriteRenderer("StarEnemy_Shot") { Layer = "Projectiles" });
                            proj.AddComponent(new ProjectileController(dir, 0.3f, gs, 10));
                            return proj;
                        }
                    ));
                    return e;
                }
            );
    }
}
