namespace WebView.Core;

public interface ICoreWebView : IWebView
{
   IntPtr Handle { get; }
   
   Task InitializeAsync(WebViewOptions? options = null);

   void Close();
}