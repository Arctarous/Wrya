using Avalonia;
using WebView.Base;

namespace WebView.Avalonia;

public static partial class WebViewExtensions
{
   public static AppBuilder UseWebView(this AppBuilder appBuilder)
   {
      UseWebViewImpl();
      return appBuilder;
   }
   
   static partial void UseWebViewImpl();
}