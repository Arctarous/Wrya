using Avalonia.Controls;
using Avalonia.Platform;
using WebView.Base;
using WebView.Core;

namespace WebView.Avalonia;

public class WebView : NativeControlHost
{
   private IWebViewCore? webViewCore;
   
   protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
   {
      var baseControl = base.CreateNativeControlCore(parent);
      webViewCore = WebViewEnvironment.CreateWebViewCore();
      var handle = webViewCore.InitializeAsync(baseControl.Handle).WaitAsync(TimeSpan.FromSeconds(5)).Result;
      
      webViewCore.Navigate(new Uri("https://www.microsoft.com"));
      return new WebViewPlatformHandle(handle, () => webViewCore.Close());
   }
}