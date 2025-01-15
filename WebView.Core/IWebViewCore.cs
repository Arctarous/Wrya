namespace WebView.Core;

public interface IWebViewCore
{
   Task<IntPtr> InitializeAsync(IntPtr parentWindow, WebViewOptions? options = null);
   
   void Close();
   
   bool GoBack();
   
   bool GoForward();
   
   void Reload();
   
   void StopLoading();
   
   void Navigate(Uri? uri);
   
   void LoadHtml(string htmlContent);
}