using LD43.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.Xna.Framework;

namespace LD43.Gameplay.Behaviors
{
    public class HammerController : Behavior
    {
        private GameplayState gs;

        public bool _isSwinging = false;

        private int holdAngle = 0;

        public HammerController(GameplayState gs)
        {
            this.gs = gs;
        }

        public override void Initialize()
        {
            base.Initialize();
            StartCoroutine(CorrectSide());
        }

        private IEnumerator CorrectSide()
        {
            while (true)
            {
                yield return null;
                holdAngle = gs.Player.FacingDirection == Models.FacingDirection.Left ? -60 : 60;
                if (!_isSwinging)
                {
                    Entity.Transform.LocalRotation = MathHelper.ToRadians(holdAngle);
                }
            }
        }

        internal void Swing()
        {
            if (_isSwinging) return;
            _isSwinging = true;
            StartCoroutine(ExecuteSwing());
        }

        private IEnumerator ExecuteSwing()
        {
            if (holdAngle >= 0)
            {
                for (var a = 0; a < 360; a += 20)
                {
                    Entity.Transform.LocalRotation = (float)MathHelper.ToRadians(60 + a);
                    yield return null;
                }
            }
            else
            {
                for (var a = 0; a < 360; a += 20)
                {
                    Entity.Transform.LocalRotation = (float)MathHelper.ToRadians(-60 - a);
                    yield return null;
                }
            }
            _isSwinging = false;
        }
    }
}
