namespace PM.Api.IoC;

public static class IoCHelper
{
    public static IServiceProvider _serviceProvider;

    public static void SetServiceProvider(IServiceProvider serviceCollection)
    {
        _serviceProvider = serviceCollection;
    }

    public static T Resolve<T>()
    {
        return _serviceProvider.GetService<T>();
    }

}

