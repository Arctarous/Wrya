using WebView.Core;

namespace WebView.Base;

public static class WebViewEnvironment
{
   private static IWebViewEnvironment? current = null;
   private static Func<IWebViewPlatformImpl>? platformImplResolver = null;

   public static IWebViewEnvironment Current => current ??= new WebViewEnvironmentImpl();
   
   internal static void SetCurrent(IWebViewEnvironment? impl)
   {
      current = impl;
   }
   
   internal static IWebViewPlatformImpl? CreatePlatformImpl() =>
      platformImplResolver?.Invoke();
   
   public static void SetPlatformImpl(Func<IWebViewPlatformImpl> resolver)
   {
      platformImplResolver = resolver;
   }
   
   public static IWebViewCore CreateWebViewCore() =>
      Current.CreateWebViewCore();
}

sealed partial class WebViewEnvironmentImpl : IWebViewEnvironment
{
   public IWebViewCore CreateWebViewCore() =>
      new WebViewCore(WebViewEnvironment.CreatePlatformImpl());
}