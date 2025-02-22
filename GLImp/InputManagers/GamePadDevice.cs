using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GLImp {
	public class GamePadDevice {
		public int DeviceID {
			get;
			private set;
		}
		public JoystickInputAction[] State;
		public JoystickInputAction[] PreviousState;
		// public GamepadCapabilities Capabilities;

		internal GamePadDevice(int DeviceID) {
			this.DeviceID = DeviceID;
		}

		public bool IsConnected {
			get {
				return GLFW.JoystickPresent(DeviceID) && GLFW.JoystickIsGamepad(DeviceID); // State.IsConnected;
			}
		}

		internal void Update() {
			PreviousState = State;
			State = GLFW.GetJoystickButtons(DeviceID).ToArray();
			//Capabilities = OpenTK.Input.GamePad.GetCapabilities(DeviceID);
		}

		public bool IsDown(int Button) {
			return State[Button] == JoystickInputAction.Press;
		}

		public bool IsUp(int Button) {
			return State[Button] == JoystickInputAction.Release;
		}

		public bool IsPressed(int Button) {
			return State[Button] == JoystickInputAction.Press && PreviousState[Button] == JoystickInputAction.Release;
		}

		public bool IsReleased(int Button) {
			return State[Button] == JoystickInputAction.Release && PreviousState[Button] == JoystickInputAction.Press;
		}
	}
}
