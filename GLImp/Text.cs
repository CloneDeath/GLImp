using System.Drawing;
using GLImp.Textures;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GLImp;

internal class Text {
	public static void DrawString(int x, int y, string text, Color color) {
		GL.PushMatrix();
		GL.Translate(new Vector3d(x, y, 0));
		GL.Scale(new Vector3d(16, 16, 1));

		GL.Color3(color);
		GL.BindTexture(TextureTarget.Texture2D, Texture.Font);
		foreach (var t in text) {
			DrawChar(t);
			GL.Translate(1f, 0.0f, 0.0f);
		}

		GL.PopMatrix();
	}

	private static void DrawChar(int charAt) {
		const int xSize = 16;
		const int ySize = 16;
		var c = charAt - ' ';

		var cx = c % xSize;
		var cy = c / xSize;
		double top = cy * (1.0f / ySize);
		double bottom = (cy + 1) * (1.0f / ySize);
		double right = (cx + 1) * (1.0f / xSize);
		double left = cx * (1.0f / xSize);
		GL.Begin(PrimitiveType.Quads);
		GL.TexCoord2(new Vector2d(top, left));
		GL.Vertex2(new Vector2d(0.0f, 0.0f));
		GL.TexCoord2(new Vector2d(bottom, left));
		GL.Vertex2(new Vector2d(0.0f, 1.0f));
		GL.TexCoord2(new Vector2d(bottom, right));
		GL.Vertex2(new Vector2d(1.0f, 1.0f));
		GL.TexCoord2(new Vector2d(top, right));
		GL.Vertex2(new Vector2d(1.0f, 0.0f));
		GL.End();
	}
}