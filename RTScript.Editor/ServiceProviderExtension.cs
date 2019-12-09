using System;

namespace RTScript.Editor
{
    public static class ServiceProviderExtension
    {
        public static T GetService<T>(this IServiceProvider provider)
            => (T)provider.GetService(typeof(T));
    }
}
