using System.Diagnostics;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;
using static Windows.Win32.PInvoke;

namespace WebView.WebView2.Interop;

delegate LRESULT WndProc(HWND hwnd, uint msg, WPARAM wParam, LPARAM lParam);

internal class WindowClass<TWindow> where TWindow : WindowBase, new()
{
   [ThreadStatic]
   private static ushort _atom = 0;
   
   [ThreadStatic]
   private static WndProc _wndProc = OnWndProc;
   
   [ThreadStatic]
   private static TWindow? _window;
   
   
   private static string ClassName { get; } = $"Win32::{nameof(WindowClass<TWindow>)}";
   
   private static HINSTANCE Instance { get; } = (HINSTANCE)Marshal.GetHINSTANCE(typeof(WindowClass<TWindow>).Module);

   public static void RegisterClass(
      bool redrawWindowOnVerticalChange = true,
      bool redrawWindowOnHorizontalChange = true,
      bool enableDoubleClickMessages = false,
      bool cacheBackgroundBitmap = false,
      bool useParentClippingRect = false,
      bool enableDropShadow = false,
      int extraClassBytes = 0,
      int extraWindowBytes = 0,
      IntPtr background = 0) 
   {
      if ( _atom != 0 )
      {
         return;
      }

      WNDCLASS_STYLES styles = 0;

      if (enableDoubleClickMessages)
      {
         styles |= WNDCLASS_STYLES.CS_DBLCLKS;
      }
      if (redrawWindowOnVerticalChange)
      {
         styles |= WNDCLASS_STYLES.CS_VREDRAW;
      }
      if (redrawWindowOnHorizontalChange)
      {
         styles |= WNDCLASS_STYLES.CS_HREDRAW;
      }
      if (cacheBackgroundBitmap)
      {
         styles |= WNDCLASS_STYLES.CS_SAVEBITS;
      }
      if (useParentClippingRect)
      {
         styles |= WNDCLASS_STYLES.CS_PARENTDC;
      }
      if (enableDropShadow)
      {
         styles |= WNDCLASS_STYLES.CS_DROPSHADOW;
      }

      unsafe
      {
         var wcex = new WNDCLASSEXW();
         wcex.cbSize = (uint)Marshal.SizeOf(wcex);
         wcex.style = styles;
         wcex.lpfnWndProc = (delegate* unmanaged[Stdcall]<HWND, uint, WPARAM, LPARAM, LRESULT>)Marshal.GetFunctionPointerForDelegate(_wndProc).ToPointer();
         wcex.cbClsExtra = extraClassBytes;
         wcex.cbWndExtra = extraWindowBytes;
         wcex.hInstance = Instance;
         wcex.hbrBackground = (HBRUSH)background;
         wcex.lpszClassName = (char *)Marshal.StringToHGlobalUni(ClassName);

         _atom = RegisterClassEx(wcex);
      }
   }

   internal static unsafe TWindow? CreateWindow(
      WINDOW_EX_STYLE exStyle,
      string windowName,
      WINDOW_STYLE style,
      int x, int y,
      int width, int height,
      HWND parent,
      object? param = null)
   {
      _window = null;
      
      CreateWindowEx(
         exStyle,
         ClassName,
         windowName,
         style,
         x,
         y,
         width,
         height,
         parent,
         new UnmanagedSafeHandle(IntPtr.Zero),
         new UnmanagedSafeHandle(Instance.Value),
         null);
      return _window;
   }

   private static LRESULT OnWndProc(HWND hwnd, uint message, WPARAM wParam, LPARAM lParam)
   {
      // This window proc is only ever used to receive the first message
      // intended for a window.  Here we create an instance of the real
      // TWindow type. 
      _window = new TWindow();

      // Pass the parameter for this first message to the new window.
      // It will replace the window proceedure and pass this first
      // message to it.
      return _window.InitializeFromFirstMessage(hwnd, message, wParam, lParam);
   }
}