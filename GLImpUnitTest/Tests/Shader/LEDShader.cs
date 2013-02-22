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
		public Texture Noise;

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
			//Tex = new Texture(@"media\LEDShader\shodan.jpg");
			Tex = new Texture(@"media\LEDShader\flagcross.bmp");
			Noise = new Texture(@"media\LEDShader\noise.bmp");
		}

		public void SetUniforms() {
			PixelSize = 6;
			pixelRadius = 0.5f;
			tolerance = 0.4f;
			luminanceBoost = 0.0f;
			luminanceSteps = 20;
			colorBoost = 0.025f;
			burntOutPercent = 0.2f;

			billboardSize = new Vector2(Tex.Width, Tex.Height);
			billboardTexture = Tex;
			
			
			
			
			
			
			noiseTexture = Noise;
		}
	}
}
