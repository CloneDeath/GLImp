using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace GLImp {
	public class JoystickManager {
		public static float GetAxis(int DeviceNumber, int Axis) {
			JoystickDevice js = GetDevice(DeviceNumber);
			if (DeviceNumber != null) {
				if (Axis < js.Axis.Count) {
					return js.Axis[Axis];
				}
			}

			return 0;
		}

		public static bool IsDown(int DeviceNumber, int ButtonNumber) {
			JoystickDevice js = GetDevice(DeviceNumber);
			if (js != null) {
				if (ButtonNumber < js.Button.Count) {
					return js.Button[ButtonNumber];
				}
			}

			return false;
		}

		public static int GetButtonCount(int DeviceNumber) {
			if (GetDevice(DeviceNumber) != null){
				return GetDevice(DeviceNumber).Button.Count;
			} else {
				return 0;
			}
		}

		public static JoystickDevice GetDevice(int DeviceNumber) {
			if (DeviceNumber < GraphicsManager.joysticks.Count) {
				return GraphicsManager.joysticks[DeviceNumber];
			} else {
				return null;
			}
		}
	}
}
