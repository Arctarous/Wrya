using WebView.Core;

namespace WebView.Base;

public static class WebViewEnvironment
{
   private static IWebViewEnvironment? current = null;
   private static Func<IntPtr, IWebViewPlatformImpl>? platformImplResolver = null;

   public static IWebViewEnvironment Current => current ??= new WebViewEnvironmentImpl();
   
   internal static void SetCurrent(IWebViewEnvironment? impl)
   {
      current = impl;
   }
   
   internal static IWebViewPlatformImpl? CreatePlatformImpl(IntPtr parentHandle) =>
      platformImplResolver?.Invoke(parentHandle);
   
   public static void SetPlatformImpl(Func<IntPtr, IWebViewPlatformImpl> resolver)
   {
      platformImplResolver = resolver;
   }
   
   public static ICoreWebView CreateCoreWebView(IntPtr parentHandle) =>
      Current.CreateWebViewCore(parentHandle);
}

sealed partial class WebViewEnvironmentImpl : IWebViewEnvironment
{
   public ICoreWebView CreateWebViewCore(IntPtr parentHandle) =>
      new CoreWebView(WebViewEnvironment.CreatePlatformImpl(parentHandle));
}