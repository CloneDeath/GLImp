using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using GLImp;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

namespace GLImpUnitTest
{
	public class Camera {
		public Vector2 Position;
		public float Zoom;
		public float PanSpeed;
		//float Rotation; //MAAAAAAAYBE? lulz, this would be weird

		public Camera() {
			Position = new Vector2(440, 270);
			Zoom = 0.7f;
			PanSpeed = 5.0f;
		}

		public void Update() {
			float speed = PanSpeed;//* Zoom;
			
			if(KeyboardManager.IsDown(MouseButton.Middle)) {
				Position -= (KeyboardManager.GetMousePosition() - KeyboardManager.GetPreviousMousePosition())/Zoom;
			}

			if(KeyboardManager.IsDown(Key.Left)) {
				Position.X -= speed;
			}
			if(KeyboardManager.IsDown(Key.Right)) {
				Position.X += speed;
			}
			if(KeyboardManager.IsDown(Key.Up)) {
				Position.Y -= speed;
			}
			if(KeyboardManager.IsDown(Key.Down)) {
				Position.Y += speed;
			}

			Zoom += KeyboardManager.GetMouseWheel() * 0.1f;
			if(Zoom < 0.5f) {
				Zoom = 0.5f;
			}

			if(Zoom > 3f) {
				Zoom = 3f;
			}
		}

		public void Draw() {
			GL.Scale(Zoom, Zoom, 1);
			GL.Translate((GraphicsManager.WindowWidth / 2) / Zoom, (GraphicsManager.WindowHeight / 2) / Zoom, 0);
			GL.Translate(-Position.X, -Position.Y, 0);
		}
	}
}
