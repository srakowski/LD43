using LD43.Engine;
using LD43.Gameplay.Models;
using LD43.Gameplay.Scenes;
using System.Linq;

namespace LD43.Gameplay.Behaviors
{
    public class RoomController : Behavior
    {
        private Room _room;
        private GameplayState _gs;
        private SceneManager _sceneManager;

        public RoomController(Room room, GameplayState gs)
        {
            _room = room;
            _gs = gs;
        }

        public override void Initialize()
        {
            base.Initialize();
            _sceneManager = Services.GetService<SceneManager>();
            _room.Enter();
        }

        public override void Update()
        {
            if (!_gs.CurrentRoom.Bounds.Contains(_gs.Player.Position))
            {
                _gs.CurrentRoom = _gs.Rooms.First(r => r.Bounds.Contains(_gs.Player.Position));
                 _gs.CurrentRoom.PlayerStartPosition = _gs.Player.Position;
                _sceneManager.Load(typeof(RoomScene), _gs);
            }
        }
    }
}
