using System;
using OpenTK.Input;
using System.Drawing;
using GLImp;
using OpenTK;
using Gwen.Control;
using Gwen;
using System.Collections.Generic;

namespace GLImpUnitTest
{
	/// <summary>
	/// Demonstrates the GameWindow class.
	/// </summary>
	public class Program {
		static Camera2D Camera = new Camera2D();

		/// <summary>
		/// Entry point of this example.
		/// </summary>
		[STAThreadAttribute]
		public static void Main() {
			GraphicsManager.UseExperimentalFullAlpha = true;
			GraphicsManager.SetResolution(800, 600);
			GraphicsManager.SetBackground(Color.Black);
			Initialize();
			Camera.OnRender += new GraphicsManager.Renderer(Render2D);
			GraphicsManager.Render += new GraphicsManager.Renderer(Render3D);
			GraphicsManager.Update += new GraphicsManager.Updater(Update);
			GraphicsManager.SetTitle("GLImp Unit Tests");
			GraphicsManager.OpenWindow();
			MainCanvas.Dispose();
		}

		

        public static TestingDisplay Setup;

		static void Initialize() {
            Camera.SetLocation(0, 200);
			Camera.CenterOnTarget(true);
            Setup = new TestingDisplay();
		}

		static void Update() {
		}

		static void Render2D() {
            Setup.Render2D();
		}

		static void Render3D() {
			Setup.Render3D();
		}
	}
}
