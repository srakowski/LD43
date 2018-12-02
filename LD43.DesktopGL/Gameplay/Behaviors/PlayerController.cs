using LD43.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;

namespace LD43.Gameplay.Behaviors
{
    public class PlayerController : Behavior
    {
        private const int SwingRadius = 192;
        private const float _gravity = 0.009f;
        private readonly GameplayState _gs;
        private float _verticalSpeed = 0f;
        private bool _onPlatform = false;
        private bool _flyMode = false;
        private bool _isSwingingWeapon = false;

        public PlayerController(GameplayState gameplayState)
        {
            _gs = gameplayState;
        }

        public override void Update()
        {
            if (Input.GetControl<Button>(Controls.SwingWeapon).IsDown())
            {
                StartCoroutine(SwingingWeapon());
            }

            var newPlayerPosition = Entity.Transform.Position;
            var kb = Keyboard.GetState();
            if (InputManager.PrevKBState.IsKeyDown(Keys.F) && InputManager.CurrKBState.IsKeyUp(Keys.F))
            {
                _flyMode = !_flyMode;
            }
            if (_flyMode)
            {
                _verticalSpeed = 0f;
                if (kb.IsKeyDown(Keys.Up))
                {
                    newPlayerPosition += (new Vector2(0, -1f) * Delta);
                }
                if (kb.IsKeyDown(Keys.Down))
                {
                    newPlayerPosition += (new Vector2(0, 1f) * Delta);
                }
                if (kb.IsKeyDown(Keys.Left))
                {
                    newPlayerPosition += (new Vector2(-1f, 0) * Delta);
                }
                if (kb.IsKeyDown(Keys.Right))
                {
                    newPlayerPosition += (new Vector2(1f, 0) * Delta);
                }
            }
            else
            {
                _verticalSpeed += (_gravity * Delta);
                if (_verticalSpeed > 9.8f) _verticalSpeed = 9.8f;

                if (_onPlatform && Input.GetControl<Button>(Controls.Jump).IsDown())
                {
                    _verticalSpeed = -3.1f;
                    _onPlatform = false;
                }

                newPlayerPosition += (new Vector2(0, _verticalSpeed) * Delta);

                if (Input.GetControl<Button>(Controls.MoveLeft).IsDown())
                {
                    newPlayerPosition += (new Vector2(-1, 0) * Delta);
                }

                if (Input.GetControl<Button>(Controls.MoveRight).IsDown())
                {
                    newPlayerPosition += (new Vector2(1, 0) * Delta);
                }
            }

            var playerBounds = CalculateBounds(Entity.Transform.Position);
            var targetBounds = CalculateBounds(newPlayerPosition);

            var tiles = _gs.Room
                .GetTilesNear(newPlayerPosition)
                .Where(t => t.IsImpassable);

            if (!tiles.Any() || !tiles.Any(t => t.Bounds.Intersects(targetBounds)))
            {
                Entity.Transform.Position = newPlayerPosition;
                return;
            }
            
            Entity.Transform.Position = StepToNewPosition(newPlayerPosition, tiles);
        }

        private IEnumerator SwingingWeapon()
        {
            _isSwingingWeapon = true;
            var destroyedInanimates = _gs.Room
                .Inanimates
                .Where(i => Vector2.Distance(Entity.Transform.Position, i.Position) < SwingRadius)
                .Where(i =>
                    (i.Position.X < Entity.Transform.Position.X && _gs.Player.FacingDirection == Models.FacingDirection.Left) ||
                    (i.Position.X > Entity.Transform.Position.X && _gs.Player.FacingDirection == Models.FacingDirection.Right)
                );
            if (destroyedInanimates.Any())
            {
                _gs.Room.DestroyInanimates(destroyedInanimates);
            }
            yield return WaitYieldInstruction.Create(400);
            _isSwingingWeapon = false;
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
                        _onPlatform = true;
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

            return prevStep;
        }

        private static Rectangle CalculateBounds(Vector2 p)
        {
            return new Rectangle(
                (p - new Vector2(64, 96)).ToPoint(),
                new Point(128, 192)
            );
        }
    }
}
