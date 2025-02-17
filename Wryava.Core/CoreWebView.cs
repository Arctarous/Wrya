
namespace Wryava.Core;

internal class CoreWebView : ICoreWebView
{
   private IWebViewPlatformImpl? impl;
   
   internal CoreWebView(IWebViewPlatformImpl? impl)
   {
      this.impl = impl;
   }
   
   public IntPtr Handle => impl?.Handle ?? IntPtr.Zero;
   
   public Task InitializeAsync(WebViewOptions? options = null)
   {
      return impl?.InitializeAsync(options ?? new WebViewOptions()) ??
             Task.CompletedTask;
   }
   
   public void Close() => impl?.Close();
   
   public bool GoBack() => impl?.GoBack() ?? false;
   
   public bool GoForward() => impl?.GoForward() ?? false;
   
   public void Reload() => impl?.Reload();
   
   public void StopLoading() => impl?.StopLoading();
   
   public void Navigate(Uri? uri) => impl?.Navigate(uri);
   
   public void LoadHtml(string htmlContent) => impl?.LoadHtml(htmlContent);
   
   public Task<string?> ExecuteScriptAsync(string script) => 
      impl?.ExecuteScriptAsync(script) ??
      Task.FromException<string?>(new InvalidOperationException());
}