using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using GLImp.Input;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GLImpUnitTest.Tests {
	class ShaderTest : TestSetup {
		RadialBlurShader Shader1 = new RadialBlurShader();
		LEDShader Shader2 = new LEDShader();
		//SteepParalaxShader Shader3 = new SteepParalaxShader();

		double Pos = 0;
		double Look = 0;
		double Dist = 0;
		double Pitch = 0;

		public ShaderTest() {
			Name = "Shader Test";

		}

		public override void Render3D() {
			Program.Camera3D.Position = new Vector3d(Pos, Dist - 2, 0);
			Program.Camera3D.LookAt(new Vector3d(Look, Dist, Pitch));

			GraphicsManager.PushMatrix();
			{
				Shader1.Activate();
				{
					Shader1.SetUniforms();
					GraphicsManager.DrawQuad(new Vector3d(-1, 0, 1),
											new Vector3d(1, 0, 1),
											new Vector3d(1, 0, -1),
											new Vector3d(-1, 0, -1));
				}
				Shader1.Deactivate();

				GraphicsManager.Translate(new Vector3d(2.2, 0, 0));

				Shader2.Activate();
				{

					Shader2.SetUniforms();
					GraphicsManager.DrawQuad(new Vector3d(-1, 0, 1),
											new Vector3d(1, 0, 1),
											new Vector3d(1, 0, -1),
											new Vector3d(-1, 0, -1));
				}
				Shader2.Deactivate();

				//GraphicsManager.Translate(new Vector3d(2.2, 0, 0));

				//Shader3.Activate();
				//{

				//    Shader3.SetUniforms((Vector3)camera);
				//    GraphicsManager.DrawQuad(new Vector3d(-1, 0, 1),
				//                            new Vector3d(1, 0, 1),
				//                            new Vector3d(1, 0, -1),
				//                            new Vector3d(-1, 0, -1));
				//}
				//Shader3.Deactivate();
			}
			GraphicsManager.PopMatrix();
		}

		public override void Update() {
			double speed = 0.1;
			if (KeyboardManager.IsDown(Keys.A)) {
				Pos -= speed;
				Look -= speed;
			}
			if (KeyboardManager.IsDown(Keys.D)) {
				Pos += speed;
				Look += speed;
			}
			if (KeyboardManager.IsDown(Keys.Left)) {
				Look -= speed;
			}
			if (KeyboardManager.IsDown(Keys.Right)) {
				Look += speed;
			}

			double DistSpeed = 0.05;
			if (KeyboardManager.IsDown(Keys.W)) {
				Dist += DistSpeed;
			}
			if (KeyboardManager.IsDown(Keys.S)) {
				Dist -= DistSpeed;
			}
			if (Dist > 1.8) Dist = 1.8;

			if (KeyboardManager.IsDown(Keys.Up)) {
				Pitch += DistSpeed;
			}
			if (KeyboardManager.IsDown(Keys.Down)) {
				Pitch -= DistSpeed;
			}
		}
	}
}
