using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace GLImp {
	public class JoystickManager {
		static bool[][] PrevButtonState;

		internal static void Init() {
			PrevButtonState = GetButtonState();
		}

		private static bool[][] GetButtonState() {
			bool[][] CurrentButtonState = new bool[GraphicsManager.joysticks.Count][];
			for (int i = 0; i < GraphicsManager.joysticks.Count; i++) {
				CurrentButtonState[i] = new bool[GetDevice(i).Button.Count];
				for (int j = 0; j < CurrentButtonState[i].Length; j++) {
					CurrentButtonState[i][j] = IsDown(i, j);
				}
			}
			return CurrentButtonState;
		}

		internal static void Update() {
			PrevButtonState = GetButtonState();
		}

		#region Buttons
		public static int GetButtonCount(int DeviceNumber) {
			if (GetDevice(DeviceNumber) != null) {
				return GetDevice(DeviceNumber).Button.Count;
			} else {
				return 0;
			}
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

		public static bool IsUp(int DeviceNumber, int ButtonNumber) {
			return !IsDown(DeviceNumber, ButtonNumber);
		}

		public static bool IsPressed(int DeviceNumber, int ButtonNumber) {
			if (GetDevice(DeviceNumber) == null) return false;
			bool DownNow = IsDown(DeviceNumber, ButtonNumber);
			bool DownPrev = PrevButtonState[DeviceNumber][ButtonNumber];

			if (DownNow && !DownPrev) return true;
			return false;
		}

		public static bool IsReleased(int DeviceNumber, int ButtonNumber) {
			if (GetDevice(DeviceNumber) == null) return false;
			bool DownNow = IsDown(DeviceNumber, ButtonNumber);
			bool DownPrev = PrevButtonState[DeviceNumber][ButtonNumber];

			if (!DownNow && DownPrev) return true;
			return false;
		}
		#endregion

		public static float GetAxis(int DeviceNumber, int Axis) {
			JoystickDevice js = GetDevice(DeviceNumber);
			if (js != null) {
				if (Axis < js.Axis.Count) {
					return js.Axis[Axis];
				}
			}

			return 0;
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
