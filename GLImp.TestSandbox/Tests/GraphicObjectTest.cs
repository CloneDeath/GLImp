using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using System.Drawing;
using GLImp.GraphicObjects;
using GLImp.Input;
using OpenTK;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GLImpUnitTest.Tests
{
    public class GraphicObjectTest : TestSetup
    {
		Vector3 Pos = new Vector3(0, 3, 0);
		Vector2 Lookat = new Vector2(0, 0);

		GraphicObject testobj = new GraphicObject(new Cube());
		public GraphicObjectTest() {
			Name = "Graphic Object Test";
		}

		public override void Render3D() {
			Program.Camera3D.Position = new Vector3d(Pos.X, Pos.Y, Pos.Z);
			Program.Camera3D.LookAt(new Vector3d(Lookat.X, Lookat.Y, 0));

			GraphicsManager.PushMatrix();
			{
				testobj.Draw();
			}
			GraphicsManager.PopMatrix();
		}

		public override void Update() {
			float speed = 0.1f;
			if (KeyboardManager.IsDown(Keys.W)) {
				Pos.Y -= speed;
			}
			if (KeyboardManager.IsDown(Keys.S)) {
				Pos.Y += speed;
			}
			if (KeyboardManager.IsDown(Keys.A)) {
				Pos.X += speed;
			}
			if (KeyboardManager.IsDown(Keys.D)) {
				Pos.X -= speed;
			}
			if (KeyboardManager.IsDown(Keys.LeftShift)) {
				Pos.Z -= speed;
			}
			if (KeyboardManager.IsDown(Keys.Space)) {
				Pos.Z += speed;
			}

			if (KeyboardManager.IsDown(Keys.Up)) {
				Lookat.Y -= speed;
			}
			if (KeyboardManager.IsDown(Keys.Down)) {
				Lookat.Y += speed;
			}
			if (KeyboardManager.IsDown(Keys.Left)) {
				Lookat.X += speed;
			}
			if (KeyboardManager.IsDown(Keys.Right)) {
				Lookat.X -= speed;
			}
		}
    }
}
