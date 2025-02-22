using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GLImp;

public class JoystickDevice {
	public JoystickInputAction[] PreviousState;
	public JoystickInputAction[] State;
	// public JoystickCapabilities Capabilities;

	internal JoystickDevice(int DeviceID) {
		this.DeviceID = DeviceID;
		State = GLFW.GetJoystickButtons(DeviceID).ToArray();
		PreviousState = State;
	}

	public int DeviceID { get; }

	public bool IsConnected => GLFW.JoystickPresent(DeviceID);

	internal void Update() {
		PreviousState = State;
		State = GLFW.GetJoystickButtons(DeviceID).ToArray();
	}

	public bool IsDown(int Button) => State[Button] == JoystickInputAction.Press;

	public bool IsUp(int Button) => State[Button] == JoystickInputAction.Release;

	public bool IsPressed(int Button) => State[Button] == JoystickInputAction.Press &&
										 PreviousState[Button] == JoystickInputAction.Release;

	public bool IsReleased(int Button) => State[Button] == JoystickInputAction.Release &&
										  PreviousState[Button] == JoystickInputAction.Press;
}