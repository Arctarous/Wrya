using WebKit;

namespace WebView.WebKit;

public class NavigationDelegate : WKNavigationDelegate
{
   private readonly WeakReference<WebKitImpl> _webView;
   //private WebNavigationEvent _lastEvent;


   public NavigationDelegate(WebKitImpl handler)
   {
      _ = handler ?? throw new ArgumentNullException(nameof(handler));
      _webView = new WeakReference<WebKitImpl>(handler);
   }


   [Export("webView:didFinishNavigation:")]
   public void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
   {
      // var handler = Handler;
      //
      // if ( handler is null || !handler.IsConnected() )
      //    return;
      //
      // var platformView = handler?.PlatformView;
      // var virtualView = handler?.VirtualView;
      //
      // if ( platformView is null || virtualView is null )
      //    return;
      //
      // platformView.UpdateCanGoBackForward(virtualView);
      //
      // if ( webView.IsLoading )
      //    return;
      //
      // var url = GetCurrentUrl();
      //
      // if ( url == $"file://{NSBundle.MainBundle.BundlePath}/" )
      //    return;

      // virtualView.Navigated(_lastEvent, url, WebNavigationResult.Success);
      //
      // // ProcessNavigatedAsync calls UpdateCanGoBackForward
      // if ( handler is WebViewHandler webViewHandler )
      //    webViewHandler.ProcessNavigatedAsync(url).FireAndForget();
      // else
      //    platformView.UpdateCanGoBackForward(virtualView);
   }


   [Export("webView:didFailNavigation:withError:")]
   public void DidFailNavigation(WKWebView webView, WKNavigation navigation, NSError error)
   {
      // var handler = Handler;
      //
      // if ( handler is null || !handler.IsConnected() )
      //    return;
      //
      // var platformView = handler?.PlatformView;
      // var virtualView = handler?.VirtualView;
      //
      // if ( platformView is null || virtualView is null )
      //    return;
      //
      // var url = GetCurrentUrl();
      //
      // virtualView.Navigated(_lastEvent, url, WebNavigationResult.Failure);
      //
      // platformView.UpdateCanGoBackForward(virtualView);
   }


   [Export("webView:didFailProvisionalNavigation:withError:")]
   public void DidFailProvisionalNavigation(WKWebView webView, WKNavigation navigation,
      NSError error)
   {
      // var handler = Handler;
      //
      // if ( handler is null || !handler.IsConnected() )
      //    return;
      //
      // var platformView = handler?.PlatformView;
      // var virtualView = handler?.VirtualView;
      //
      // if ( platformView is null || virtualView is null )
      //    return;
      //
      // var url = GetCurrentUrl();
      //
      // virtualView.Navigated(_lastEvent, url, WebNavigationResult.Failure);
      //
      // platformView.UpdateCanGoBackForward(virtualView);
   }


   // https://stackoverflow.com/questions/37509990/migrating-from-uiwebview-to-wkwebview
   [Export("webView:decidePolicyForNavigationAction:decisionHandler:")]
   public void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction,
      Action<WKNavigationActionPolicy> decisionHandler)
   {
      // var handler = Handler;
      //
      // if ( handler is null || !handler.IsConnected() )
      // {
      //    decisionHandler.Invoke(WKNavigationActionPolicy.Cancel);
      //    return;
      // }
      //
      // var platformView = handler?.PlatformView;
      // var virtualView = handler?.VirtualView;
      //
      // if ( platformView is null || virtualView is null )
      // {
      //    decisionHandler.Invoke(WKNavigationActionPolicy.Cancel);
      //    return;
      // }

      // var navEvent = WebNavigationEvent.NewPage;
      // var navigationType = navigationAction.NavigationType;
      //
      // switch ( navigationType )
      // {
      //    case WKNavigationType.LinkActivated:
      //       navEvent = WebNavigationEvent.NewPage;
      //
      //       if ( navigationAction.TargetFrame == null )
      //          webView?.LoadRequest(navigationAction.Request);
      //
      //       break;
      //    case WKNavigationType.FormSubmitted:
      //       navEvent = WebNavigationEvent.NewPage;
      //       break;
      //    case WKNavigationType.BackForward:
      //       navEvent = CurrentNavigationEvent;
      //       break;
      //    case WKNavigationType.Reload:
      //       navEvent = WebNavigationEvent.Refresh;
      //       break;
      //    case WKNavigationType.FormResubmitted:
      //       navEvent = WebNavigationEvent.NewPage;
      //       break;
      //    case WKNavigationType.Other:
      //       navEvent = WebNavigationEvent.NewPage;
      //       break;
      // }
      //
      // _lastEvent = navEvent;

      var request = navigationAction.Request;
      var lastUrl = request.Url.ToString();

      // bool cancel = virtualView.Navigating(navEvent, lastUrl);
      // platformView.UpdateCanGoBackForward(virtualView);
      // decisionHandler(cancel ? WKNavigationActionPolicy.Cancel : WKNavigationActionPolicy.Allow);
   }


   private string GetCurrentUrl()
   {
      return Handler?.Url?.AbsoluteUri ?? string.Empty;
   }


   //internal WebNavigationEvent CurrentNavigationEvent { get; set; }

   private WebKitImpl? Handler
   {
      get
      {
         if ( _webView.TryGetTarget(out var handler) ) return handler;

         return null;
      }
   }
}