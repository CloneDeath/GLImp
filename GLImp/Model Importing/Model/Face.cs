using System;
using System.Collections.Generic;
using System.Text;
using Mila.Types;

namespace Mila.Data
{
    /// <summary>
    /// Represents an n-vertex polygon in 3D-space.
    /// </summary>
    public class Face
    {
        public int[] vertices = null;
        public int[] vertexAttributes = null;
        public Vector3D normal = new Vector3D();
    }
}
