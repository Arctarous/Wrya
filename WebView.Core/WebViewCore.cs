using System;
using WebView.Base;

namespace WebView.Core;

public class WebViewCore : IWebViewCore
{
   private IWebViewPlatformImpl? impl;
   
   internal WebViewCore(IWebViewPlatformImpl? impl)
   {
      this.impl = impl;
   }
   
   public Task<IntPtr> InitializeAsync(
      IntPtr parentWindow, WebViewOptions? options = null)
   {
      return impl?.InitializeAsync(parentWindow, options ?? new WebViewOptions()) ??
             Task.FromResult<IntPtr>(IntPtr.Zero);
   }


   public void Close() => impl?.Close();


   public bool GoBack() => impl?.GoBack() ?? false;


   public bool GoForward() => impl?.GoForward() ?? false;


   public void Reload() => impl?.Reload();


   public void StopLoading() => impl?.StopLoading();


   public void Navigate(Uri? uri) => impl?.Navigate(uri);


   public void LoadHtml(string htmlContent) => impl?.LoadHtml(htmlContent);
}