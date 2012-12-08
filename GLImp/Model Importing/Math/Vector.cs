using System;
using System.Collections.Generic;
using System.Text;

namespace Mila.Types
{

    /// <summary>
    /// Represents a vector in 3D-space.
    /// </summary>
    public class Vector3D
    {
        private double x, y, z;

        #region Getters_And_Setters

        /// <summary>
        /// Gets or sets the x component of the vector.
        /// </summary>
        public double X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Gets or sets the y component of the vector.
        /// </summary>
        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// Gets or sets the z component of the vector.
        /// </summary>
        public double Z
        {
            get { return z; }
            set { z = value; }
        }

        #endregion

        /// <summary>
        /// Creates a new null vector.
        /// </summary>
        public Vector3D()
        {
            X = 0.0f;
            Y = 0.0f;
            Z = 0.0f;
        }

        /// <summary>
        /// Creates a new vector with the given components.
        /// </summary>
        /// <param name="x">The x component to set.</param>
        /// <param name="y">The y component to set.</param>
        /// <param name="z">The z component to set.</param>
        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Creates a new vector that is equivalent to another vector.
        /// </summary>
        /// <param name="v">The vector to copy.</param>
        public Vector3D(Vector3D v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        /// <summary>
        /// Creates a new vector from the given point.
        /// </summary>
        /// <param name="p">The point to construct a position vector from.</param>
        public Vector3D(Point3D p)
        {
            X = p.X;
            Y = p.Y;
            Z = p.Z;
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

        /// <summary>
        /// Returns the dot product of this vector and another.
        /// </summary>
        /// <param name="v">The vector to multiply by.</param>
        /// <returns>the dot product of this vector and another.</returns>
        public double dotProduct(Vector3D v)
        {
            return x * v.X + y * v.Y + z * v.Z;
        }

        /// <summary>
        /// Returns the cross product of this vector and another.
        /// </summary>
        /// <param name="v">The vector to multiply by.</param>
        /// <returns>the cross product of this vector and another.</returns>
        public Vector3D crossProduct(Vector3D v)
        {
            return new Vector3D(Y * v.z - Z * v.y, Z * v.x - X * v.z, X * v.y - Y * v.x);
        }

        /// <summary>
        /// Normalizes the vector (turning this vector into a unit vector).
        /// </summary>
        public void normalize()
        {
            double len = (double)System.Math.Sqrt((double)(X * X + Y * Y + Z * Z));

            X /= len;
            Y /= len;
            Z /= len;
        }

        /// <summary>
        /// Returns the length of the vector.
        /// </summary>
        /// <returns>the length of the vector.</returns>
        public double length()
        {
            return (double)System.Math.Sqrt((double)(X * X + Y * Y + Z * Z));
        }

        /// <summary>
        /// Returns the squared length of the vector.
        /// </summary>
        /// <returns>the squared length of the vector.</returns>
        public double squaredLength()
        {
            return (X * X + Y * Y + Z * Z);
        }

        /// <summary>
        /// Returns the quotient between this vector and another co-directional vector.
        /// </summary>
        /// <returns></returns>
        public double scalarDivision(Vector3D v)
        {
            if (x > 0.0001f || x < -0.0001f)
            {
                return z / v.Z;
            }
            else if (y > 0.0001f || y < -0.0001f)
            {
                return y / v.Y;
            }
            else
            {
                return x / v.X;
            }
        }

        /// <summary>
        /// Returns true if the given vector is a scalar multiple of the other given vector.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool isScalarMultipleOf(Vector3D v)
        {
            double a = 1, b = 1, c = 1;

            if (x == 0)
                if (v.X == 0)
                    a = 1;
                else
                    a = 0;
            else
                a = v.X / x;

            if (y == 0)
                if (v.Y == 0)
                    b = 1;
                else
                    b = 0;
            else
                b = v.Y / y;

            if (a != b)
                return false;

            if (z == 0)
                if (v.Z == 0)
                    c = 1;
                else
                    c = 0;
            else
                c = v.Z / z;

            if (b != c)
                return false;

            if (a == 0)
                return false;

            return true;
        }

        /// <summary>
        /// Returns true if the two vectors are equivalent.
        /// </summary>
        /// <param name="v">The other vector to compare against.</param>
        public bool equals(Vector3D v)
        {
            return v.X == x && v.Y == y && v.Z == z;
        }

        public void scalarMultiply(double n)
        {
            x *= n;
            y *= n;
            z *= n;
        }

        public void add(Vector3D v)
        {
            x += v.x;
            y += v.y;
            z += v.z;
        }

        /// <summary>
        /// Returns string to be used for tracing purpose.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", x.ToString(), y.ToString(), z.ToString());
        }
    }
}
