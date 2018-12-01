using LD43.Engine;
using Microsoft.Xna.Framework;

namespace LD43.Gameplay.Behaviors
{
    public class PlayerController : Behavior
    {
        public override void Update()
        {
            if (Input.GetControl<Button>(Controls.MoveLeft).IsDown())
            {
                Entity.Transform.Position += (new Vector2(-1, 0) * Delta);
            }
            else if (Input.GetControl<Button>(Controls.MoveRight).IsDown())
            {
                Entity.Transform.Position += (new Vector2(1, 0) * Delta);
            }
        }
    }
}
