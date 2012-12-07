﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using GLImp;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

namespace GLImp {
	public class Camera2D {
		internal static Vector2 Position;
		internal static float Zoom;
		internal static bool Centered = false;
		//internal static float Rotation; //TODO nicholas implement this when we need it

		static Camera2D() {
			Position = new Vector2(0, 0);
			Zoom = 1f;
		}

		internal static void Draw() {
			GL.Scale(Zoom, Zoom, 1);
			if (Centered) {
				GL.Translate((GraphicsManager.WindowWidth / 2) / Zoom, (GraphicsManager.WindowHeight / 2) / Zoom, 0);
			}
			GL.Translate(-Position.X, -Position.Y, 0);
		}

		public static void SetLocation(float X, float Y) {
			Position = new Vector2(X, Y);
		}

		/// <summary>
		/// Set to true if the location of the camera is the center of the screen. Set it to false for the location to be the top right.
		/// </summary>
		/// <param name="c"></param>
		public static void CenterOnTarget(bool c) {
			Centered = c;
		}

		public static void SetZoom(float Zoom) {
			Camera2D.Zoom = Zoom;
		}
	}
}