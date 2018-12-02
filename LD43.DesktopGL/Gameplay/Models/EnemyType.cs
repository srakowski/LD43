using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace LD43.Gameplay.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnemyType
    {
        StarEnemy,
    }

    public static class EnemyTypeEx
    {
        public static T Match<T>(this EnemyType self, Func<T> star) =>
            self == EnemyType.StarEnemy ? star()
            : default(T);
    }
}