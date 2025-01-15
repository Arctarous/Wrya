using Microsoft.Web.WebView2.Core;
using WebView.Base;
using WebView.Core;

namespace WebView.WebView2;

public class WebView2Impl : IWebViewPlatformImpl
{
   private CoreWebView2Controller? webView = null;
   
   ~WebView2Impl()
   {
      Close();
   }

   public async Task<IntPtr> InitializeAsync(
      IntPtr parentWindow,
      WebViewOptions options)
   {
      var env = await CoreWebView2Environment.CreateAsync();
      webView = await env.CreateCoreWebView2ControllerAsync(parentWindow);
      
      return parentWindow;
   }
   
   public void Close()
   {
      webView?.Close();
      webView = null;
   }


   public bool GoBack()
   {
      throw new NotImplementedException();
   }


   public bool GoForward()
   {
      throw new NotImplementedException();
   }


   public void Reload()
   {
      throw new NotImplementedException();
   }


   public void StopLoading()
   {
      throw new NotImplementedException();
   }


   public void Navigate(Uri? uri)
   {
      throw new NotImplementedException();
   }


   public void LoadHtml(string htmlContent)
   {
      throw new NotImplementedException();
   }


   public void Dispose()
   {
      Close();
      GC.SuppressFinalize(this);
   }
}