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

        public override void Render2DPre()
        {
			GraphicsManager.DrawRectangle(0, 0, 40, 40, Color.White);
			//Red Tests
			GraphicsManager.DrawRectangle(10, 0, 10, 10, Color.FromArgb(0, 255, 0, 0)); //Should be white square
			GraphicsManager.DrawRectangle(20, 0, 10, 10, Color.FromArgb(127, 255, 0, 0)); //Should be pink square
			GraphicsManager.DrawRectangle(30, 0, 10, 10, Color.FromArgb(255, 255, 0, 0)); //Should be red square

			//Black Tests
			GraphicsManager.DrawRectangle(10, 10, 10, 10, Color.FromArgb(0, 0, 0, 0)); //Should be white square
			GraphicsManager.DrawRectangle(20, 10, 10, 10, Color.FromArgb(127, 0, 0, 0)); //Should be gray square
			GraphicsManager.DrawRectangle(30, 10, 10, 10, Color.FromArgb(255, 0, 0, 0)); //Should be black square

			GraphicsManager.SetColor(Color.White);
        }
	}
}
