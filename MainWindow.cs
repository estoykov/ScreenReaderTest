using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenReaderTest
{
    public class MainWindow : CustomNativeWindow
    {
        private bool controlsOnTop_ = true;

        public MainWindow()
        {
            InitWindow();
            AddChildren();
        }

        public ControlsWindow ControlsWindow { get; private set; }

        public ApplicationWindow ApplicationWindow { get; private set; }

        public string ZOrderText
        { 
            get
            {
                return controlsOnTop_ ? "Controls on top" : "Controls at bottom";
            }
        }

        public void ToggleZOrder()
        {
            controlsOnTop_ = !controlsOnTop_;
            UpdateChildrenBounds();
        }

        private void InitWindow()
        {
            var createParams = new CreateParams();
            createParams.X = 100;
            createParams.Y = 100;
            createParams.Width = 800;
            createParams.Height = 600;
            createParams.Caption = "Screen reader test";
            CreateHandle(createParams);
            Utils.SetTopWindowStyles(Handle);
        }

        private void AddChildren()
        {
            ControlsWindow = new ControlsWindow(this);
            ControlsWindow.SetParent(Handle);
            ControlsWindow.Show(false);
            ApplicationWindow = new ApplicationWindow();
            ApplicationWindow.SetParent(Handle);
            ApplicationWindow.Show(false);
        }

        protected override void OnDestroyedCore()
        {
            base.OnDestroyedCore();
            Win32.PostQuitMessage(0);
        }

        protected override bool OnEraseBackgroundCore(IntPtr hDC)
        {
            return true;
        }

        protected override bool OnPaintCore(IntPtr hDC)
        {
            return true;
        }

        protected override void OnBoundsChangedCore(Rectangle prevBounds, Rectangle newBounds)
        {
            base.OnBoundsChangedCore(prevBounds, newBounds);
            UpdateChildrenBounds();
        }

        private void UpdateChildrenBounds()
        {
            var clientRect = ClientRect;
            var controlsInsertAfter = controlsOnTop_ ? Win32.HWND_TOP : Win32.HWND_BOTTOM;
            ControlsWindow.SetBounds(controlsInsertAfter, clientRect);
            var windowArea = ControlsWindow.WindowArea;
            var applicationInsertAfter = controlsOnTop_ ? Win32.HWND_BOTTOM : Win32.HWND_TOP;
            ApplicationWindow.SetBounds(applicationInsertAfter, windowArea);
        }
    }
}
