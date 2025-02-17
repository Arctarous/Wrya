using System.Runtime.InteropServices;

namespace Wryava.WebView2.Interop;

public class UnmanagedSafeHandle(IntPtr handle) : SafeHandle(handle, false)
{
   protected override bool ReleaseHandle()
   {
      return false;
   }
   
   public override bool IsInvalid => handle == IntPtr.Zero;
}