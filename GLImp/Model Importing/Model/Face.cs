using Mila.Types;

namespace Mila.Data;

/// <summary>
/// Represents an n-vertex polygon in 3D-space.
/// </summary>
public class Face
{
    public int[]? vertices = null;
    public int[]? vertexAttributes = null;
    public Vector3D normal = new();
}