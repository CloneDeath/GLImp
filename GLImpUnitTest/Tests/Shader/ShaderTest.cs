using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using OpenTK;

namespace GLImpUnitTest.Tests {
	class ShaderTest : TestSetup {
		LEDShader Shader = new LEDShader();
		public ShaderTest() {
			Name = "Shader Test";
		}

		public override void Render3D() {
			GraphicsManager.SetTexture(Shader.Tex);
			Shader.Activate();
			
			GraphicsManager.DrawQuad(new Vector3d(-1, 0, -1),
									new Vector3d(-1, 0, 1),
									new Vector3d(1, 0, 1),
									new Vector3d(1, 0, -1));

			GraphicsManager.SetCamera(new Vector3d(1, 3, 0));
			GraphicsManager.SetLookAt(new Vector3d(0, 0, 0));
			Shader.Deactivate();
		}
	}
}
