namespace WebView.Core;

public interface IWebViewEnvironment
{
   ICoreWebView CreateWebViewCore(IntPtr parentHandle);
}