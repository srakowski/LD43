using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Newtonsoft.Json;

namespace LD43.Gameplay
{
    public static class Content
    {
        private static Dictionary<string, object> _assetCatalog = new Dictionary<string, object>();

        public static Dictionary<string, object> GetAssetCatalog() => _assetCatalog;

        public static void Load(ContentManager content)
        {
            void LoadAsset<T>(string name) => _assetCatalog[name] = content.Load<T>(name);
            void Texture(string name) => LoadAsset<Texture2D>(name);
            void Font(string name) => LoadAsset<SpriteFont>(name);

            Texture("PlayerPlaceholder");
            Texture("PlatformPlaceholder");
            Texture("Tile_BG");
            Texture("Tile_FG");
            Texture("Tile_PF");
            Texture("Vase");
            Texture("GoldDrop");
            Texture("StarEnemy");
            Texture("StarEnemy_Shot");
            Texture("SoulDrop");
            Texture("SacrificialFirePit");

            Font("GenericFont");
        }
    }
}
