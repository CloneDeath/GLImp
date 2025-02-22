using System.Collections.Generic;
using System.Linq;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GLImp.Input;

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
		return keys.Where(pair => IsDown(pair.Key)).Select(pair => pair.Key).ToList();
	}

	//If a key was pressed since last time we checked
	public static bool IsPressed(Keys key) {
		if (prevkeys.TryGetValue(key, out var value) && value) {
			return false;
		}
		return keys.ContainsKey(key) && keys[key];
	}

	public static List<Keys> GetAllPressedKeys() {
		return keys.Where(pair => IsPressed(pair.Key)).Select(pair => pair.Key).ToList();
	}

	//If a key was released since last time we checked
	public static bool IsReleased(Keys key) {
		if (prevkeys.TryGetValue(key, out var value) && !value) {
			return false;
		}
		return keys.ContainsKey(key) && !keys[key];
	}

	public static List<Keys> GetAllReleasedKeys() {
		return keys.Where(pair => IsPressed(pair.Key)).Select(pair => pair.Key).ToList();
	}
}