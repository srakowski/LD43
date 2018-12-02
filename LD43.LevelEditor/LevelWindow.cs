using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace LD43.LevelEditor
{
    public class LevelWindow : MonoGame.Forms.Controls.UpdateWindow
    {
        public Dictionary<string, Texture2D> textures;

        public RoomViewModel RVM { get; internal set; }

        public IEnumerable<string> TexturesToLoad { get; set; }

        public Vector2 position;

        protected override void Initialize()
        {
            base.Initialize();
            if (textures == null)
            {
                InitTextures();
            }
        }

        private void InitTextures()
        {
            if (TexturesToLoad == null) return;
            textures = new Dictionary<string, Texture2D>();
            Editor.Content.RootDirectory = @"C:\Users\srako\Desktop\LD43\LD43.DesktopGL\bin\DesktopGL\AnyCPU\Debug\Content";
            foreach (var file in TexturesToLoad)
            {
                textures[file] = Editor.Content.Load<Texture2D>(file);
            }
        }

        private Matrix TransformationMatrix(Vector2 position) =>
            Matrix.Identity *
            Matrix.CreateTranslation(-position.X, -position.Y, 0f) *
            Matrix.CreateTranslation(
                (Editor.graphics.Viewport.Width * 0.5f),
                (Editor.graphics.Viewport.Height * 0.5f),
                0f);

        public Vector2 ToWorldCoords(Vector2 coords) =>
            Vector2.Transform(coords, Matrix.Invert(this.TransformationMatrix(position)));

        protected override void Update(GameTime gameTime)
        {
            if (textures == null)
            {
                InitTextures();
            }

            var kb = Keyboard.GetState();
            var Delta = 20;
            if (kb.IsKeyDown(Keys.Up))
            {
                position += (new Vector2(0, -1f) * Delta);
            }
            if (kb.IsKeyDown(Keys.Down))
            {
                position += (new Vector2(0, 1f) * Delta);
            }
            if (kb.IsKeyDown(Keys.Left))
            {
                position += (new Vector2(-1f, 0) * Delta);
            }
            if (kb.IsKeyDown(Keys.Right))
            {
                position += (new Vector2(1f, 0) * Delta);
            }

            base.Update(gameTime);
            var msState = Mouse.GetState();
            if (msState.RightButton == ButtonState.Pressed || msState.LeftButton == ButtonState.Pressed && msState.Position.X > 0)
            {
                var ms = new
                {
                    LeftButton = msState.LeftButton,
                    RightButton = msState.RightButton,
                    Position = ToWorldCoords(msState.Position.ToVector2()).ToPoint()
                };

                var x = ms.Position.X / RVM.TileSize;
                var y = ms.Position.Y / RVM.TileSize;
                if (x >= 0 && x < RVM.Width && y >= 0 && y < RVM.Height)
                {
                    if (RVM.Mode == "Tile")
                    {
                        var tile = RVM.Tiles.FirstOrDefault(r => r.Position.X == x && r.Position.Y == y);
                        RVM.Select(tile);
                        if (ms.LeftButton == ButtonState.Pressed)
                        {
                            tile.TextureName = RVM.LeftClickTexture;
                        }
                        if (ms.RightButton == ButtonState.Pressed)
                        {
                            tile.TextureName = RVM.RightClickTexture;
                        }
                    }
                    else if (RVM.Mode == "SpawnGroup")
                    {
                        var tile = RVM.Tiles.FirstOrDefault(r => r.Position.X == x && r.Position.Y == y);
                        tile.SpawnGroup = RVM.SpawnGroup;
                    }
                    else if (RVM.Mode == "PlayerStart")
                    {
                        RVM.PlayerStartPosition = new Point(ms.Position.X, ms.Position.Y);
                    }
                    else if (RVM.Mode == "Inanimate")
                    {
                        var ix = (ms.Position.X - (ms.Position.X % RVM.SnapTo)) + (RVM.SnapTo / 2);
                        var iy = (ms.Position.Y - (ms.Position.Y % RVM.SnapTo)) + (RVM.SnapTo / 2);
                        var tar = RVM.Inanimates.FirstOrDefault(m => m.Position.X == ix && m.Position.Y == iy);
                        if (ms.LeftButton == ButtonState.Pressed && tar == null)
                        {
                            RVM.Inanimates.Add(new InanimateTypeViewModel
                            {
                                Type = RVM.InanimateType,
                                Position = new Point(ix, iy),
                            });
                        }
                        if (ms.RightButton == ButtonState.Pressed && tar != null)
                        {                            
                            RVM.Inanimates.Remove(tar);
                        }
                    }
                    else if (RVM.Mode == "Enemy")
                    {
                        var ix = (ms.Position.X - (ms.Position.X % RVM.SnapTo)) + (RVM.SnapTo / 2);
                        var iy = (ms.Position.Y - (ms.Position.Y % RVM.SnapTo)) + (RVM.SnapTo / 2);
                        var tar = RVM.Enemies.FirstOrDefault(m => m.Position.X == ix && m.Position.Y == iy);
                        if (ms.LeftButton == ButtonState.Pressed && tar == null)
                        {
                            RVM.Enemies.Add(new EnemyViewModel
                            {
                                Type = RVM.EnemyType,
                                Position = new Point(ix, iy),
                            });
                        }
                        if (ms.RightButton == ButtonState.Pressed && tar != null)
                        {
                            RVM.Enemies.Remove(tar);
                        }
                    }
                }
            }
        }

        protected override void Draw()
        {
            Editor.graphics.Clear(Color.CornflowerBlue);

            if (textures == null) return;

            var matrix = TransformationMatrix(position);

            Editor.spriteBatch.Begin(transformMatrix: matrix);
            for (var x = 0; x < RVM.Width; x++)
                for (var y = 0; y < RVM.Height; y++)
                {
                    var t = RVM.Tiles.FirstOrDefault(r => r.Position.X == x && r.Position.Y == y);
                    Editor.spriteBatch.Draw(
                        texture: textures[t.TextureName],
                        destinationRectangle: new Rectangle(x * RVM.TileSize, y * RVM.TileSize, RVM.TileSize, RVM.TileSize),
                        color: t.SpawnGroup  == RVM.SpawnGroup && t.SpawnGroup> 0 ? Color.Green :
                            t.SpawnGroup > 0 ? Color.Red : Color.White);
                }

            Editor.spriteBatch.End();


            Editor.spriteBatch.Begin(transformMatrix: matrix);
            foreach (var ina in RVM.Inanimates)
            {
                Editor.spriteBatch.Draw(
                    texture: textures[ina.Type],
                    position: ina.Position.ToVector2(),
                    color: Color.White,
                    origin: new Vector2(RVM.SnapTo, RVM.SnapTo) / 2f
                );
            }
            Editor.spriteBatch.End();


            Editor.spriteBatch.Begin(transformMatrix: matrix);
            foreach (var ina in RVM.Enemies)
            {
                Editor.spriteBatch.Draw(
                    texture: textures["StarEnemy"],
                    position: ina.Position.ToVector2(),
                    color: Color.White,
                    origin: new Vector2(RVM.SnapTo, RVM.SnapTo) / 2f
                );
            }
            Editor.spriteBatch.End();



            Editor.spriteBatch.Begin(transformMatrix: matrix);
            var m = 1;
            Editor.spriteBatch.Draw(
                texture: textures["PlayerPlaceholder"],
                destinationRectangle: new Rectangle((int)(RVM.PlayerStartPosition.X * m),(int)(RVM.PlayerStartPosition.Y * m),
                (int)(128 * m), (int)(192 * m)), color: Color.White);
            Editor.spriteBatch.End();
        }
    }
}
