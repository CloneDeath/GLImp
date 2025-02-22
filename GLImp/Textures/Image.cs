using OpenTK.Mathematics;

namespace GLImp;

public interface Image {
	public int Width { get; set; }
	public int Height { get; set; }

	public void Draw(Vector2d Position, Vector2d Size);

	public void Draw(Vector2d Position) => Draw(Position, new Vector2d(Width, Height));
	public void Draw(double X, double Y) => Draw(new Vector2d(X, Y));

	public void Draw(double X, double Y, double width, double height) =>
		Draw(new Vector2d(X, Y), new Vector2d(width, height));
}