using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using GLImp;

namespace GLImp {
	public class KeyboardManager {
		private static Dictionary<Key, bool> prevkeys = new Dictionary<Key, bool>();
		private static Dictionary<Key, bool> keys = new Dictionary<Key, bool>();

		internal static void Init() {
			GraphicsManager.keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>(KeyDown);
			GraphicsManager.keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(KeyUp);
		}

		internal static void Update() {
			prevkeys = new Dictionary<Key, bool>();
			foreach (KeyValuePair<Key, bool> entry in keys) {
				prevkeys.Add(entry.Key, entry.Value);
			}
		}

		//Key will tell you which key has changed state, IsDown is true if the key is currently being pressed
		internal static void KeyDown(object sender, KeyboardKeyEventArgs key) {
			if (keys.ContainsKey(key.Key))
			{
				keys[key.Key] = true;
			}
			else
			{
				keys.Add(key.Key, true);
			}
		}

		internal static void KeyUp(object sender, KeyboardKeyEventArgs key)
		{
			if (keys.ContainsKey(key.Key))
			{
				keys[key.Key] = false;
			}
			else
			{
				keys.Add(key.Key, false);
			}
		}

		//Returns if the keyboard key is currently down or not
		public static bool IsDown(Key key) {
			if (keys.ContainsKey(key))
			{
				return keys[key];
			}
			else
			{
				return false;
			}
		}

		public static bool IsUp(Key key)
		{
			return !IsDown(key);
		}

		

		public static List<Key> GetAllDownKeys() {
			List<Key> l = new List<Key>();

			foreach (KeyValuePair<Key, bool> pair in keys) {
				if (IsDown(pair.Key)) {
					l.Add(pair.Key);
				}
			}

			return l;
		}

		//If a key was pressed since last time we checked
		public static bool IsPressed(Key key) {
			if(prevkeys.ContainsKey(key) && prevkeys[key]) { //If it was previously down
				return false;
			} else if(keys.ContainsKey(key) && keys[key]) { //Previously up & is currently down
				return true;
			} else { //The key is currently not down
				return false;
			}
		}
		

		public static List<Key> GetAllPressedKeys() {
			List<Key> l = new List<Key>();

			foreach(KeyValuePair<Key, bool> pair in keys) {
				if(IsPressed(pair.Key)) {
					l.Add(pair.Key);
				}
			}

			return l;
		}

		//If a key was released since last time we checked
		public static bool IsReleased(Key key) {
			if (prevkeys.ContainsKey(key) && !prevkeys[key]) { //If it was previously up
				return false;
			} else if (keys.ContainsKey(key) && !keys[key]) { //Previously down & is currently up
				return true;
			} else { //The key is currently not up
				return false;
			}
		}

		

		public static List<Key> GetAllReleasedKeys() {
			List<Key> l = new List<Key>();

			foreach (KeyValuePair<Key, bool> pair in keys) {
				if (IsReleased(pair.Key)) {
					l.Add(pair.Key);
				}
			}

			return l;
		}
	}
}
