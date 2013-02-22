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
				this.SetUniform("tex0", 2);
			}
		}

		public RadialBlurShader() : base(File.ReadAllText(@"media\LEDShader\billboard4.vert"), File.ReadAllText(@"media\LEDShader\billboard4.frag")){
			Tex = new Texture(@"media\LEDShader\shodan.jpg");
		}

		public void SetUniforms(){
			time = 0f;
			tex0 = Tex;
			resolution = new Vector2(Tex.Width, Tex.Height);
		}
	}
}
