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

        protected override void Update(GameTime gameTime)
        {
            if (textures == null)
            {
                InitTextures();
            }

            base.Update(gameTime);
            var ms = Mouse.GetState();
            if (ms.RightButton == ButtonState.Pressed || ms.LeftButton == ButtonState.Pressed && ms.Position.X > 0)
            {
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
                    else if (RVM.Mode == "PlayerStart")
                    {
                        RVM.PlayerStartPosition = new Point(ms.Position.X, ms.Position.Y);
                    }
                    else if (RVM.Mode == "Inanimate")
                    {
                        if (ms.LeftButton == ButtonState.Pressed)
                        {
                            var ix = (ms.Position.X - (ms.Position.X % RVM.SnapInanimateTo)) + (RVM.SnapInanimateTo / 2);
                            var iy = (ms.Position.Y - (ms.Position.Y % RVM.SnapInanimateTo)) + (RVM.SnapInanimateTo / 2);
                            RVM.Inanimates.Add(new InanimateTypeViewModel
                            {
                                Type = RVM.InanimateType,
                                Position = new Point(ix, iy),
                            });
                        }
                        if (ms.RightButton == ButtonState.Pressed)
                        {
                            var ix = (ms.Position.X - (ms.Position.X % RVM.SnapInanimateTo)) + (RVM.SnapInanimateTo / 2);
                            var iy = (ms.Position.Y - (ms.Position.Y % RVM.SnapInanimateTo)) + (RVM.SnapInanimateTo / 2);
                            var tar = RVM.Inanimates.FirstOrDefault(m => m.Position.X == ix && m.Position.Y == iy);
                            if (tar != null)
                            {
                                RVM.Inanimates.Remove(tar);
                            }
                        }
                    }
                }
            }
        }

        protected override void Draw()
        {
            if (textures == null) return;

            Editor.spriteBatch.Begin();
            for (var x = 0; x < RVM.Width; x++)
                for (var y = 0; y < RVM.Height; y++)
                {
                    var t = RVM.Tiles.FirstOrDefault(r => r.Position.X == x && r.Position.Y == y);
                    Editor.spriteBatch.Draw(
                        texture: textures[t.TextureName],
                        destinationRectangle: new Rectangle(x * RVM.TileSize, y * RVM.TileSize, RVM.TileSize, RVM.TileSize),
                        color: Color.White);
                }

            Editor.spriteBatch.End();


            Editor.spriteBatch.Begin();
            foreach (var ina in RVM.Inanimates)
            {
                Editor.spriteBatch.Draw(
                    texture: textures["Vase"],
                    position: ina.Position.ToVector2(),
                    color: Color.White,
                    origin: new Vector2(RVM.SnapInanimateTo, RVM.SnapInanimateTo) / 2f
                );
            }
            Editor.spriteBatch.End();



            Editor.spriteBatch.Begin();
            var m = 1;
            Editor.spriteBatch.Draw(
                texture: textures["PlayerPlaceholder"],
                destinationRectangle: new Rectangle((int)(RVM.PlayerStartPosition.X * m),(int)(RVM.PlayerStartPosition.Y * m),
                (int)(128 * m), (int)(192 * m)), color: Color.White);
            Editor.spriteBatch.End();
        }
    }
}
