using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD43.Gameplay
{
    public class AudioPlayer
    {
        private static Dictionary<string, object> _assetCatalog;

        public static void PlaySfx(string name)
        {
            if (_assetCatalog == null) return;
            (_assetCatalog[name] as SoundEffect).Play();
        }

        public static bool playing = false;

        public static void PlaySong()
        {
            if (playing) return;
            playing = true;

            var s = (_assetCatalog["abeat"] as Song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.Play(s);
        }

        internal static void Load(Dictionary<string, object> assetCatalog)
        {
            _assetCatalog = assetCatalog;

        }
    }
}
