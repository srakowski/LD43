using LD43.Engine;
using LD43.Gameplay.Behaviors;

namespace LD43.Gameplay.Scenes
{
    public class Dungeon : Scene
    {
        public static Dungeon Create(object state)
        {
            var d = new Dungeon();

            var player = new Entity();
            player.AddComponent(new SpriteRenderer("PlayerPlaceholder"));
            player.AddComponent(new PlayerController());
            d.AddEntity(player);

            return d;
        }
    }
}
