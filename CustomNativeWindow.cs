using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenReaderTest
{
    public class CustomNativeWindow : NativeWindow
    {
        private Rectangle bounds_;

        public string Caption
        {
            get
            {
                return Win32.GetWindowCaption(Handle);
            }
            set
            {
                var text = value ?? "";
                Win32.SendMessage(Handle, Win32.WM_SETTEXT, IntPtr.Zero, text);
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return bounds_;
            }
        }

        public Rectangle ClientRect
        {
            get
            {
                Win32.GetClientRect(Handle, out sRECT sClientRect);
                return sClientRect.ToRectangle();
            }
        }

        public FormWindowState WindowState
        {
            get
            {
                return Win32.GetWindowState(Handle);
            }
        }

        public void Show(bool activate)
        {
            var cmdShow = activate ? Win32.SW_SHOW : Win32.SW_SHOWNA;
            Win32.ShowWindow(Handle, cmdShow);
        }

        public Point PointToClient(int x, int y)
        {
            return Utils.PointToClient(Handle, x, y);
        }

        public Point PointToClient(Point point)
        {
            return PointToClient(point.X, point.Y);
        }

        public Point PointToScreen(int x, int y)
        {
            return Utils.PointToScreen(Handle, x, y);
        }

        public Point PointToScreen(Point point)
        {
            return PointToScreen(point.X, point.Y);
        }

        public void SetParent(IntPtr hParent)
        {
            Win32.SetParent(Handle, hParent);
        }

        public void SetBounds(Rectangle bounds)
        {
            Win32.SetWindowBounds(Handle, bounds.X, bounds.Y, bounds.Width, bounds.Height);
        }

        public void SetBounds(IntPtr insertAfter, Rectangle bounds)
        {
            Win32.SetWindowBounds(Handle, insertAfter, bounds.X, bounds.Y, bounds.Width, bounds.Height);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case Win32.WM_ERASEBKGND:
                    if (OnEraseBackground(ref m))
                    {
                        return;
                    }
                    break;
                case Win32.WM_PAINT:
                    if (OnPaint(ref m))
                    {
                        return;
                    }
                    break;
            }
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case Win32.WM_DESTROY:
                    OnDestroyedCore();
                    break;
                case Win32.WM_SHOWWINDOW:
                    OnShowWindow(m);
                    break;
                case Win32.WM_WINDOWPOSCHANGED:
                    OnWindowPosChanged(m);
                    break;
            }
        }

        private void OnShowWindow(Message m)
        {
            var isVisible = m.WParam.ToInt64() != 0;
            if (isVisible)
            {
                var bounds = Win32.GetWindowBounds(Handle);
                CheckBoundsChanged(bounds);
            }
        }

        private bool OnEraseBackground(ref Message m)
        {
            if (m.WParam != IntPtr.Zero)
            {
                return OnEraseBackgroundCore(m.WParam);
            }
            return false;
        }

        private bool OnPaint(ref Message m)
        {
            sRECT updateRec;
            if (Win32.GetUpdateRect(Handle, out updateRec, false))
            {
                sPAINTSTRUCT paintStruct;
                var hDC = Win32.BeginPaint(Handle, out paintStruct);
                try
                {
                    if (hDC != IntPtr.Zero)
                    {
                        return OnPaintCore(hDC);
                    }
                    return false;
                }
                finally
                {
                    if (hDC != IntPtr.Zero)
                    {
                        Win32.EndPaint(Handle, ref paintStruct);
                    }
                }
            }
            else
            {
                var hDC = Win32.GetDC(Handle);
                try
                {
                    return OnPaintCore(hDC);
                }
                finally
                {
                    Win32.ReleaseDC(Handle, hDC);
                }
            }
        }

        private void OnWindowPosChanged(Message m)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                return;
            }
            var newBounds = bounds_;
            var windowPos = sWINDOWPOS.FromPtr(m.LParam);
            var flags = windowPos.flags;
            if ((flags & Win32.SWP_NOMOVE) == 0)
            {
                newBounds.X = windowPos.x;
                newBounds.Y = windowPos.y;
            }
            if ((flags & Win32.SWP_NOSIZE) == 0)
            {
                newBounds.Width = windowPos.cx;
                newBounds.Height = windowPos.cy;
            }
            CheckBoundsChanged(newBounds);
        }

        private void CheckBoundsChanged(Rectangle newBounds)
        {
            if (newBounds != bounds_)
            {
                var prevBounds = bounds_;
                bounds_ = newBounds;
                OnBoundsChangedCore(prevBounds, newBounds);
            }
        }

        protected virtual void OnDestroyedCore()
        {
        }

        protected virtual void OnBoundsChangedCore(Rectangle prevBounds, Rectangle newBounds)
        {
        }

        protected virtual bool OnEraseBackgroundCore(IntPtr hDC)
        {
            return false;
        }

        protected virtual bool OnPaintCore(IntPtr hDC)
        {
            return false;
        }

        protected void FillBackground(IntPtr hDC, Color color)
        {
            var bounds = Bounds;
            bounds.Location = Point.Empty;
            Utils.FillRect(hDC, bounds, color);
        }
    }
}
