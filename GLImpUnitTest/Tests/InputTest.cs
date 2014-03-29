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
			//Console.WriteLine(GamePad.GetState(0).DPad.IsLeft);
			string buttons = "";

			buttons += JoystickManager.Joysticks[0].State.GetHat(JoystickHat.Hat0).Position + ", ";

			for (int i = 0; i < JoystickManager.Joysticks[0].Capabilities.ButtonCount; i++) {
				if (JoystickManager.Joysticks[0].IsDown((JoystickButton)i)) {
					buttons += "JS-" + i + ", ";
				}
			}

			foreach (Key k in KeyboardManager.GetAllDownKeys()) {
				buttons += k.ToString() + ", ";
			}

			GraphicsManager.DrawString(0, 0, buttons);

			if (JoystickManager.Joysticks[0].IsConnected) {
				for (int i = 0; i < JoystickManager.Joysticks[0].Capabilities.AxisCount; i++) {
					GraphicsManager.DrawString(0, 20 * (i + 1), "Axis " + i + ": " + JoystickManager.Joysticks[0].State.GetAxis((JoystickAxis)i));
				}
			}
        }
    }
}
