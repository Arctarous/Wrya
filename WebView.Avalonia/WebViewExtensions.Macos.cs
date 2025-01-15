using Avalonia;
using WebView.Base;
using WebView.WebKit;

namespace WebView.Avalonia;

public static partial class WebViewExtensions
{
   static partial void UseWebViewImpl()
   {
      WebViewEnvironment.SetPlatformImpl(() => new WebKitImpl());
   }
}