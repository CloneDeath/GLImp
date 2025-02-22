using GLImp.Mila.Types;

namespace GLImp.Mila.Data;

/// <summary>
///     Represents a set of attributes relevant to how a vertex is drawn on a particular face.
/// </summary>
public class VertexAttributes {
	public Vector3D normal = new();
	public double r = 0.0f, g = 0.0f, b = 0.0f;
	public double u = 0.0f, v = 0.0f;
}