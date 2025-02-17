using System;
using Wryava.Core;

namespace Wryava.Core;

public interface IWebViewPlatformImpl : IDisposable
{
   IntPtr? Handle { get; }
   
   Task InitializeAsync(WebViewOptions options);
   
   void Close();
   
   bool GoBack();
   
   bool GoForward();
   
   void Reload();
   
   void StopLoading();
   
   void Navigate(Uri? uri);
   
   void LoadHtml(string htmlContent);

   Task<string?> ExecuteScriptAsync(string script);
}
