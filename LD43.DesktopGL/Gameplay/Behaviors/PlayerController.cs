using LD43.Engine;
using LD43.Gameplay.Models;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using LD43.Gameplay.Scenes;

namespace LD43.Gameplay.Behaviors
{
    public class PlayerController : Behavior
    {
        private const int RecoupTime = 1000;
        private const int SwingRadius = 256;
        private const int SwingTime = 200;
        private const float _gravity = 0.009f;
        private readonly GameplayState _gs;
        private float _verticalSpeed = 0f;
        private float _horizontalSpeed = 0f;
        private bool _grounded = false;
        private bool _groundedOnPlatform = false;
        private bool _isSwingingWeapon = false;
        private bool _isKnockingBack = false;
        private int? _ignorePlatformsUntilY = null;
        private bool _dead = false;
        private bool _win = false;

        public PlayerController(GameplayState gameplayState)
        {
            _gs = gameplayState;
        }

        public override void Initialize()
        {
            base.Initialize();
            _gs.Player.Bounds = CalculateBounds(Entity.Transform.Position);            
        }

        public override void Update()
        {
            if (_gs.YouWin && !_win)
            {
                _win = true;
                StartCoroutine(Win());
            }

            if (_win) return;


            if (_gs.Player.IsDead && !_dead)
            {
                _dead = true;
                StartCoroutine(Death());
            }

            if (_dead) return;

            if (_gs.Player.GotHit)
            {
                StartCoroutine(HandleKnockback());
            }

            if (!_isSwingingWeapon && !_isKnockingBack && Input.GetControl<Button>(Controls.SwingWeapon).IsDown())
            {
                StartCoroutine(SwingWeapon());
            }

            if (!_isKnockingBack)
            {
                PickupDrops();
            }

            HandleMovement();
            _gs.Player.Bounds = CalculateBounds(Entity.Transform.Position);
        }

        private IEnumerator HandleKnockback()
        {
            _isKnockingBack = true;
            _gs.Player.GotHit = false;
            _gs.Player.IsInvulnerable = true;
            _verticalSpeed = -2;
            _horizontalSpeed = _gs.Player.FacingDirection == Models.FacingDirection.Right ? -1 : 1;
            _grounded = false;
            while (!_grounded)
            {
                yield return null;
            }
            _horizontalSpeed = 0;
            _isKnockingBack = false;
            yield return WaitYieldInstruction.Create(RecoupTime);
            _gs.Player.IsInvulnerable = false;
        }

        private IEnumerator SwingWeapon()
        {
            _isSwingingWeapon = true;
            ExecuteAttack();
            yield return WaitYieldInstruction.Create(SwingTime);
            ExecuteAttack();
            _isSwingingWeapon = false;
        }

        private void ExecuteAttack()
        {
            var destroyedInanimates = _gs.CurrentRoom.Inanimates
                            .Where(i => Vector2.Distance(Entity.Transform.Position, i.Position) < SwingRadius)
                            .Where(i => IsFacing(i.Position));
            if (destroyedInanimates.Any())
            {
                _gs.CurrentRoom.DestroyInanimates(destroyedInanimates);
            }

            var hitEnemies = _gs.CurrentRoom.Enemies
                .Where(i => Vector2.Distance(Entity.Transform.Position, i.Position) < SwingRadius)
                .Where(i => IsFacing(i.Position));
            if (hitEnemies.Any())
            {
                _gs.CurrentRoom.HitEnemies(hitEnemies);
            }
        }

        private void PickupDrops()
        {
            var bounds = CalculateBounds(Entity.Transform.Position);
            _gs.CurrentRoom.Drops
                .Where(d => bounds.Contains(d.Position))
                .ToList()
                .ForEach(d =>
                {
                    var r = d.Match(
                        goldDrop: g => new { GoldToAdd = g.Value, SoulsToAdd = 0 },
                        soulDrop: s => new { GoldToAdd = 0, SoulsToAdd = s.Value }
                    );
                    _gs.Player.Pickup(r.GoldToAdd, r.SoulsToAdd);
                    _gs.CurrentRoom.PickupDrop(d);
                });
        }

