using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace GLImp {
	public class TextureFrame {
		float TilesHorizontal = 1;
		float TilesVertical = 1;
		int Xat = 0;
		int Yat = 0;
		Texture texture;

		private float Left {
			get {
				return (Xat * 1.0f) / TilesHorizontal;
			}
		}

		private float Right {
			get {
				return ((Xat + 1) * 1.0f) / TilesHorizontal;
			}
		}

		private float Top {
			get {
				return (Yat * 1.0f) / TilesVertical;
			}
		}

		private float Bottom {
			get {
				return ((Yat + 1) * 1.0f) / TilesVertical;
			}
		}

		public TextureFrame(Texture tex, int Horizontal, int Vertical, int X, int Y) {
			TilesHorizontal = Horizontal;
			TilesVertical = Vertical;

			texture = tex;

			Xat = X;
			Yat = Y;
		}

		public void Draw(Vector2 p1, Vector2 p2){
			GraphicsManager.SetColor(Color.White);
			GL.BindTexture(TextureTarget.Texture2D, texture.ID);
			GL.Begin(BeginMode.Quads);
				GL.TexCoord2(Left,	Top);	GL.Vertex2(p1.X, p1.Y);
				GL.TexCoord2(Right, Top);	GL.Vertex2(p2.X, p1.Y);
				GL.TexCoord2(Right, Bottom);GL.Vertex2(p2.X, p2.Y);
				GL.TexCoord2(Left,	Bottom);GL.Vertex2(p1.X, p2.Y);
			GL.End();
		}
	}
}
