using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Input;

namespace GLImp {
	public class MouseManager {
		private static Vector2d MousePos;
		private static Vector2d PrevMousePosition;

		private static Dictionary<MouseButton, bool> prevmouseButtons = new Dictionary<MouseButton, bool>();
		private static Dictionary<MouseButton, bool> mouseButtons = new Dictionary<MouseButton, bool>();

		private static int WheelPosition;

		internal static void Init() {
			GraphicsManager.mouse.ButtonDown += new EventHandler<MouseButtonEventArgs>(MouseDown);
			GraphicsManager.mouse.ButtonUp += new EventHandler<MouseButtonEventArgs>(MouseUp);
			GraphicsManager.mouse.Move += new EventHandler<MouseMoveEventArgs>(MouseMove);
			GraphicsManager.mouse.WheelChanged += new EventHandler<MouseWheelEventArgs>(MouseWheel);
		}

		internal static void Update() {
			prevmouseButtons = new Dictionary<MouseButton, bool>();
			foreach (KeyValuePair<MouseButton, bool> kvp in mouseButtons) {
				prevmouseButtons.Add(kvp.Key, kvp.Value);
			}

			WheelPosition = 0;

			PrevMousePosition = MousePos;
		}

		internal static void MouseDown(object sender, MouseButtonEventArgs mouse) {
			if (mouseButtons.ContainsKey(mouse.Button)) {
				mouseButtons[mouse.Button] = true;
			} else {
				mouseButtons.Add(mouse.Button, true);
			}
		}

		internal static void MouseUp(object sender, MouseButtonEventArgs mouse) {
			if (mouseButtons.ContainsKey(mouse.Button)) {
				mouseButtons[mouse.Button] = false;
			} else {
				mouseButtons.Add(mouse.Button, false);
			}
		}

		internal static void MouseMove(object sender, MouseMoveEventArgs mouse) {
			MousePos = new Vector2d(mouse.X, mouse.Y);
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

		//Returns if the mouse button is currently down or not
		public static bool IsDown(MouseButton mb) {
			if (mouseButtons.ContainsKey(mb)) {
				return mouseButtons[mb];
			} else {
				return false;
			}
		}

		public static bool IsPressed(MouseButton btn) {
			if (prevmouseButtons.ContainsKey(btn) && prevmouseButtons[btn]) { //If it was previously down
				return false;
			} else if (mouseButtons.ContainsKey(btn) && mouseButtons[btn]) { //Previously up & is currently down
				return true;
			} else { //The key is currently not down
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

		/// <summary>
		/// Returns the mouse position for windows machines, which is relative to the top left of the window.
		/// </summary>
		/// <returns></returns>
		public static Vector2d GetMousePositionWindows() {
			System.Drawing.Point loc = System.Windows.Forms.Cursor.Position;
			loc.X -= GraphicsManager.Instance.Location.X;
			loc.Y -= GraphicsManager.Instance.Location.Y;
			return new Vector2d(loc.X, loc.Y);
		}

		/// <summary>
		/// Returns the previous mouse position. Mostly used for trackign how far the mouse has moved since last update.
		/// </summary>
		/// <returns></returns>
		public static Vector2d PreviousMousePosition {
			get {
				return PrevMousePosition;
			}
		}

		/// <summary>
		/// Returns the mouse position relative to the top left corner of the viewable area, in pixels.
		/// </summary>
		/// <returns></returns>
		public static Vector2d MousePosition {
			get
			{
				return MousePos;
			}
		}

		/// <summary>
		/// Gets the numbler of clicks (positive for up, negative for down) the scroll wheel has moved since last update.
		/// </summary>
		/// <returns>How far the mouse wheel has been scrolled.</returns>
		public static int GetMouseWheel() {
			return WheelPosition;
		}
	}
}
