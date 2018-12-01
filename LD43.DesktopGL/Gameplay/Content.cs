using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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

            Texture("PlayerPlaceholder");
        }
    }
}
