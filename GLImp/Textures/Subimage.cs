using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace GLImp {
	public class Subimage : Image {
		Texture texture;
		int Xoffset;
		int Yoffset;
		public int Width;
		public int Height;

		public Subimage(Texture tex, int x, int y, int Width, int Height) {
			this.texture = tex;
			this.Width = Width;
			this.Height = Height;
			this.Xoffset = x;
			this.Yoffset = y;
		}

		private float Left {
			get {
				return (Xoffset * 1.0f) / texture.Width;
			}
		}

		private float Right {
			get {
				return ((Xoffset + 1) * 1.0f) / texture.Width;
			}
		}

		private float Top {
			get {
				return (Yoffset * 1.0f) / texture.Height;
			}
		}

		private float Bottom {
			get {
				return ((Yoffset + 1) * 1.0f) / texture.Height;
			}
		}

		public override void Draw(float x, float y) {
			this.Draw(x, y, this.Width, this.Height);
		}

		public override void Draw(float X, float Y, float Width, float Height) {
			float X1 = X;
			float Y1 = Y;
			float X2 = X + Width;
			float Y2 = Y + Height;
			GraphicsManager.SetColor(Color.White);
			GL.BindTexture(TextureTarget.Texture2D, texture.ID);
			GL.Begin(BeginMode.Quads);
				GL.TexCoord2(Left,	Top);	GL.Vertex2(X1, Y1);
				GL.TexCoord2(Right, Top);	GL.Vertex2(X2, Y1);
				GL.TexCoord2(Right, Bottom);GL.Vertex2(X2, Y2);
				GL.TexCoord2(Left, Bottom); GL.Vertex2(X1, Y2);
			GL.End();
		}

		
	}
}
