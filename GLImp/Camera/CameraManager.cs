using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace GLImp
{
	internal class CameraManager
	{
		private static List<Camera> Cameras = new List<Camera>();

		public static void Add(Camera camera)
		{
			if (!Cameras.Contains(camera)) {
				Cameras.Add(camera);
			}
		}

		public static void Remove(Camera camera)
		{
			if (Cameras.Contains(camera)) {
				Cameras.Remove(camera);
			}
		}

		private static void SortCameras()
		{
			//Make sure we have at least one camera to sort first.
			if (Cameras.Count == 0) {
				return;
			}

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
			List<Camera> newlist = new List<Camera>();
			foreach (Camera camera in Cameras) {
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

		internal static void Draw()
		{
			SortCameras();

			foreach (Camera camera in Cameras) {
				if (camera.FillWindow) {
					GL.Viewport(0, 0, GraphicsManager.WindowWidth, GraphicsManager.WindowHeight);
				} else {
					GL.Viewport(camera.ViewportArea.X, GraphicsManager.WindowHeight - camera.ViewportArea.Y, camera.ViewportArea.Width, camera.ViewportArea.Height);
				}
				GL.Clear(ClearBufferMask.DepthBufferBit);
				camera.Draw();
			}
		}
	}
}
