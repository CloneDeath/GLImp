namespace GLImp.Input;

public class GamePadManager {
	public const int MaxJoysticks = 4;
	public static GamePadDevice[] GamePads { get; } = new GamePadDevice[MaxJoysticks];

	internal static void Init() {
		for (var i = 0; i < MaxJoysticks; i++) {
			GamePads[i] = new GamePadDevice(i);
		}
	}

	internal static void Update() {
		foreach (var dev in GamePads) {
			dev.Update();
		}
	}
}