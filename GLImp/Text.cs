using System;
using System.Text;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GLImp
{
	class Text
	{
		public static void DrawString(int x, int y, String text, Color color)
		{
			GL.PushMatrix();
			GL.Translate(new Vector3(x, y, 0));
			GL.Scale(new Vector3(16, 16, 1));

			if (text != null)
			{
				GL.BindTexture(TextureTarget.Texture2D, Texture.Font);
				for (int i = 0; i < text.Length; i++)
				{
					DrawChar(text[i]);
					GL.Translate(1f, 0.0f, 0.0f);
				}
			}
			GL.PopMatrix();
		}

		private static void DrawChar(int charAt)
		{
			int xSize = 16;
			int ySize = 16;
			int c = charAt - ' ';

			int cx = c % xSize;
			int cy = c / xSize;
			float top = (cy) * (1.0f / ySize);
			float bottom = (cy + 1) * (1.0f / ySize);
			float right = (cx + 1) * (1.0f / xSize);
			float left = (cx) * (1.0f / xSize);
			GL.Begin(BeginMode.Quads);
				GL.TexCoord2(new Vector2d(top, left)); GL.Vertex2(new Vector2d(0.0f, 0.0f));
				GL.TexCoord2(new Vector2d(bottom, left)); GL.Vertex2(new Vector2d(0.0f, 1.0f));
				GL.TexCoord2(new Vector2d(bottom, right)); GL.Vertex2(new Vector2d(1.0f, 1.0f));
				GL.TexCoord2(new Vector2d(top, right)); GL.Vertex2(new Vector2d(1.0f, 0.0f));
			GL.End();
		}
	}
}
