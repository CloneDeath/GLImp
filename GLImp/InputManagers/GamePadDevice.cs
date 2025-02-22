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
		public GamepadState State;
		public GamepadState PreviousState;
		// public GamepadCapabilities Capabilities;

		internal GamePadDevice(int DeviceID) {
			this.DeviceID = DeviceID;
		}

		public bool IsConnected {
			get {
				return GLFW.JoystickPresent(DeviceID); // State.IsConnected;
			}
		}

		internal void Update() {
			PreviousState = State;
			GLFW.GetGamepadState(DeviceID, out State);
			//Capabilities = OpenTK.Input.GamePad.GetCapabilities(DeviceID);
		}

		public bool IsDown(OpenTK.Input.Buttons Button)
		{
			return State.Buttons.GetButton(Button) == ButtonState.Pressed;
		}

		public bool IsUp(OpenTK.Input.Buttons Button)
		{
			return State.Buttons.GetButton(Button) == ButtonState.Released;
		}

		public bool IsPressed(OpenTK.Input.Buttons Button)
		{
			return State.Buttons.GetButton(Button) == ButtonState.Pressed && PreviousState.Buttons.GetButton(Button) == ButtonState.Released;
		}

		public bool IsReleased(OpenTK.Input.Buttons Button)
		{
			return State.Buttons.GetButton(Button) == ButtonState.Released && PreviousState.Buttons.GetButton(Button) == ButtonState.Pressed;
		}
	}
}
