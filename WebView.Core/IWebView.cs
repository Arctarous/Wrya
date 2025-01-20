namespace WebView.Core;

public interface IWebView
{
   bool GoBack();
   
   bool GoForward();
   
   void Reload();
   
   void StopLoading();
   
   void Navigate(Uri? uri);
   
   void LoadHtml(string htmlContent);
   
   Task<string> ExecuteScriptAsync(string script);
}