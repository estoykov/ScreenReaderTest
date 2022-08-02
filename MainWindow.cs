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
        public MainWindow()
        {
            InitWindow();
        }

        private void InitWindow()
        {
            var createParams = new CreateParams();
            createParams.X = 100;
            createParams.Y = 100;
            createParams.Width = 800;
            createParams.Height = 600;
            createParams.Style = 
                Win32.WS_CLIPCHILDREN | Win32.WS_CLIPSIBLINGS |
                Win32.WS_OVERLAPPED | Win32.WS_CAPTION | Win32.WS_SYSMENU |
                Win32.WS_MINIMIZEBOX | Win32.WS_MAXIMIZEBOX | Win32.WS_THICKFRAME;
            createParams.Caption = "Screen reader test";
            CreateHandle(createParams);
            Show(true);
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
            FillBackground(hDC, Color.White);
            return true;
        }
    }
}
