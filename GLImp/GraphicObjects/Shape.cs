using System.Drawing;
using GLImp.Textures;
using OpenTK.Mathematics;

namespace GLImp.GraphicObjects;

public class Shape {
	public int[]? Colors;
	public int[] Indices = [];
	public Vector3[]? Normals;
	public Vector2[]? TexCoords;

	public Texture? Texture;
	public Vector3[] Vertices = [];

	/// <summary>
	///     Converts a System.Drawing.Color to a System.Int32.
	/// </summary>
	/// <param name="c">The System.Drawing.Color to convert.</param>
	/// <returns>
	///     A System.Int32 containing the R, G, B, A values of the
	///     given System.Drawing.Color in the Rbga32 format.
	/// </returns>
	protected static int ColorToRgba32(Color c) => (c.A << 24) | (c.B << 16) | (c.G << 8) | c.R;
}