        private void HandleMovement()
        {
            if (Input.GetControl<Button>(Controls.ResetPosition).IsDown())
            {
                Entity.Transform.Position = _gs.CurrentRoom.PlayerStartPosition;
                return;
            }

            var newPlayerPosition = Entity.Transform.Position;

            _verticalSpeed += (_gravity * Delta);
            if (_verticalSpeed > 9.8f) _verticalSpeed = 9.8f;

            if (_grounded && Input.GetControl<Button>(Controls.Jump).IsDown() && !_isKnockingBack)
            {
                _verticalSpeed = -3.1f;
                _grounded = false;
            }
            else if (_groundedOnPlatform && Input.GetControl<Button>(Controls.Drop).IsDown())
            {
                _ignorePlatformsUntilY = (int)(Entity.Transform.Position.Y + 128);
                _groundedOnPlatform = false;
            }

            newPlayerPosition += (new Vector2(_horizontalSpeed, _verticalSpeed) * Delta);

            if (Input.GetControl<Button>(Controls.MoveLeft).IsDown() && !_isKnockingBack)
            {
                if (_gs.Player.FacingDirection == Models.FacingDirection.Left)
                {
                    newPlayerPosition += (new Vector2(-1, 0) * Delta);
                }
                _gs.Player.FacingDirection = Models.FacingDirection.Left;
            }

            if (Input.GetControl<Button>(Controls.MoveRight).IsDown() && !_isKnockingBack)
            {
                if (_gs.Player.FacingDirection == Models.FacingDirection.Right)
                {
                    newPlayerPosition += (new Vector2(1, 0) * Delta);
                }
                _gs.Player.FacingDirection = Models.FacingDirection.Right;
            }

            var playerBounds = CalculateBounds(Entity.Transform.Position);
            var targetBounds = CalculateBounds(newPlayerPosition);

            _ignorePlatformsUntilY =
                _ignorePlatformsUntilY.HasValue && Entity.Transform.Position.Y < _ignorePlatformsUntilY.Value
                    ? _ignorePlatformsUntilY
                    : null;

            var tiles = _gs.CurrentRoom
                .GetTilesNear(newPlayerPosition)
                .Where(t => (t.Tag as TileTag).IsImpassable ||
                    ((t.Tag as TileTag).IsPlatform && playerBounds.Bottom < (t.Bounds.Top + 1)))
                .OrderBy(t => (t.Tag as TileTag).IsImpassable ? 0 : 1);

            if (!tiles.Any() || !tiles.Any(t => t.Bounds.Intersects(targetBounds)))
            {
                Entity.Transform.Position = newPlayerPosition;
                return;
            }

            Entity.Transform.Position = StepToNewPosition(newPlayerPosition, tiles);
        }

