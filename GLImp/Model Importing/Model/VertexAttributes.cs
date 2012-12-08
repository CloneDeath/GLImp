using System;
using System.Collections.Generic;
using System.Text;
using Mila.Types;

namespace Mila.Data
{
    /// <summary>
    /// Represents a set of attributes relevant to how a vertex is drawn on a particular face.
    /// </summary>
    public class VertexAttributes
    {
        public double u = 0.0f, v = 0.0f;
        public double r = 0.0f, g = 0.0f, b = 0.0f;
        public Vector3D normal = new Vector3D();
    }
}
