using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using GLImp;

namespace GLImp {
	public class KeyboardManager {
		private static Vector2 MousePosition;
		private static Vector2 PrevMousePosition;
		private static Dictionary<Key, bool> prevkeys = new Dictionary<Key, bool>();
		private static Dictionary<Key, bool> keys = new Dictionary<Key, bool>();
		private static Dictionary<MouseButton, bool> prevmouseButtons = new Dictionary<MouseButton, bool>();
		private static Dictionary<MouseButton, bool> mouseButtons = new Dictionary<MouseButton, bool>();
		private static int WheelPosition;

		//Copy over the current state to the previous state when this is called. This will get called around 60Hz, so, make it fast!
		//Also, make sure that the current state is unchanged and independant from the previous state (ie: no shallow copy)
		internal static void Update() {
			prevmouseButtons = new Dictionary<MouseButton, bool>();
			foreach(KeyValuePair<MouseButton, bool> kvp in mouseButtons) {
				prevmouseButtons.Add(kvp.Key, kvp.Value);
			}
			
			prevkeys = new Dictionary<Key, bool>();
			foreach(KeyValuePair<Key, bool> entry in keys) {
				prevkeys.Add(entry.Key, entry.Value);
			}

			WheelPosition = 0;

			PrevMousePosition = MousePosition;
		}

		internal static void Init()
		{
			GraphicsManager.keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>(KeyDown);
			GraphicsManager.keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(KeyUp);
			GraphicsManager.mouse.ButtonDown += new EventHandler<MouseButtonEventArgs>(MouseDown);
			GraphicsManager.mouse.ButtonUp += new EventHandler<MouseButtonEventArgs>(MouseUp);
			GraphicsManager.mouse.Move += new EventHandler<MouseMoveEventArgs>(MouseMove);
			GraphicsManager.mouse.WheelChanged += new EventHandler<MouseWheelEventArgs>(MouseWheel);
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

		internal static void MouseDown(object sender, MouseButtonEventArgs mouse)
		{
			if (mouseButtons.ContainsKey(mouse.Button))
			{
				mouseButtons[mouse.Button] = true;
			}
			else
			{
				mouseButtons.Add(mouse.Button, true);
			}
		}

		internal static void MouseUp(object sender, MouseButtonEventArgs mouse)
		{
			if (mouseButtons.ContainsKey(mouse.Button))
			{
				mouseButtons[mouse.Button] = false;
			}
			else
			{
				mouseButtons.Add(mouse.Button, false);
			}
		}

		internal static void MouseMove(object sender, MouseMoveEventArgs mouse)
		{
			MousePosition = new Vector2(mouse.X, mouse.Y);
		}

		internal static void MouseWheel(object Sender, MouseWheelEventArgs mouse) {
			WheelPosition += mouse.Delta;
		}

		//Change the mouse position to X,Y on the open window. I have no idea how to do this, figure this out :P
		/// <summary>
		/// Change the mouse position in the current window.
		/// </summary>
		/// <param name="x">X position to set the mouse to.</param>
		/// <param name="y">Y position to set the mouse to.</param>
		public static void SetMousePositionWindows(int x, int y) {
			System.Drawing.Point loc = GraphicsManager.Instance.Location;
			System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x + loc.X, y + loc.Y);
		}

		//When called, hide the mouse cursor
		public static void HideMouseWindows() {
			System.Windows.Forms.Cursor.Hide();
		}

		//When called, show the mouse cursor
		public static void ShowMouseWindows() {
			System.Windows.Forms.Cursor.Show();
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

		//Returns if the mouse button is currently down or not
		public static bool IsDown(MouseButton mb) {
			if (mouseButtons.ContainsKey(mb))
			{
				return mouseButtons[mb];
			}
			else
			{
				return false;
			}
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
		public static bool IsPressed(MouseButton btn) {
			if(prevmouseButtons.ContainsKey(btn) && prevmouseButtons[btn]) { //If it was previously down
				return false;
			} else if(mouseButtons.ContainsKey(btn) && mouseButtons[btn]) { //Previously up & is currently down
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

		public static bool IsReleased(MouseButton btn) {
			if (prevmouseButtons.ContainsKey(btn) && !prevmouseButtons[btn]) { //If it was previously up
				return false;
			} else if (mouseButtons.ContainsKey(btn) && !mouseButtons[btn]) { //Previously down & is currently up
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

		//Return the mouse position
		public static Vector2 GetMousePositionWindows() {
			System.Drawing.Point loc = System.Windows.Forms.Cursor.Position;
			loc.X -= GraphicsManager.Instance.Location.X;
			loc.Y -= GraphicsManager.Instance.Location.Y;
			return new Vector2(loc.X, loc.Y);
			//return MousePosition;
		}

		public static Vector2 GetPreviousMousePosition() {
			return PrevMousePosition;
		}

		//Return the mouse position
		public static Vector2 GetMousePosition() {
			return MousePosition;
		}

		public static int GetMouseWheel() {
			return WheelPosition;
		}
	}
}
