using Avalonia;
using Wryava.Core;
using Wryava.WebKit;

namespace Wryava.Avalonia;

public static partial class WebViewExtensions
{
   static partial void UseWebViewImpl()
   {
      WebViewEnvironment.SetPlatformImpl((parentHandle) => new WebKitImpl(parentHandle));
   }
}