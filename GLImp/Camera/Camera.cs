using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GLImp
{
	public abstract class Camera
	{
		public int Layer;
		public Action OnRender;

		internal bool FillWindow = true;
		internal Rectangle ViewportArea = new Rectangle(0, 0, GraphicsManager.WindowWidth, GraphicsManager.WindowHeight);

		internal abstract void Draw();

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
		/// Enables viewport if it is not already.
		/// </summary>
		/// <param name="x">Pixels from the left of the draw area.</param>
		/// <param name="y">Pixels from the top of the draw area.</param>
		/// <param name="width">Width of the camera in pixels.</param>
		/// <param name="height">Height of the camera in pixels.</param>
		public void SetViewport(int x, int y, int width, int height)
		{
			FillWindow = false;
			ViewportArea = new Rectangle(x, y, width, height);
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
