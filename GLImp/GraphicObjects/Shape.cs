using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using System.Drawing;

namespace GLImp.GraphicObjects {
	public class Shape {
		public Vector3[] Vertices;
		public Vector3[] Normals;
		public int[] Indices;
		public int[] Colors;
		public Vector2[] TexCoords;

		public Texture Texture;


		/// <summary>
		/// Converts a System.Drawing.Color to a System.Int32.
		/// </summary>
		/// <param name="c">The System.Drawing.Color to convert.</param>
		/// <returns>A System.Int32 containing the R, G, B, A values of the
		/// given System.Drawing.Color in the Rbga32 format.</returns>
		protected static int ColorToRgba32(Color c) {
			return (int)((c.A << 24) | (c.B << 16) | (c.G << 8) | c.R);
		}
	}
}
