using System;
using System.Collections.Generic;
using System.Text;

namespace Mila.Types
{
    /// <summary>
    /// Represents a plane in 3D-space.
    /// </summary>
    public class Plane
    {
        private float a, b, c, d;

        #region Getters_And_Setters

        /// <summary>
        /// Gets or sets the a component of the plane.
        /// </summary>
        public float A
        {
            get { return a; }
            set { a = value; }
        }

        /// <summary>
        /// Gets or sets the b component of the plane.
        /// </summary>
        public float B
        {
            get { return b; }
            set { b = value; }
        }

        /// <summary>
        /// Gets or sets the c component of the plane.
        /// </summary>
        public float C
        {
            get { return c; }
            set { c = value; }
        }

        /// <summary>
        /// Gets or sets the d component of the plane.
        /// </summary>
        public float D
        {
            get { return d; }
            set { d = value; }
        }

        #endregion

        /// <summary>
        /// Creates a new plane with a null vector and zero distance.
        /// </summary>
        public Plane()
        {
            A = 0.0f;
            B = 0.0f;
            C = 0.0f;
            D = 0.0f;
        }

        /// <summary>
        /// Creates a new plane with the given components.
        /// </summary>
        /// <param name="a">The a component to set.</param>
        /// <param name="b">The b component to set.</param>
        /// <param name="c">The c component to set.</param>
        /// <param name="d">The d component to set.</param>
        public Plane(float a, float b, float c, float d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }

        /// <summary>
        /// Creates a new plane from the given normal vector and a distance.
        /// </summary>
        /// <param name="v">The vector normal to the plane.</param>
        /// <param name="d">The distance along the plane.</param>
        public Plane(Vector3D v, float d)
        {
            A = v.X;
            B = v.Y;
            C = v.Z;
            D = d;
        }
    }
}