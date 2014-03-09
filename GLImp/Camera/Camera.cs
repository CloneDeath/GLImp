using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;

namespace GLImp
{
	/// <summary>
	/// Base class for all Camera Types.
	/// </summary>
	public abstract class Camera
	{
		/// <summary>
		/// Layer used for ordering cameras. Higher layers are drawn on top of lower layers.
		/// </summary>
		public int Layer;

		/// <summary>
		/// The OnRender event will be triggered whenever the camera is ready to start drawing.
		/// </summary>
		public Action<FrameEventArgs> OnRender;

		private bool FillWindow = true;
		private Rectangle ViewportArea = new Rectangle(0, 0, GraphicsManager.WindowWidth, GraphicsManager.WindowHeight);

		internal abstract void Draw(FrameEventArgs e);

		/// <summary>
		/// Disables viewports. This camera will fill up the window.
		/// </summary>
		public void DisableViewport()
		{
			FillWindow = true;
		}

		/// <summary>
		/// Enables viewports.
		/// </summary>
		/// <param name="x">Pixels from the left of the draw area.</param>
		/// <param name="y">Pixels from the top of the draw area.</param>
		/// <param name="width">Width of the camera in pixels.</param>
		/// <param name="height">Height of the camera in pixels.</param>
		public void EnableViewport(int x, int y, int width, int height)
		{
			FillWindow = false;
			ViewportArea = new Rectangle(x, y, width, height);
		}

		/// <summary>
		/// Returns the current viewport area. Setting this will enable viewports.
		/// </summary>
		public Rectangle Viewport
		{
			get
			{
				if (FillWindow) {
					return new Rectangle(0, 0, GraphicsManager.WindowWidth, GraphicsManager.WindowHeight);
				} else {
					return ViewportArea;
				}
			}
			set
			{
				FillWindow = false;
				ViewportArea = value;
			}
		}

		/// <summary>
		/// Disables this camera from being drawn.
		/// </summary>
		public void Disable()
		{
			CameraManager.Remove(this);
		}

		/// <summary>
		/// Re-Enables a disabled camera. All created cameras are enabled by default.
		/// </summary>
		public void Enable()
		{
			CameraManager.Add(this);
		}
	}
}
