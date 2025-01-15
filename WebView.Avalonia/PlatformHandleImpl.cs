using Avalonia.Controls.Platform;
using Avalonia.Platform;

namespace WebView.Avalonia;

internal sealed class WebViewPlatformHandle(IntPtr handle, Action? destroyHandler = null)
   : PlatformHandle(handle, PlatformHandleHelper.Descriptor),
      INativeControlHostDestroyableControlHandle
{
   public void Destroy()
   {
      destroyHandler?.Invoke();
   }
}

static class PlatformHandleHelper
{
   public static string Descriptor =>
#if WINDOWS
      "HWND";
#elif MACOS
      "NSView";
#elif LINUX
      "Xid";
#else
      "UNKNOWN";
#endif
}
