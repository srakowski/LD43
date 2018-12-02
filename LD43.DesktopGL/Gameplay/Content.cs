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
            void Room(string name) => _assetCatalog[name] = JsonConvert.DeserializeObject<Models.RoomConfig>(File.ReadAllText(Path.Combine(content.RootDirectory, $"{name}.json")));

            Texture("PlayerPlaceholder");
            Texture("PlatformPlaceholder");
            Texture("Tile_BG");
            Texture("Tile_FG");
            Texture("Vase");

            Room("Rooms/room");

            Font("GenericFont");
        }
    }
}
