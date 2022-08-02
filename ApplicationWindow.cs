using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenReaderTest
{
    public class ApplicationWindow : CustomNativeWindow
    {
        public ApplicationWindow()
        {
            InitWindow();
            AddControls();
        }

        public Panel ContentPanel { get; private set; }
        public Label TextLabel { get; private set; }

        private void InitWindow()
        {
            var createParams = new CreateParams();
            createParams.Caption = "Application window";
            CreateHandle(createParams);
            Utils.SetChildWindowStyles(Handle);
        }

        private void AddControls()
        {
            TextLabel = new Label();
            TextLabel.Dock = DockStyle.Fill;
            TextLabel.Text = "The quick brown fox jumps over the lazy dog";
            TextLabel.TextAlign = ContentAlignment.MiddleCenter;
            ContentPanel = new Panel();
            ContentPanel.CreateControl();
            ContentPanel.Controls.Add(TextLabel);
            Win32.SetParent(ContentPanel.Handle, Handle);
            ContentPanel.Visible = true;
        }

        protected override void OnBoundsChangedCore(Rectangle prevBounds, Rectangle newBounds)
        {
            base.OnBoundsChangedCore(prevBounds, newBounds);
            var clientRect = ClientRect;
            Win32.SetWindowBounds(ContentPanel.Handle, clientRect);
        }
    }
}
