using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using OpenTK;
using System.Drawing;
using GLImp.Input;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GLImpUnitTest.Tests {
	class InsideCubeTest : TestSetup {
		double X, Y, pitch, yaw;
		public InsideCubeTest() {
			Name = "Inside Cube 3D Test";
			X = 0.5;
			Y = 0.5;
			pitch = 0;
			yaw = 0;
		}

		public override void Render3D() {
			GraphicsManager.DrawQuad(new Vector3d(0, 0, 0),
									new Vector3d(0, 0, 1),
									new Vector3d(1, 0, 1),
									new Vector3d(1, 0, 0), Color.Red);
			GraphicsManager.DrawQuad(new Vector3d(0, 1, 0),
									new Vector3d(0, 1, 1),
									new Vector3d(1, 1, 1),
									new Vector3d(1, 1, 0), Color.Green);
			GraphicsManager.DrawQuad(new Vector3d(0, 0, 0),
									new Vector3d(0, 0, 1),
									new Vector3d(0, 1, 1),
									new Vector3d(0, 1, 0), Color.Blue);
			GraphicsManager.DrawQuad(new Vector3d(1, 0, 0),
									new Vector3d(1, 0, 1),
									new Vector3d(1, 1, 1),
									new Vector3d(1, 1, 0), Color.Yellow);
			GraphicsManager.DrawQuad(new Vector3d(1, 0, 0),
									new Vector3d(1, 1, 0),
									new Vector3d(0, 1, 0),
									new Vector3d(0, 0, 0), Color.Purple);
			GraphicsManager.DrawQuad(new Vector3d(1, 0, 1),
									new Vector3d(1, 1, 1),
									new Vector3d(0, 1, 1),
									new Vector3d(0, 0, 1), Color.Orange);

			Program.Camera3D.Position = new Vector3d(0.5, 0.5, 0.5);

			if (KeyboardManager.IsDown(Keys.Up)) {
				pitch += 10;
			}
			if (KeyboardManager.IsDown(Keys.Down)) {
				pitch -= 10;
			}
			if (KeyboardManager.IsDown(Keys.Left)) {
				yaw += 10;
			}
			if (KeyboardManager.IsDown(Keys.Right)) {
				yaw -= 10;
			}
			Program.Camera3D.RotateCamera(yaw, pitch);
			//GraphicsManager.SetLookAt(new Vector3d(0, 0, 0));
		}
	}
}
