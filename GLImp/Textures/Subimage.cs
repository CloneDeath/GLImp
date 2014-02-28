using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace GLImp {
	public class SubImage : Image {
		Texture texture;
		int Xoffset;
		int Yoffset;

		public SubImage(Texture tex, int x, int y, int Width, int Height) {
			this.texture = tex;
			this.Width = Width;
			this.Height = Height;
			this.Xoffset = x;
			this.Yoffset = y;
		}

		private double Left {
			get {
				return (Xoffset * 1.0) / texture.Width;
			}
		}

		private double Right {
			get {
				return ((Xoffset + this.Width) * 1.0) / texture.Width;
			}
		}

		private double Top {
			get {
				return (Yoffset * 1.0) / texture.Height;
			}
		}

		private double Bottom {
			get {
				return ((Yoffset + this.Height) * 1.0) / texture.Height;
			}
		}

		public override void Draw(double x, double y) {
			this.Draw(x, y, false);
		}

		public void Draw(double x, double y, bool Flip) {
			this.Draw(x, y, this.Width, this.Height, Flip);
		}

		public override void Draw(double X, double Y, double Width, double Height) {
			this.Draw(X, Y, Width, Height, false);
		}

		public void Draw(double X, double Y, double Width, double Height, bool Flip) {
			double X1 = X;
			double Y1 = Y;
			double X2 = X + Width;
			double Y2 = Y + Height;
			GraphicsManager.SetColor(Color.White);
			GL.BindTexture(TextureTarget.Texture2D, texture.ID);
			if (!Flip){
				GL.Begin(PrimitiveType.Quads);
					GL.TexCoord2(Left,	Top);	GL.Vertex2(X1, Y1);
					GL.TexCoord2(Right, Top);	GL.Vertex2(X2, Y1);
					GL.TexCoord2(Right, Bottom);GL.Vertex2(X2, Y2);
					GL.TexCoord2(Left, Bottom); GL.Vertex2(X1, Y2);
				GL.End();
			} else {
				GL.Begin(PrimitiveType.Quads);
					GL.TexCoord2(Right,	Top);	GL.Vertex2(X1, Y1);
					GL.TexCoord2(Left, Top);	GL.Vertex2(X2, Y1);
					GL.TexCoord2(Left, Bottom);GL.Vertex2(X2, Y2);
					GL.TexCoord2(Right, Bottom); GL.Vertex2(X1, Y2);
				GL.End();
			}
		}

		
	}
}
