using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using GLImp;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

namespace GLImp {
	public class Camera2D {
		#region Static Members and Functions
		internal static List<Camera2D> Cameras = new List<Camera2D>();
		internal static void SortCameras(){
			//Shortcut check, if all cameras are in order, return.
			//todo nicholas instead of checking every time, set/unset a flag everytime cameras is changed, and just check that flag.
			//note: also need to set the flag if "Layer" ever changes for any member.

			int LayerAt = Cameras[0].Layer;
			bool InOrder = true;
			for (int i = 0; i < Cameras.Count; i++) {
				if (Cameras[i].Layer >= LayerAt) {
					LayerAt = Cameras[i].Layer;
				} else {
					InOrder = false;
					break;
				}
			}

			if (InOrder) {
				return;
			}
			
			//Not in order, time to do an insertion sort.
			List<Camera2D> newlist = new List<Camera2D>();
			foreach (Camera2D camera in Cameras) {
				for (int i = 0; i < newlist.Count; i++) {
					if (newlist[i].Layer > camera.Layer) {
						newlist.Insert(i, camera);
						break;
					}
				}

				//Didn't get added, append to end
				if (!newlist.Contains(camera)) {
					newlist.Add(camera);
				}
			}
			Cameras = newlist;
		}
		#endregion

		#region Members
		private Vector2d Position;
		private double Zoom;
		private bool Centered = false;
		//private double Rotation; //TODO nicholas implement this when we need it
		public int Layer;
		#endregion

		#region Constructors
		public Camera2D() 
			: this(0, 0, 1) {
		}

		public Camera2D(double X, double Y)
			: this(X, Y, 1) {
		}

		public Camera2D(double X, double Y, double Zoom) {
			Position = new Vector2d(X, Y);
			this.Zoom = Zoom;
			Layer = 0;

			Cameras.Add(this);
		}
		#endregion

		#region Drawing
		public event GLImp.GraphicsManager.Renderer OnRender;
		internal void Draw() {
			GraphicsManager.PushMatrix();

			GL.Scale(Zoom, Zoom, 1);
			if (Centered) {
				GL.Translate((GraphicsManager.WindowWidth / 2) / Zoom, (GraphicsManager.WindowHeight / 2) / Zoom, 0);
			}
			GL.Translate(-Position.X, -Position.Y, 0);

			if (OnRender != null) {
				OnRender();
			}

			GraphicsManager.PopMatrix();
		}
		#endregion

		#region Properties
		public double X {
			get {
				return Position.X;
			}
			set {
				Position.X = value;
			}
		}

		public double Y {
			get {
				return Position.Y;
			}
			set {
				Position.Y = value;
			}
		}

		public double Width {
			get {
				return GraphicsManager.WindowWidth / Zoom;
			}
		}

		public double Height {
			get {
				return GraphicsManager.WindowHeight / Zoom;
			}
		}

		public double Top {
			get {
				return Y;
			}
		}

		public double Left {
			get {
				return X;
			}
		}

		public double Bottom {
			get {
				return Y + Height;
			}
		}

		public double Right {
			get {
				return X + Width;
			}
		}
		#endregion

		public void SetLocation(double X, double Y) {
			Position = new Vector2d(X, Y);
		}

		/// <summary>
		/// Set to true if the location of the camera is the center of the screen. Set it to false for the location to be the top right.
		/// </summary>
		/// <param name="c"></param>
		public void CenterOnTarget(bool c) {
			Centered = c;
		}

		/// <summary>
		/// Set the magnification of the camera. "2" will make everything twice as big.
		/// </summary>
		/// <param name="Zoom">What you want to set the zoom level to.</param>
		public void SetZoom(double Zoom) {
			this.Zoom = Zoom;
		}

		public double GetZoom() {
			return Zoom;
		}

		/// <summary>
		/// Disables this camera from being drawn.
		/// </summary>
		public void Disable() {
			if (Cameras.Contains(this)) {
				Cameras.Remove(this);
			}
		}

		/// <summary>
		/// Re-Enables a disabled camera. All created cameras are enabled by default.
		/// </summary>
		public void Enable() {
			if (!Cameras.Contains(this)) {
				Cameras.Add(this);
			}
		}
	}
}
