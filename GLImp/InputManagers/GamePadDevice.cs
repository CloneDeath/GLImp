using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GLImp;

public class GamePadDevice {
	public int DeviceID {
		get;
	}
	public JoystickInputAction[] State;
	public JoystickInputAction[] PreviousState;

	internal GamePadDevice(int DeviceID) {
		this.DeviceID = DeviceID;
		State = GLFW.GetJoystickButtons(DeviceID).ToArray();
		PreviousState = State;
	}

	public bool IsConnected => GLFW.JoystickPresent(DeviceID) && GLFW.JoystickIsGamepad(DeviceID);

	internal void Update() {
		PreviousState = State;
		State = GLFW.GetJoystickButtons(DeviceID).ToArray();
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