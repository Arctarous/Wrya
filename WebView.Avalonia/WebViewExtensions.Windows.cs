using Avalonia;
using WebView.Base;
using WebView.WebView2;

namespace WebView.Avalonia;

public static partial class WebViewExtensions
{
   static partial void UseWebViewImpl()
   {
      WebViewEnvironment.SetPlatformImpl(parentHandle => new WebView2Impl(parentHandle));
   }
}