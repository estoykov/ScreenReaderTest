using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenReaderTest
{
    public class Utils
    {
        public static void SetChildWindowStyles(IntPtr handle)
        {
            var styles = Win32.GetWindowStyles(handle);
            styles = styles & ~(Win32.WS_CAPTION | Win32.WS_THICKFRAME | Win32.WS_OVERLAPPEDWINDOW | Win32.WS_POPUP)
                | Win32.WS_CHILDWINDOW;
            Win32.SetWindowStyles(handle, styles);
            var exStyles = Win32.GetWindowExStyles(handle);
            exStyles &= ~(Win32.WS_EX_APPWINDOW);
            exStyles |= Win32.WS_EX_TOOLWINDOW;
            Win32.SetWindowExStyles(handle, exStyles);
        }

        public static void SetTopWindowStyles(IntPtr handle)
        {
            var styles = Win32.GetWindowStyles(handle);
            styles = styles & ~(Win32.WS_CHILDWINDOW) |
                Win32.WS_CLIPCHILDREN | Win32.WS_CLIPSIBLINGS |
                Win32.WS_OVERLAPPED | Win32.WS_CAPTION | Win32.WS_SYSMENU |
                Win32.WS_MINIMIZEBOX | Win32.WS_MAXIMIZEBOX | Win32.WS_THICKFRAME;
            Win32.SetWindowStyles(handle, styles);
            var exStyles = Win32.GetWindowExStyles(handle);
            exStyles &= ~(Win32.WS_EX_TOOLWINDOW);
            exStyles |= Win32.WS_EX_APPWINDOW;
            Win32.SetWindowExStyles(handle, exStyles);
        }

        public static void SetLayeredWindowStyles(IntPtr handle, Color keyColor)
        {
            var exStyles = Win32.GetWindowExStyles(handle);
            exStyles = exStyles | Win32.WS_EX_LAYERED;
            Win32.SetWindowExStyles(handle, exStyles);
            Win32.SetLayeredWindowAttributes(handle, ColorTranslator.ToWin32(keyColor), 0, Win32.LWA_COLORKEY);
        }

        public static Point PointToScreen(IntPtr handle, int x, int y)
        {
            var p = new sPOINT() { x = x, y = y };
            Win32.ClientToScreen(handle, ref p);
            return new Point(p.x, p.y);
        }

        public static Point PointToClient(IntPtr handle, int x, int y)
        {
            var p = new sPOINT() { x = x, y = y };
            Win32.ScreenToClient(handle, ref p);
            return new Point(p.x, p.y);
        }

        public static void FillRect(IntPtr hDC, Rectangle rect, Color color)
        {
            using (var g = Graphics.FromHdc(hDC))
            {
                using (var brush = new SolidBrush(color))
                {
                    g.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
                }
            }
        }
    }
}
