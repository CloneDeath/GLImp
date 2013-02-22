using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace GLImpUnitTest.Tests {
	class ShaderTest : TestSetup {
		RadialBlurShader Shader = new RadialBlurShader();
		public ShaderTest() {
			Name = "Shader Test";

		}

		public override void Render3D() {
			GraphicsManager.SetCamera(new Vector3d(0, 3, 0));
			GraphicsManager.SetLookAt(new Vector3d(0, 0, 0));

			Shader.SetUniforms();
			Shader.Activate();
			//GraphicsManager.SetTexture(Shader.Tex);
			//GraphicsManager.SetColor(Color.White);
			GraphicsManager.DrawQuad(new Vector3d(-1, 0, 1),
									new Vector3d(1, 0, 1),
									new Vector3d(1, 0, -1),
									new Vector3d(-1, 0, -1));

			
			Shader.Deactivate();
		}
	}
}
