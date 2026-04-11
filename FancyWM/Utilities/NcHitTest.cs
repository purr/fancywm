using System;
using System.Runtime.InteropServices;

namespace FancyWM.Utilities
{
    /// <summary>
    /// Thin wrapper around WM_NCHITTEST for classifying whether a window
    /// considers the current cursor position a sizing border or not.
    /// Used at gesture start to distinguish edge-resize from title-bar-move,
    /// since WinMan fires the same EVENT_SYSTEM_MOVESIZESTART for both.
    /// </summary>
    internal static class NcHitTest
    {
        private const int WM_NCHITTEST = 0x0084;

        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;
        private const int HTBORDER = 18;
        private const int HTSIZE = 4;  // aka HTGROWBOX

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern nint SendMessage(IntPtr hWnd, int msg, nint wParam, nint lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out POINT pt);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT { public int X, Y; }

        private static nint MakeLParam(int x, int y)
            => (x & 0xFFFF) | ((y & 0xFFFF) << 16);

        /// <summary>
        /// Returns true if the window reports that the current cursor position
        /// is over a sizing border or corner (HTLEFT..HTBOTTOMRIGHT, HTBORDER, HTSIZE).
        /// </summary>
        public static bool IsBorderResize(IntPtr hwnd)
        {
            if (!GetCursorPos(out var cursor))
                return false;
            nint result = SendMessage(hwnd, WM_NCHITTEST, 0, MakeLParam(cursor.X, cursor.Y));
            int code = (int)(result & 0xFFFF);
            return code is HTLEFT or HTRIGHT or HTTOP or HTTOPLEFT or HTTOPRIGHT
                       or HTBOTTOM or HTBOTTOMLEFT or HTBOTTOMRIGHT
                       or HTBORDER or HTSIZE;
        }
    }
}
