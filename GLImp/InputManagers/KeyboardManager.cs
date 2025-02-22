using System.Collections.Generic;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GLImp;

public class KeyboardManager {
	private static Dictionary<Keys, bool> prevkeys = new();
	private static readonly Dictionary<Keys, bool> keys = new();

	internal static void Init() {
		GraphicsManager.Instance.KeyDown += KeyDown;
		GraphicsManager.Instance.KeyUp += KeyUp;
	}

	internal static void Update() {
		prevkeys = new Dictionary<Keys, bool>();
		foreach (var entry in keys) {
			prevkeys.Add(entry.Key, entry.Value);
		}
	}

	//Key will tell you which key has changed state, IsDown is true if the key is currently being pressed
	internal static void KeyDown(KeyboardKeyEventArgs key) {
		keys[key.Key] = true;
	}

	internal static void KeyUp(KeyboardKeyEventArgs key) {
		keys[key.Key] = false;
	}

	//Returns if the keyboard key is currently down or not
	public static bool IsDown(Keys key) => keys.ContainsKey(key) && keys[key];

	public static bool IsUp(Keys key) => !IsDown(key);

	public static List<Keys> GetAllDownKeys() {
		var l = new List<Keys>();

		foreach (var pair in keys) {
			if (IsDown(pair.Key)) {
				l.Add(pair.Key);
			}
		}

		return l;
	}

	//If a key was pressed since last time we checked
	public static bool IsPressed(Keys key) {
		if (prevkeys.ContainsKey(key) && prevkeys[key]) {
			//If it was previously down
			return false;
		}

		if (keys.ContainsKey(key) && keys[key]) {
			//Previously up & is currently down
			return true;
		} //The key is currently not down

		return false;
	}

	public static List<Keys> GetAllPressedKeys() {
		var l = new List<Keys>();

		foreach (var pair in keys) {
			if (IsPressed(pair.Key)) {
				l.Add(pair.Key);
			}
		}

		return l;
	}

	//If a key was released since last time we checked
	public static bool IsReleased(Keys key) {
		if (prevkeys.ContainsKey(key) && !prevkeys[key]) {
			//If it was previously up
			return false;
		}

		if (keys.ContainsKey(key) && !keys[key]) {
			//Previously down & is currently up
			return true;
		} //The key is currently not up

		return false;
	}

	public static List<Keys> GetAllReleasedKeys() {
		var l = new List<Keys>();

		foreach (var pair in keys) {
			if (IsReleased(pair.Key)) {
				l.Add(pair.Key);
			}
		}

		return l;
	}
}