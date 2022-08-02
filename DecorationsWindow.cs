using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenReaderTest
{
    public class DecorationsWindow : CustomNativeWindow
    {
        public DecorationsWindow()
        {
            InitWindow();
        }

        private void InitWindow()
        {
            var createParams = new CreateParams();
            createParams.Caption = "Decorations window";
            CreateHandle(createParams);
            Utils.SetChildWindowStyles(Handle);
        }
    }
}
