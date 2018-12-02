using LD43.Engine;
using System;

namespace LD43.Gameplay.Behaviors
{
    public class SacrificeTimerUpdater : Behavior
    {
        private GameplayState _gs;
        private SpriteTextRenderer _spriteTextRenderer;

        public SacrificeTimerUpdater(GameplayState gs, SpriteTextRenderer spriteTextRenderer)
        {
            _gs = gs;
            _spriteTextRenderer = spriteTextRenderer;
        }

        public override void Update()
        {
            _gs.DecrementSacrficeTimer(Delta);
            var tr = TimeSpan.FromMilliseconds(_gs.SacrificeRequiredInMilleseconds);
            _spriteTextRenderer.Text = $"Sacrifice Required In: {tr.Minutes.ToString("00")}:{tr.Seconds.ToString("00")}";
        }
    }
}
