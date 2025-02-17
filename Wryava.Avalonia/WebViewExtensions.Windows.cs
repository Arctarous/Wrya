using Avalonia;
using Wryava.Core;
using Wryava.WebView2;

namespace Wryava.Avalonia;

public static partial class WebViewExtensions
{
   static partial void UseWebViewImpl()
   {
      WebViewEnvironment.SetPlatformImpl(parentHandle => new WebView2Impl(parentHandle));
   }
}