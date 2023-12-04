namespace ChatAppMobile
{
    public class ServiceHelper
    {
        [Obsolete]
        public static TService GetService<TService>() => Current.GetService<TService>()!;

        [Obsolete]
        public static IServiceProvider Current =>
#if WINDOWS10_0_17763_0_OR_GREATER
        MauiWinUIApplication.Current.Services;
#elif __ANDROID__
        MauiApplication.Current.Services;

#elif IOS || MACCATALYST
        MauiUIApplicationDelegate.Current.Services;
#else
        null;
#endif
    }
}

