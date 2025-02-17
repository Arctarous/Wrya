using System.Drawing;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using static Windows.Win32.PInvoke;

namespace Wryava.WebView2.Interop;

internal class WindowBase
{
   private readonly WndProc _wndProc;
   
   protected WindowBase()
   {
      _wndProc = new(OnWndProc);
   }
   
   public HWND? Hwnd { get; private set; }
   
   public event EventHandler? WindowPosChanged;
   
   public event EventHandler? Closed;
   
   public LRESULT InitializeFromFirstMessage(HWND hwnd, uint message, WPARAM wParam, LPARAM lParam)
   {
      Hwnd = hwnd;
      
      // Replace the window procedure for this window instance.
      NativeMethods.SetWindowLongPtr(hwnd, WINDOW_LONG_PTR_INDEX.GWLP_WNDPROC, Marshal.GetFunctionPointerForDelegate(_wndProc));
      
      // Give the window a chance to initialize.
      OnCreate();
      
      // Manually invoke the window procedure for this message.
      return OnWndProc(hwnd, message, wParam, lParam);
   }
   
   LRESULT OnWndProc(HWND hwnd, uint message, WPARAM wParam, LPARAM lParam)
   {
      switch ( message )
      {
         case WM_DESTROY:
            DestroyWindow(hwnd);
            Closed?.Invoke(this, EventArgs.Empty);
            break;
         case WM_WINDOWPOSCHANGED:
            WindowPosChanged?.Invoke(this, EventArgs.Empty);
            break;
         default:
            break;
      }
      
      return OnMessage(hwnd, message, wParam, lParam);
   }
   
   protected virtual LRESULT OnMessage(HWND hwnd, uint message, WPARAM wParam, LPARAM lParam)
   {
      return DefWindowProc(hwnd, message, wParam, lParam);
   }

   protected virtual void OnCreate() { }

   public Rectangle ClientRect
   {
      get
      {
         GetWindowRect(Hwnd!.Value, out var rect);
         Point topLeft = new(rect.left, rect.top);
         Point bottomRight = new(rect.right, rect.bottom);
         ScreenToClient(Hwnd!.Value, ref topLeft);
         ScreenToClient(Hwnd!.Value, ref bottomRight);
         
         return new Rectangle(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
      }
   }

   public bool IsWindow => IsWindow(Hwnd!.Value);
   
   public void Close() => DestroyWindow(Hwnd!.Value);
}

internal class WindowBase<TWindow> : WindowBase where TWindow : WindowBase, new()
{
   static WindowBase()
   {
      WindowClass<TWindow>.RegisterClass();
   }
   
   public static TWindow? Create(
      WINDOW_EX_STYLE exStyle,
      string windowName,
      WINDOW_STYLE style,
      int x, int y,
      int width, int height,
      HWND parent)
   {
      return WindowClass<TWindow>.CreateWindow(exStyle, windowName, style, x, y, width, height, parent);
   }
}