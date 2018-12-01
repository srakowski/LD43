using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace LD43.Engine
{
    public class InputManager : GameComponent
    {
        public static KeyboardState PrevKBState { get; private set; }
        public static KeyboardState CurrKBState { get; private set; }

        private Dictionary<string, Control> _controls;

        public InputManager(Game game, Dictionary<string, Control> controls) : base(game)
        {
            _controls = controls;
            game.Components.Add(this);
            game.Services.AddService(this);
        }

        public override void Update(GameTime gameTime)
        {
            PrevKBState = CurrKBState;
            CurrKBState = Keyboard.GetState();
        }

        public T GetControl<T>(string name) where T : Control => _controls[name] as T;
    }

    public abstract class Control { }

    public class Button : Control
    {
        private Keys _key;

        public Button(Keys key)
        {
            _key = key;
        }

        public bool IsDown() => InputManager.CurrKBState.IsKeyDown(_key);
    }
}
