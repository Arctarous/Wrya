using Avalonia;

namespace Wryava.Avalonia;

public static partial class WebViewExtensions
{
   public static AppBuilder UseWebView(this AppBuilder appBuilder)
   {
      UseWebViewImpl();
      return appBuilder;
   }
   
   static partial void UseWebViewImpl();
}