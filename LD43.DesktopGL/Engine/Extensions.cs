using System;

namespace LD43.Engine
{
    public static class Extensions
    {
        public static T GetService<T>(this IServiceProvider self) => (T)self.GetService(typeof(T));
    }
}
