using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GLImpUnitTest.Tests {
	class LEDShader : Shader {
		public Texture Tex;

		int PixelSize {
			set {
				this.SetUniform("pixelSize", value);
			}
		}
		Vector2 billboardSize {
			set {
				int v1 = (int)value.X;
				int v2 = (int)value.Y;
				this.SetUniform("billboardSize", v1, v2);
			}
		}
		Texture billboardTexture {
			set {
				this.SetUniform("billboardTexture", value);
			}
		}
		

		public LEDShader() : base(File.ReadAllText(@"media\LEDShader\billboard1.vert"), File.ReadAllText(@"media\LEDShader\billboard1.frag")){
			PixelSize = 5;
			Tex = new Texture(@"media\LEDShader\shodan.jpg");
			billboardSize = new Vector2(Tex.Width, Tex.Height);
			billboardTexture = Tex;
		}
	}
}