        private Vector2 StepToNewPosition(Vector2 newPlayerPosition, IEnumerable<Tile> tiles)
        {
            var step = Vector2.Distance(Entity.Transform.Position, newPlayerPosition) / 200f;

            var prevStep = Entity.Transform.Position;
            int i = 0;

            Tile? collTile = null;

            for (i = 0; i < 200; i++)
            {
                var currStep = Vector2.SmoothStep(prevStep, newPlayerPosition, step);
                if (tiles.Any(t => t.Bounds.Intersects(CalculateBounds(currStep))))
                {
                    collTile = tiles.First(t => t.Bounds.Intersects(CalculateBounds(currStep)));
                    break;
                }
                prevStep = currStep;
            }

            var hitY = true;

            var nX = new Vector2(newPlayerPosition.X, prevStep.Y);
            for (var first = true; i < 200; i++)
            {
                var currStep = Vector2.SmoothStep(prevStep, nX, step);
                if (tiles.Any(t => t.Bounds.Intersects(CalculateBounds(currStep))))
                {
                    if (!first)
                    {
                        collTile = tiles.First(t => t.Bounds.Intersects(CalculateBounds(currStep)));
                    }
                    break;
                }
                prevStep = currStep;
                first = false;
            }

            var nY = new Vector2(prevStep.X, newPlayerPosition.Y);
            for (var first = true; i < 200; i++)
            {
                var currStep = Vector2.SmoothStep(prevStep, nY, step);
                if (tiles.Any(t => t.Bounds.Intersects(CalculateBounds(currStep))))
                {
                    if (!first)
                    {
                        collTile = tiles.First(t => t.Bounds.Intersects(CalculateBounds(currStep)));
                    }
                    hitY = true;
                    break;
                }
                prevStep = currStep;
                first = false;
                hitY = false;
            }

            var bounds = CalculateBounds(prevStep);

            if (hitY)
            {
                if (_verticalSpeed >= 0)
                {
                    if (collTile.HasValue && collTile.Value.Bounds.Y >= bounds.Bottom)
                    {
                        _grounded = true;
                        _verticalSpeed = 0;
                    }
                }

                if (_verticalSpeed <= 0)
                {
                    if (collTile.HasValue && collTile.Value.Bounds.Y <= bounds.Top)
                    {
                        _verticalSpeed = 0;
                    }
                }
            }

            _groundedOnPlatform = collTile.HasValue && (collTile.Value.Tag as TileTag).IsPlatform;
            if (_groundedOnPlatform && _ignorePlatformsUntilY.HasValue)
            {
                _groundedOnPlatform = false;
                return newPlayerPosition;
            }

            return prevStep;
        }

        private static Rectangle CalculateBounds(Vector2 p)
        {
            var r = new Rectangle(
                (p + new Vector2(-64, -96)).ToPoint(),
                new Point(128, 192)
            );
            return r;
        }

        private bool IsFacing(Vector2 pos)
        {
            var bounds = CalculateBounds(Entity.Transform.Position);
            return
                (pos.X < bounds.Right && _gs.Player.FacingDirection == Models.FacingDirection.Left) ||
                (pos.X > bounds.Left && _gs.Player.FacingDirection == Models.FacingDirection.Right);
        }

        private IEnumerator Win()
        {
            var sm = Services.GetService<SceneManager>();
            Services.GetService<RenderingManager>().Layers.Where(l => l.Name != "Player").ToList().ForEach(l => l.Show = false);
            var e = new Entity();
            e.Transform.Position = Entity.Transform.Position + new Vector2(0, 200);
            e.AddComponent(new SpriteTextRenderer("GenericFont")
            {
                Text = $"You have completed all required sacrifices and have won the game.\nSee you next Ludum Dare!\nPress [Enter] to play again...",
                Layer = "Player",
                Center = true,
            });
            sm.ActiveScene.AddEntity(e);
            yield return WaitYieldInstruction.Create(500);
            while (!Input.GetControl<Button>(Controls.Continue).IsDown()) yield return null;
            sm.Load(typeof(RoomScene), GameplayState.Create());
        }

        private IEnumerator Death()
        {
            var sm = Services.GetService<SceneManager>();
            Services.GetService<RenderingManager>().Layers.Where(l => l.Name != "Player").ToList().ForEach(l => l.Show = false);
            var e = new Entity();
            e.Transform.Position = Entity.Transform.Position + new Vector2(0, 200);
            e.AddComponent(new SpriteTextRenderer("GenericFont")
            {
                Text = $"You have died.\n{_gs.Player.ReasonForDeath}\nPress [Enter] to play again...",
                Layer = "Player",
                Center = true,
            });
            sm.ActiveScene.AddEntity(e);
            yield return WaitYieldInstruction.Create(500);
            while (!Input.GetControl<Button>(Controls.Continue).IsDown()) yield return null;
            sm.Load(typeof(RoomScene), GameplayState.Create());
        }
    }
}
