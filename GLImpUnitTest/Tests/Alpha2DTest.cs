using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using System.Drawing;

namespace GLImpUnitTest.Tests {
	class Alpha2DTest : TestSetup {
		public Alpha2DTest() {
			Name = "Alpha 2D Test";
		}

        public override void Render2D()
        {
			GraphicsManager.UseExperimentalFullAlpha = true;
			GraphicsManager.DrawRectangle(5, 0, 35, 40, Color.White);
			//Red Tests
			GraphicsManager.DrawRectangle(10, 0, 10, 10, Color.FromArgb(0, 255, 0, 0)); //Should be white square
			GraphicsManager.DrawRectangle(20, 0, 10, 10, Color.FromArgb(127, 255, 0, 0)); //Should be pink square
			GraphicsManager.DrawRectangle(30, 0, 10, 10, Color.FromArgb(255, 255, 0, 0)); //Should be red square

			//Green Tests
			GraphicsManager.DrawRectangle(10, 10, 10, 10, Color.FromArgb(0, 0, 255, 0)); //Should be white square
			GraphicsManager.DrawRectangle(20, 10, 10, 10, Color.FromArgb(127, 0, 255, 0)); //Should be light green
			GraphicsManager.DrawRectangle(30, 10, 10, 10, Color.FromArgb(255, 0, 255, 0)); //Should be green square

			//Blue Tests
			GraphicsManager.DrawRectangle(10, 20, 10, 10, Color.FromArgb(0, 0, 0, 255)); //Should be white square
			GraphicsManager.DrawRectangle(20, 20, 10, 10, Color.FromArgb(127, 0, 0, 255)); //Should be light blue square
			GraphicsManager.DrawRectangle(30, 20, 10, 10, Color.FromArgb(255, 0, 0, 255)); //Should be blue square

			//Black Tests
			GraphicsManager.DrawRectangle(10, 30, 10, 10, Color.FromArgb(0, 0, 0, 0)); //Should be white square
			GraphicsManager.DrawRectangle(20, 30, 10, 10, Color.FromArgb(127, 0, 0, 0)); //Should be gray square
			GraphicsManager.DrawRectangle(30, 30, 10, 10, Color.FromArgb(255, 0, 0, 0)); //Should be black square

			GraphicsManager.SetColor(Color.White);
        }
	}
}
