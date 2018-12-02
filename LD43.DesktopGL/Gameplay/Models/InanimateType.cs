using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace LD43.Gameplay.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InanimateType
    {
        Vase,
    }

    public static class InanimateTypeEx
    {
        public static T Match<T>(this InanimateType self, Func<T> vase) =>
            self == InanimateType.Vase ? vase()
            : default(T);
    }
}