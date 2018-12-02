using LD43.Engine;

namespace LD43.Gameplay.Behaviors
{
    public class SacrificePitController : Behavior
    {
        private GameplayState _gs;

        public SacrificePitController(GameplayState gs)
        {
            _gs = gs;
        }

        public override void Initialize()
        {
        }

        public override void Update()
        {
            if (_gs.Player.Bounds.Contains(Entity.Transform.Position))
            {
                if (_gs.DoesPlayerHaveTheSacrifice())
                {
                    _gs.CompleteTheSacrifice();
                }
            }
        }
    }
}
