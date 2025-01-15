using System;
using WebView.Core;

namespace WebView.Base;

public interface IWebViewPlatformImpl : IDisposable
{
   Task<IntPtr> InitializeAsync(IntPtr parentWindow, WebViewOptions options);
   
   void Close();
   
   bool GoBack();
   
   bool GoForward();
   
   void Reload();
   
   void StopLoading();
   
   void Navigate(Uri? uri);
   
   void LoadHtml(string htmlContent);
}
