using LD43.Engine;
using System;
using System.Collections.Generic;

namespace LD43.Gameplay.Scenes
{
    public static class Catalog
    {
        public static Dictionary<Type, Func<object, Scene>> Create()
        {
            return new Dictionary<Type, Func<object, Scene>>
            {
                { typeof(RoomScene), RoomScene.Create }
            };
        }
    }
}
