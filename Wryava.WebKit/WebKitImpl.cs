using System;
using System.Globalization;
using ObjCRuntime;
using WebKit;
using Wryava.Core;

namespace Wryava.WebKit;

public class WebKitImpl : IWebViewPlatformImpl
{
   private const string ScriptMessageHandlerName = "webwindowinterop";

   private readonly WKWebView _wkWebView;
   
   public Uri? Url => _wkWebView?.Url;
   
   public WebKitImpl(IntPtr parentHandle)
   {
      _wkWebView = new(CGRect.Empty, new WKWebViewConfiguration())
      {
         NavigationDelegate = new NavigationDelegate(this),
      };
   }
   
   ~WebKitImpl()
   {
      Close();
   }
   
   public IntPtr? Handle => _wkWebView.Handle;


   public Task InitializeAsync(WebViewOptions options)
   {
      var config = _wkWebView.Configuration;

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

      // config.UserContentController.AddScriptMessageHandler(new WebViewScriptMessageHandler(this), ScriptMessageHandlerName);
      // iOS WKWebView doesn't allow handling 'http'/'https' schemes, so we use the fake 'app' scheme
      // config.SetUrlSchemeHandler(new SchemeHandler(this), urlScheme: "app");

      if (options.DeveloperToolsEnabled)
      {
         config.Preferences.SetValueForKey(NSObject.FromObject(true), new NSString("developerExtrasEnabled"));
         if (OperatingSystem.IsIOSVersionAtLeast(16, 4) ||
             OperatingSystem.IsMacCatalystVersionAtLeast(16, 6))
         {
            // Enable Developer Extras for iOS builds for 16.4+ and Mac Catalyst builds for 16.6 (macOS 13.5)+
            _wkWebView.SetValueForKey(NSObject.FromObject(true), new NSString("inspectable"));
         }
      }
      
      return Task.CompletedTask;
   }

   public void Close()
   {
      _wkWebView.Dispose();
   }

   public void Dispose()
   {
      Close();
      GC.SuppressFinalize(this);
   }


   public bool GoBack()
   {
      if (_wkWebView.CanGoBack)
      {
         return _wkWebView.GoBack() != null;
      }
      return false;
   }


   public bool GoForward()
   {
      if (_wkWebView.CanGoForward)
      {
         return _wkWebView.GoForward() != null;
      }
      return false;
   }
   
   public void Reload() => _wkWebView.Reload();
   
   public void StopLoading() => _wkWebView.StopLoading();
   
   public void Navigate(Uri? uri)
   {
      if (uri is null)
      {
         return;
      }

      using var nsUrl = new NSUrl(uri.AbsoluteUri);
      using var request = new NSUrlRequest(nsUrl);
      _wkWebView.LoadRequest(request);
   }
   
   public void LoadHtml(string htmlContent)
   {
      _wkWebView.LoadHtmlString(htmlContent, null!);
   }
   
   public async Task<string?> ExecuteScriptAsync(string script)
   {
      var javascript = string.Format(CultureInfo.InvariantCulture, "javascript:{0}", script);
      var result = await _wkWebView.EvaluateJavaScriptAsync(javascript);
      return result?.ToString();
   }
   
   // public void EvaluateJavaScript(EvaluateJavaScriptAsyncRequest request)
   // {
   //    request.RunAndReport(ExecuteScriptAsync(request.Script));
   // }
}