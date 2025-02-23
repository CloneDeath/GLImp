using System;
using OpenTK.Input;
using System.Drawing;
using GLImp;
using OpenTK;
using Gwen.Control;
using Gwen;
using System.Collections.Generic;
using OpenTK.Windowing.Common;

namespace GLImpUnitTest
{
	/// <summary>
	/// Demonstrates the GameWindow class.
	/// </summary>
	public class Program {
		static Camera2D Camera2D = new Camera2D();
		public static Camera3D Camera3D = new Camera3D();

		/// <summary>
		/// Entry point of this example.
		/// </summary>
		[STAThreadAttribute]
		public static void Main() {
			GraphicsManager.UseExperimentalFullAlpha = true;
			GraphicsManager.SetResolution(800, 600);
			GraphicsManager.SetBackground(Color.Black);
			Initialize();
			Camera2D.OnRender += Render2D;
			Camera3D.OnRender += Render3D;
			Camera3D.Layer = 100;
			Camera2D.Layer = 1;
			GraphicsManager.Update += Update;
			GraphicsManager.SetTitle("GLImp Unit Tests");
			GraphicsManager.Start();
			MainCanvas.Dispose();
		}



        public static TestingDisplay Setup;

		static void Initialize() {
            Camera2D.SetLocation(0, 200);
			Camera2D.CenterOnTarget(true);
            Setup = new TestingDisplay();
		}

		static void Update(FrameEventArgs e)
		{
			Setup.Update();
		}

		static void Render2D(FrameEventArgs e) {
            Setup.Render2D();
		}

		static void Render3D(FrameEventArgs e)
		{
			Setup.Render3D();
		}
	}
}
