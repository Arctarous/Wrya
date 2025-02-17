using System.Globalization;
using System.Web;
using WebKit;

namespace Wryava.WebKit;

internal class SchemeHandler : NSObject, IWKUrlSchemeHandler
{
   private readonly WeakReference<WebKitImpl?> _webViewHandler;

			public SchemeHandler(WebKitImpl webViewHandler)
			{
				_webViewHandler = new(webViewHandler);
			}

			private WebKitImpl? Handler => _webViewHandler is not null && _webViewHandler.TryGetTarget(out var h) ? h : null;

			[Export("webView:startURLSchemeTask:")]
			public void StartUrlSchemeTask(WKWebView webView, IWKUrlSchemeTask urlSchemeTask)
			{
				// var url = urlSchemeTask.Request.Url?.AbsoluteString ?? "";
				//
				// var responseData = GetResponseBytes(url);
				//
				// if (responseData.StatusCode == 200)
				// {
				// 	using (var dic = new NSMutableDictionary<NSString, NSString>())
				// 	{
				// 		dic.Add((NSString)"Content-Length", (NSString)(responseData.ResponseBytes.Length.ToString(CultureInfo.InvariantCulture)));
				// 		dic.Add((NSString)"Content-Type", (NSString)responseData.ContentType);
				// 		// Disable local caching. This will prevent user scripts from executing correctly.
				// 		dic.Add((NSString)"Cache-Control", (NSString)"no-cache, max-age=0, must-revalidate, no-store");
				// 		if (urlSchemeTask.Request.Url != null)
				// 		{
				// 			using var response = new NSHttpUrlResponse(urlSchemeTask.Request.Url, responseData.StatusCode, "HTTP/1.1", dic);
				// 			urlSchemeTask.DidReceiveResponse(response);
				// 		}
				// 	}
				//
				// 	urlSchemeTask.DidReceiveData(NSData.FromArray(responseData.ResponseBytes));
				// 	urlSchemeTask.DidFinish();
				// }
			}

			// private (byte[] ResponseBytes, string ContentType, int StatusCode) GetResponseBytes(string? url)
			// {
			// 	if (Handler is null)
			// 	{
			// 		return (Array.Empty<byte>(), ContentType: string.Empty, StatusCode: 404);
			// 	}
			//
			// 	var fullUrl = url;
			// 	url = HybridWebViewQueryStringHelper.RemovePossibleQueryString(url);
			//
			// 	if (new Uri(url) is Uri uri && AppOriginUri.IsBaseOf(uri))
			// 	{
			// 		var relativePath = AppOriginUri.MakeRelativeUri(uri).ToString().Replace('\\', '/');
			//
			// 		var bundleRootDir = Path.Combine(NSBundle.MainBundle.ResourcePath, Handler.HybridRoot!);
			//
			// 		// 1. Try special InvokeDotNet path
			// 		if (relativePath == InvokeDotNetPath)
			// 		{
			// 			var fullUri = new Uri(fullUrl!);
			// 			var invokeQueryString = HttpUtility.ParseQueryString(fullUri.Query);
			// 			(var contentBytes, var bytesContentType) = Handler.InvokeDotNet(invokeQueryString);
			// 			if (contentBytes is not null)
			// 			{
			// 				return (contentBytes, bytesContentType!, StatusCode: 200);
			// 			}
			// 		}
			//
			// 		string contentType;
			//
			// 		// 2. If nothing found yet, try to get static content from the asset path
			// 		if (string.IsNullOrEmpty(relativePath))
			// 		{
			// 			//relativePath = Handler.DefaultFile!.Replace('\\', '/');
			// 			contentType = "text/html";
			// 		}
			// 		else
			// 		{
			// 			if (!ContentTypeProvider.TryGetContentType(relativePath, out contentType!))
			// 			{
			// 				// TODO: Log this
			// 				contentType = "text/plain";
			// 			}
			// 		}
			//
			// 		var assetPath = Path.Combine(bundleRootDir, relativePath);
			//
			// 		if (File.Exists(assetPath))
			// 		{
			// 			return (File.ReadAllBytes(assetPath), contentType, StatusCode: 200);
			// 		}
			// 	}
			//
			// 	return (Array.Empty<byte>(), ContentType: string.Empty, StatusCode: 404);
			// }

			[Export("webView:stopURLSchemeTask:")]
			public void StopUrlSchemeTask(WKWebView webView, IWKUrlSchemeTask urlSchemeTask)
			{
			}
}