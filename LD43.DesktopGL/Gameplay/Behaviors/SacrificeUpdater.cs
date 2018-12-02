using LD43.Engine;
using System;

namespace LD43.Gameplay.Behaviors
{
    public class SacrificeUpdater : Behavior
    {
        private GameplayState _gs;
        private SpriteTextRenderer _spriteTextRenderer;

        public SacrificeUpdater(GameplayState gs, SpriteTextRenderer spriteTextRenderer)
        {
            _gs = gs;
            _spriteTextRenderer = spriteTextRenderer;
            SetHudText();
        }

        public override void Update()
        {
            _gs.DecrementSacrficeTimer(Delta);            
            SetHudText();
        }

        private void SetHudText()
        {
            var tr = TimeSpan.FromMilliseconds(_gs.SacrificeRequiredInMilleseconds);
            _spriteTextRenderer.Text =
                $"HP: {_gs.Player.HP}/{_gs.Player.MaxHP}\n" +
                $"Sacrifice Due In: {tr.Minutes.ToString("00")}:{tr.Seconds.ToString("00")}\n" +
                $"Gold: {_gs.Player.GoldCollected}/{_gs.Sacrifice.GoldRequired}\n" +
                $"Souls: {_gs.Player.SoulsCollected}/{_gs.Sacrifice.SoulsRequired}";
        }
    }
}
