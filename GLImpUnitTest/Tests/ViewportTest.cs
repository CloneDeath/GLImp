using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using System.Drawing;

namespace GLImpUnitTest.Tests {
	class ViewportTest : TestSetup {
		public ViewportTest()
		{
			Name = "Viewport Test";

			Camera2D back = new Camera2D();
			back.OnRender += RenderBack;
			back.SetLocation(0, 0);
			back.Layer = 0;

			Camera2D red = new Camera2D();
			red.OnRender += RenderRed;
			red.SetLocation(0, 0);
			red.Layer = 1;
			red.EnableViewport(500, 500, 100, 100);

			Camera2D blue = new Camera2D();
			blue.OnRender += RenderBlue;
			blue.SetLocation(0, 0);
			blue.Layer = 2;
			blue.EnableViewport(500, 10, 100, 100);
		}

		void RenderBack()
		{
			GraphicsManager.DrawRectangle(0, 0, 1000, 1000, Color.Green);
		}

		void RenderRed()
		{
			GraphicsManager.DrawRectangle(0, 0, 1000, 1000, Color.Red);
		}

		void RenderBlue()
		{
			GraphicsManager.DrawRectangle(0, 0, 1000, 1000, Color.Blue);
		}
	}
}
