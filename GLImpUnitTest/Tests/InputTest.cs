using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using System.Drawing;
using OpenTK.Input;

namespace GLImpUnitTest.Tests
{
    public class InputTest : TestSetup
    {
		public InputTest() {
			Name = "Input Test";
		}

        public override void Render2D()
        {
			string buttons = "";
			for (int i = 0; i < JoystickManager.GetButtonCount(0); i++) {
				if (JoystickManager.IsDown(0, i)) {
					buttons += "JS-" + i + ", ";
				}
			}

			foreach (Key k in KeyboardManager.GetAllDownKeys()) {
				buttons += k.ToString() + ", ";
			}

			GraphicsManager.DrawString(0, 0, buttons);

			JoystickDevice js = JoystickManager.GetDevice(0);
			if (js != null) {
				for (int i = 0; i < js.Axis.Count; i++) {
					GraphicsManager.DrawString(0, 20 * (i + 1), "Axis " + i + ": " + js.Axis[i]);
				}
			}
        }
    }
}
