using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace GLImp {
	public class JoystickDevice {
		public int DeviceID {
			get;
			private set;
		}
		public JoystickState State;
		public JoystickState PreviousState;
		public JoystickCapabilities Capabilities;

		internal JoystickDevice(int DeviceID) {
			this.DeviceID = DeviceID;
		}

		public bool IsConnected {
			get {
				return State.IsConnected;
			}
		}

		internal void Update() {
			PreviousState = State;
			State = OpenTK.Input.Joystick.GetState(DeviceID);
			Capabilities = OpenTK.Input.Joystick.GetCapabilities(DeviceID);
		}

		public bool IsDown(JoystickButton Button) {
			return State.IsButtonDown(Button);
		}

		public bool IsUp(JoystickButton Button) {
			return State.IsButtonUp(Button);
		}

		public bool IsPressed(JoystickButton Button) {
			return State.IsButtonDown(Button) && PreviousState.IsButtonUp(Button);
		}

		public bool IsReleased(JoystickButton Button) {
			return State.IsButtonUp(Button) && PreviousState.IsButtonDown(Button);
		}
	}
}
