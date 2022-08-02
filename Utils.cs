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
        public static void FillRect(IntPtr hDC, Rectangle rect, Color color)
        {
            using (var g = Graphics.FromHdc(hDC))
            {
                using (var brush = new SolidBrush(color))
                {
                    g.FillRectangle(brush, 0, 0, rect.Width, rect.Height);
                }
            }
        }
    }
}
