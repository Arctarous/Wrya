using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace WebView.WebView2.Interop;

static class NativeMethods
{
   [DllImport("user32.dll", EntryPoint="SetWindowLongPtr", SetLastError=true)]
   private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, WINDOW_LONG_PTR_INDEX nIndex, IntPtr dwNewLong);

   public static IntPtr SetWindowLongPtr(HWND hwnd, WINDOW_LONG_PTR_INDEX nIndex, IntPtr dwNewLong)
   {
      return IntPtr.Size == 4 ? PInvoke.SetWindowLong(hwnd, nIndex, (int)dwNewLong) : SetWindowLongPtr64(hwnd, nIndex, dwNewLong);
   }
}