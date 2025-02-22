namespace GLImp;

public class JoystickManager {
	public const int MaxJoysticks = 4;
	public static JoystickDevice[] Joysticks = new JoystickDevice[MaxJoysticks];

	internal static void Init() {
		for (var i = 0; i < MaxJoysticks; i++) {
			Joysticks[i] = new JoystickDevice(i);
		}
	}

	internal static void Update() {
		foreach (var dev in Joysticks) {
			dev.Update();
		}
	}
}