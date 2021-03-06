﻿using System;
using LD43.Engine;
using LD43.Gameplay.Behaviors;
using Microsoft.Xna.Framework;

namespace LD43.Gameplay.Models
{
    public class Player : Entity
    {
        public Player(GameplayState gs)
        {
            AddComponent(new SpriteRenderer("PlayerPlaceholder") { Layer = "Player" });
            AddComponent(new PlayerController(gs));

            Hammer = new Entity();
            Hammer.AddComponent(new SpriteRenderer("Hammer") { Layer = "Player", Origin = new Vector2(115, 215) });
            Hammer.Parent = this;
            Hammer.AddComponent(new HammerController(gs));
        }

        public Vector2 Position
        {
            get => Transform.Position;
            set => Transform.Position = value;
        }

        public Entity Hammer { get; set; }

        public Rectangle Bounds { get; set; }

        public FacingDirection FacingDirection { get; set; } = FacingDirection.Right;

        public int GoldCollected { get; set; }

        public int SoulsCollected { get; set; }

        public float HP { get; set; } = 5;

        public int MaxHP { get; set; } = 5;

        public bool IsInvulnerable { get; set; } = false;

        public bool GotHit { get; set; } = false;

        public bool IsDead { get; set; } = false;

        public string ReasonForDeath { get; private set; }

        public Action Shake { get; internal set; }

        public void Pickup(int goldToAdd, int soulsToAdd)
        {
            GoldCollected += goldToAdd;
            SoulsCollected += soulsToAdd;
        }

        internal void Hit()
        {
            if (IsInvulnerable) return;
            AudioPlayer.PlaySfx("hit");
            Shake?.Invoke();
            IsInvulnerable = true;
            HP -= 0.5f;
            if (HP <= 0f)
            {
                HP = 0f;
                Kill("You took too much damage.");
            };
            GotHit = true;
        }

        internal void Kill(string reason)
        {
            IsDead = true;
            ReasonForDeath = reason;
        }
    }
}
