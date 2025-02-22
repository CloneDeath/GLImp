using System.Collections.Generic;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GLImp;

public class KeyboardManager {
	private static Dictionary<Keys, bool> prevkeys = new Dictionary<Keys, bool>();
	private static Dictionary<Keys, bool> keys = new Dictionary<Keys, bool>();

	internal static void Init() {
		GraphicsManager.Instance.KeyDown += KeyDown;
		GraphicsManager.Instance.KeyUp += KeyUp;
	}

	internal static void Update() {
		prevkeys = new Dictionary<Keys, bool>();
		foreach (KeyValuePair<Keys, bool> entry in keys) {
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
	public static bool IsDown(Keys key) {
		return keys.ContainsKey(key) && keys[key];
	}

	public static bool IsUp(Keys key)
	{
		return !IsDown(key);
	}



	public static List<Keys> GetAllDownKeys() {
		List<Keys> l = new List<Keys>();

		foreach (KeyValuePair<Keys, bool> pair in keys) {
			if (IsDown(pair.Key)) {
				l.Add(pair.Key);
			}
		}

		return l;
	}

	//If a key was pressed since last time we checked
	public static bool IsPressed(Keys key) {
		if(prevkeys.ContainsKey(key) && prevkeys[key]) { //If it was previously down
			return false;
		} else if(keys.ContainsKey(key) && keys[key]) { //Previously up & is currently down
			return true;
		} else { //The key is currently not down
			return false;
		}
	}


	public static List<Keys> GetAllPressedKeys() {
		List<Keys> l = new List<Keys>();

		foreach(KeyValuePair<Keys, bool> pair in keys) {
			if(IsPressed(pair.Key)) {
				l.Add(pair.Key);
			}
		}

		return l;
	}

	//If a key was released since last time we checked
	public static bool IsReleased(Keys key) {
		if (prevkeys.ContainsKey(key) && !prevkeys[key]) { //If it was previously up
			return false;
		} else if (keys.ContainsKey(key) && !keys[key]) { //Previously down & is currently up
			return true;
		} else { //The key is currently not up
			return false;
		}
	}



	public static List<Keys> GetAllReleasedKeys() {
		List<Keys> l = new List<Keys>();

		foreach (KeyValuePair<Keys, bool> pair in keys) {
			if (IsReleased(pair.Key)) {
				l.Add(pair.Key);
			}
		}

		return l;
	}
}