using GLImp.Mila.Types;

namespace GLImp.Mila.Data;

/// <summary>
///     Represents an n-vertex polygon in 3D-space.
/// </summary>
public class Face {
	public Vector3D normal = new();
	public int[]? vertexAttributes = null;
	public int[]? vertices = null;
}