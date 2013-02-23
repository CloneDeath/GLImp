using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using System.IO;

namespace GLImpUnitTest.Tests {
	class LEDShader : Shader {
		Texture Tex;

		float bright {
			set {
				this.SetUniform("bright", value);
			}
		}

		float ledScale {
			set {
				this.SetUniform("ledScale", value);
			}
		}

		Texture cmap {
			set {
				this.SetUniform("cmap", value, 1);
			}
		}

		public LEDShader() : base(File.ReadAllText(@"media\LEDShader\billboard1.vert"), File.ReadAllText(@"media\LEDShader\LED.frag")){
			Tex = new Texture(@"media\LEDShader\laughing_man.jpg");
		}

		public void SetUniforms() {
			bright = 2.0f;
			ledScale = 125;
			cmap = Tex;
		}
	}
}
