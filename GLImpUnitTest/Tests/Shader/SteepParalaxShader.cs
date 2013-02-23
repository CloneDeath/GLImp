using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using System.IO;
using OpenTK;

namespace GLImpUnitTest.Tests {
	class SteepParalaxShader : Shader {
		Texture Tex;
		Texture Bump;

		Texture texture {
			set {
				this.SetUniform("base", value, 1);
			}
		}

		Texture map {
			set {
				this.SetUniform("map", value, 2);
			}
		}

		public Vector3 ambientColor {
			set {
				this.SetUniform("ambientColor", value);
			}
		}
		public Vector3 diffuseColor {
			set {
				this.SetUniform("diffuseColor", value);
			}
		}
		public Vector3 specularColor {
			set {
				this.SetUniform("specularColor", value);
			}
		}



		public float shininess {
			set {
				this.SetUniform("shininess", value);
			}
		}
		public float scale {
			set {
				this.SetUniform("scale", value);
			}
		}
		public float shadow {
			set {
				this.SetUniform("shadow", value);
			}
		}

		public SteepParalaxShader() : base(File.ReadAllText(@"media\LEDShader\steepparalax.vert"), File.ReadAllText(@"media\LEDShader\steepparalax.frag")){
			Tex = new Texture(@"media\LEDShader\collage.jpg");
			Bump = new Texture(@"media\LEDShader\collage-bump.jpg");
		}

		public void SetUniforms(Vector3 pos) {
			texture = Tex;
			map = Bump;

			ambientColor = new Vector3(1, 1, 1);
			diffuseColor = new Vector3(1, 1, 1);
			specularColor = new Vector3(1, 1, 1);

			shininess = 1;
			scale = 1;
			shadow = 1;
		}
	}
}
