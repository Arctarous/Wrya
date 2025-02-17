using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Platform;
using Wryava.Core;

namespace Wryava.Avalonia;

public class WebView : NativeControlHost
{
   private ICoreWebView? _coreWebView;
   
   public static readonly DirectProperty<WebView, IWebView?> ControllerProperty =
      AvaloniaProperty.RegisterDirect<WebView, IWebView?>(
         nameof(Controller),
         o => o.Controller,
         defaultBindingMode: BindingMode.OneWayToSource);
   
   public IWebView? Controller => _coreWebView;
   
   protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
   {
      var baseControl = base.CreateNativeControlCore(parent);
      _coreWebView = WebViewEnvironment.CreateCoreWebView(parent.Handle);

      RaisePropertyChanged(ControllerProperty, null,_coreWebView);
      
      SynchronizationContext.Current?.Post(async void (state) =>
      {
         await _coreWebView.InitializeAsync();
         //_coreWebView.Navigate(new Uri("https://www.microsoft.com"));
         _coreWebView.Navigate(new Uri("https://www.bilibili.com"));
      }, null);
      
      return new WebViewPlatformHandle(_coreWebView.Handle, () => _coreWebView.Close());
   }
}