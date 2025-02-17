
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Microsoft.Web.WebView2.Core;
using Wryava.Core;

namespace Wryava.WebView2;

public class WebView2Impl : IWebViewPlatformImpl
{
   private readonly WebViewHostWindow _hostWindow;
   private CoreWebView2Controller? _webViewController = null;

   public IntPtr? Handle => _hostWindow?.Hwnd?.Value;
   
   private CoreWebView2? CoreWebView2 => _webViewController?.CoreWebView2;

   public WebView2Impl(IntPtr parentHandle)
   {
      _hostWindow = WebViewHostWindow.Create(
         0, 
         "WebView2HostWindow",
         WINDOW_STYLE.WS_CHILD | WINDOW_STYLE.WS_VISIBLE | WINDOW_STYLE.WS_CLIPCHILDREN | WINDOW_STYLE.WS_CLIPSIBLINGS,
         0, 0, 600, 600,
         new HWND(parentHandle))!;

      _hostWindow.WindowPosChanged += OnHostWindowPosChanged;

      _hostWindow.Closed += OnHostWindowClosed;
   }


   private void OnHostWindowClosed(object? sender, EventArgs e)
   {
      _hostWindow.WindowPosChanged -= OnHostWindowPosChanged;
   }
   
   private void OnHostWindowPosChanged(object? sender, EventArgs e)
   {
      if ( _webViewController != null )
      {
         _webViewController.Bounds = _hostWindow.ClientRect;
      }
   }
   
   ~WebView2Impl()
   {
      Close();
   }

   public async Task InitializeAsync(WebViewOptions options)
   {
      var handle = _hostWindow.Hwnd?.Value ?? IntPtr.Zero;
      var env = await CoreWebView2Environment.CreateAsync();
      _webViewController = await env.CreateCoreWebView2ControllerAsync(handle);
      _webViewController.BoundsMode = CoreWebView2BoundsMode.UseRawPixels;
      _webViewController.Bounds = _hostWindow.ClientRect;
   }
   
   public void Close()
   {
      _webViewController?.Close();
      _webViewController = null;

      if (_hostWindow.IsWindow)
      {
         _hostWindow.Close();
      }
   }


   public bool GoBack()
   {
      switch (CoreWebView2?.CanGoBack)
      {
         case true:
            CoreWebView2.GoBack();
            return true;
         default:
            return false;
      }
   }
   
   public bool GoForward()
   {
      switch (CoreWebView2?.CanGoForward)
      {
         case true:
            CoreWebView2.GoForward();
            return true;
         default:
            return false;
      }
   }

   public void Reload() => CoreWebView2?.Reload();
   
   public void StopLoading() => CoreWebView2?.Stop();

   public void Navigate(Uri? uri) => CoreWebView2?.Navigate(uri?.AbsoluteUri);

   public void LoadHtml(string htmlContent) => CoreWebView2?.Navigate(htmlContent);
   
   public Task<string?> ExecuteScriptAsync(string script) =>
      CoreWebView2?.ExecuteScriptAsync(script) ??
      Task.FromException<string?>(new InvalidOperationException());
   
   public void Dispose()
   {
      Close();
      GC.SuppressFinalize(this);
   }
}