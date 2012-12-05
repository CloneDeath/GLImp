using System;
using System.Collections.Generic;
using System.Text;

namespace Mila.Types
{

    /// <summary>
    /// Represents a point in 3D-space.
    /// </summary>
    public class Point3D
    {
        private float x, y, z;

        #region Getters_And_Setters

        /// <summary>
        /// Gets or sets the x coordinate of this point.
        /// </summary>
        public float X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Gets or sets the y coordinate of this point.
        /// </summary>
        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// Gets or sets the z coordinate of this point.
        /// </summary>
        public float Z
        {
            get { return z; }
            set { z = value; }
        }

        #endregion

        /// <summary>
        /// Creates a new point that represents the origin.
        /// </summary>
        public Point3D()
        {
            X = 0.0f;
            Y = 0.0f;
            Z = 0.0f;
        }

        /// <summary>
        /// Creates a new point with the given set of coordinates.
        /// </summary>
        /// <param name="x">The x coordinate to set.</param>
        /// <param name="y">The y coordinate to set.</param>
        /// <param name="z">The z coordinate to set.</param>
        public Point3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Creates a new point from the given position vector.
        /// </summary>
        /// <param name="v">The position vector to create a point from.</param>
        public Point3D(Vector3D v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        /// <summary>
        /// Sets new value from the given coordinates.
        /// </summary>
        /// <param name="v"></param>
        public void Set(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Sets new value from the given vector.
        /// </summary>
        /// <param name="v"></param>
        public void Set(Vector3D v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        /// <summary>
        /// Sets new value from the given point.
        /// </summary>
        /// <param name="p"></param>
        public void Set(Point3D p)
        {
            X = p.X;
            Y = p.Y;
            Z = p.Z;
        }
    }
}
