using WebKit;

namespace WebView.WebKit;

internal class WebViewScriptMessageHandler : NSObject, IWKScriptMessageHandler
{
   private readonly WeakReference<WebKitImpl?> _webView;

   public WebViewScriptMessageHandler(WebKitImpl webViewHandler)
   {
      _webView = new(webViewHandler);
   }
   
   public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
   {
      ArgumentNullException.ThrowIfNull(message);
      //_webView?.MessageReceived(AppOriginUri, ((NSString)message.Body).ToString());
   }
}