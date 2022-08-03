using Accessibility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenReaderTest
{
    public class CustomNativeWindowAccessibleObject : AccessibleObject
    {
        public CustomNativeWindowAccessibleObject(CustomNativeWindow owner)
        {
            Owner = owner;
            UseStdAccessibleObjects(owner.Handle);
        }

        public CustomNativeWindow Owner { get; }

        public override AccessibleObject HitTest(int x, int y)
        {
            var accessibleObject = Owner.OnAccessibleObjectHitTest(x, y);
            return accessibleObject ?? base.HitTest(x, y);
        }
    }
}
