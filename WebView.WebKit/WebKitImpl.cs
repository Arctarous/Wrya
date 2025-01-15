using System;
using ObjCRuntime;
using WebView.Base;
using WebKit;
using WebView.Core;

namespace WebView.WebKit;

public class WebKitImpl : IWebViewPlatformImpl
{
   private const string ScriptMessageHandlerName = "webwindowinterop";

   private WKWebView? wkWebView;
   
   public Uri? Url => wkWebView?.Url;
   
   ~WebKitImpl()
   {
      Close();
   }
   
   public Task<IntPtr> InitializeAsync(
      IntPtr parentWindow,
      WebViewOptions options)
   {
      var config = new WKWebViewConfiguration()
      {
         WebsiteDataStore = WKWebsiteDataStore.DefaultDataStore,
      };

      // By default, setting inline media playback to allowed, including autoplay
      // and picture in picture, since these things MUST be set during the webview
      // creation, and have no effect if set afterwards.
      // A custom handler factory delegate could be set to disable these defaults
      // but if we do not set them here, they cannot be changed once the
      // handler's platform view is created, so erring on the side of wanting this
      // capability by default.
      if (OperatingSystem.IsMacCatalystVersionAtLeast(10) || OperatingSystem.IsIOSVersionAtLeast(10))
      {
         // config.AllowsPictureInPictureMediaPlayback = true;
         // config.AllowsInlineMediaPlayback = true;
         config.MediaTypesRequiringUserActionForPlayback = WKAudiovisualMediaTypes.None;
      }

      config.DefaultWebpagePreferences!.AllowsContentJavaScript = true;

      config.UserContentController.AddScriptMessageHandler(new WebViewScriptMessageHandler(this), ScriptMessageHandlerName);
      // iOS WKWebView doesn't allow handling 'http'/'https' schemes, so we use the fake 'app' scheme
      config.SetUrlSchemeHandler(new SchemeHandler(this), urlScheme: "app");

      wkWebView = new(CGRect.Empty, config)
      {
         NavigationDelegate = new WKNavigationDelegate(),
         //AutoresizesSubviews = true,
      };

      if (options.DeveloperToolsEnabled)
      {
         config.Preferences.SetValueForKey(NSObject.FromObject(true), new NSString("developerExtrasEnabled"));
         if (OperatingSystem.IsIOSVersionAtLeast(16, 4) || OperatingSystem.IsMacCatalystVersionAtLeast(16, 6))
         {
            // Enable Developer Extras for iOS builds for 16.4+ and Mac Catalyst builds for 16.6 (macOS 13.5)+
            wkWebView.SetValueForKey(NSObject.FromObject(true), new NSString("inspectable"));
         }
      }
      
      return Task.FromResult(wkWebView.Handle.Handle);
   }

   public void Close()
   {
      wkWebView?.Dispose();
      wkWebView = null;
   }

   public void Dispose()
   {
      Close();
      GC.SuppressFinalize(this);
   }
   
   public bool GoBack() => wkWebView?.CanGoBack ?? false;
   
   public bool GoForward() => wkWebView?.CanGoForward ?? false;
   
   public void Reload() => wkWebView?.Reload();
   
   public void StopLoading() => wkWebView?.StopLoading();
   
   public void Navigate(Uri? uri)
   {
      if (uri is null)
      {
         return;
      }

      using var nsUrl = new NSUrl(uri.AbsoluteUri);
      using var request = new NSUrlRequest(nsUrl);
      wkWebView?.LoadRequest(request);
   }
   
   public void LoadHtml(string htmlContent)
   {
      wkWebView?.LoadHtmlString(htmlContent, default!);
   }
   
   async Task<string> EvaluateJavaScript(string script)
   {
      NSObject? result = null;
      if ( wkWebView != null )
      {
         result = await wkWebView.EvaluateJavaScriptAsync(script);
      }
      return result?.ToString() ?? "null";
   }
   
   public async void EvaluateJavaScript(EvaluateJavaScriptAsyncRequest request)
   {
      request.RunAndReport(EvaluateJavaScript(request.Script));
   }
}