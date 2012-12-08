﻿using System;
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
		/// <summary>
		/// Entry point of this example.
		/// </summary>
		[STAThreadAttribute]
		public static void Main() {
			GraphicsManager.SetResolution(800, 600);
			GraphicsManager.SetBackground(Color.Black);
			Initialize();
			GraphicsManager.Render2DPre += new GraphicsManager.Renderer(Render2DPre);
			GraphicsManager.Update += new GraphicsManager.Updater(Update);
			GraphicsManager.SetTitle("GLImp Unit Tests");
			GraphicsManager.OpenWindow();
			MainCanvas.Dispose();
		}

        public static TestingDisplay Setup;

		static void Initialize() {
            Camera2D.SetLocation(-70, 0);
			Camera2D.CenterOnTarget(true);
            Setup = new TestingDisplay();
		}

		static void Update() {
		}

		static void Render2DPre() {
			GraphicsManager.PushMatrix();

            Setup.Render2DPre();
			
			GraphicsManager.PopMatrix();
		}
	}
}
