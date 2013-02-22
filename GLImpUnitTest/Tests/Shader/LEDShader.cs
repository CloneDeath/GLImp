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
				this.SetUniform("billboardTexture", value, 0);
			}
		}

		float tolerance {
			set {
				SetUniform("tolerance", value);
			}
		}
		float pixelRadius {
			set {
				SetUniform("pixelRadius", value);
			}
		}
		float luminanceSteps {
			set {
				SetUniform("luminanceSteps", value);
			}
		}
		float luminanceBoost {
			set {
				SetUniform("luminanceBoost", value);
			}
		}
		float colorBoost {
			set {
				SetUniform("colorBoost", value);
			}
		}
		float burntOutPercent {
			set {
				SetUniform("burntOutPercent", value);
			}
		}
		Texture noiseTexture {
			set {
				SetUniform("noiseTexture", value, 1);
			}
		}

		public LEDShader() : base(File.ReadAllText(@"media\LEDShader\billboard4.vert"), File.ReadAllText(@"media\LEDShader\billboard4.frag")){
			PixelSize = 5;
			Tex = new Texture(@"media\LEDShader\shodan.jpg");
			billboardSize = new Vector2(Tex.Width, Tex.Height);
			billboardTexture = Tex;
			tolerance = 1.0f;
			pixelRadius = 0.3f;
			luminanceSteps = 255;
			luminanceBoost = 1.0f;
			colorBoost = 1.0f;
			burntOutPercent = 0.0f;
			noiseTexture = Tex;

		}
	}
}
