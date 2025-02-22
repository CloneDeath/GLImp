using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GLImp;

public class SubImage : Image {
	private readonly Texture texture;
	private readonly int Xoffset;
	private readonly int Yoffset;

	public SubImage(Texture tex, int x, int y, int width, int height) {
		texture = tex;
		Width = width;
		Height = height;
		Xoffset = x;
		Yoffset = y;
	}

	private double Left => Xoffset * 1.0 / texture.Width;

	private double Right => (Xoffset + (Width - 1)) * 1.0 / texture.Width;

	private double Top => Yoffset * 1.0 / texture.Height;

	private double Bottom => (Yoffset + (Height - 1)) * 1.0 / texture.Height;

	public int Width { get; set; }
	public int Height { get; set; }

	public void Draw(Vector2d Position, Vector2d Size) {
		Draw(Position.X, Position.Y, Size.X, Size.Y, false);
	}

	public void Draw(double x, double y, bool Flip) {
		Draw(x, y, Width, Height, Flip);
	}

	public void Draw(double X, double Y, double width, double height, bool Flip) {
		var X1 = X;
		var Y1 = Y;
		var X2 = X + width;
		var Y2 = Y + height;
		GL.BindTexture(TextureTarget.Texture2D, texture.ID);
		if (!Flip) {
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(Left, Top);
			GL.Vertex2(X1, Y1);
			GL.TexCoord2(Right, Top);
			GL.Vertex2(X2, Y1);
			GL.TexCoord2(Right, Bottom);
			GL.Vertex2(X2, Y2);
			GL.TexCoord2(Left, Bottom);
			GL.Vertex2(X1, Y2);
			GL.End();
		} else {
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(Right, Top);
			GL.Vertex2(X1, Y1);
			GL.TexCoord2(Left, Top);
			GL.Vertex2(X2, Y1);
			GL.TexCoord2(Left, Bottom);
			GL.Vertex2(X2, Y2);
			GL.TexCoord2(Right, Bottom);
			GL.Vertex2(X1, Y2);
			GL.End();
		}
	}
}