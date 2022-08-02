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
        public DecorationsWindow DecorationsWindow { get; private set; }

        public MainWindow()
        {
            InitWindow();
            AddDecorationsWindow();
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

        private void AddDecorationsWindow()
        {
            DecorationsWindow = new DecorationsWindow();
            DecorationsWindow.SetParent(Handle);
            DecorationsWindow.Show(false);
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

        protected override void OnBoundsChangedCore(Rectangle prevBounds, Rectangle newBounds)
        {
            base.OnBoundsChangedCore(prevBounds, newBounds);
            UpdateChildrenBounds();
        }

        private void UpdateChildrenBounds()
        {
            var clientRect = ClientRect;
            DecorationsWindow.SetBounds(Win32.HWND_TOP, clientRect);
        }
    }
}
