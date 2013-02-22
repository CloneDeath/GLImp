using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GLImpUnitTest.Tests {
	class RadialBlurShader : Shader {
		public Texture Tex;

		Vector2 resolution {
			set {
				this.SetUniform("resolution", value);
			}
		}

		float time {
			set {
				this.SetUniform("time", value);
			}
		}

		Texture tex0 {
			set {
				this.SetUniform("tex0", value, 2);
			}
		}

		public RadialBlurShader() : base(File.ReadAllText(@"media\LEDShader\billboard1.vert"), File.ReadAllText(@"media\LEDShader\radialblur.frag")){
			Tex = new Texture(@"media\LEDShader\laughing_man.jpg");
		}

		float time_var = 0;
		public void SetUniforms(){
			time = time_var;
			time_var += 0.01f;
			tex0 = Tex;
			resolution = new Vector2(GraphicsManager.WindowWidth, GraphicsManager.WindowHeight);
		}
	}
}
