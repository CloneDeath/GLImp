using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using OpenTK;
using System.Drawing;
using OpenTK.Input;

namespace GLImpUnitTest.Tests {
	class Line3DTest : TestSetup {
		public Line3DTest() {
			Name = "Line 3D Test";
		}

		public override void Render3D() {
			GraphicsManager.DrawLine(new Vector3d(0, 0, 0),
									new Vector3d(1, 0, 1), Color.Red);

			GraphicsManager.DrawLine(new Vector3d(0, 0, 1),
									new Vector3d(1, 0, 0), Color.Red);
			
			GraphicsManager.SetCamera(new Vector3d(0.5, 1, 0.5));
			GraphicsManager.SetLookAt(new Vector3d(0.5, 0, 0.5));
		}
	}
}
