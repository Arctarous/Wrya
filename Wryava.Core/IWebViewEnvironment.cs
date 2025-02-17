namespace Wryava.Core;

public interface IWebViewEnvironment
{
   ICoreWebView CreateWebViewCore(IntPtr parentHandle);
}