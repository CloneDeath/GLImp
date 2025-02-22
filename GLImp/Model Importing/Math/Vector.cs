using System;

namespace Mila.Types;

/// <summary>
///     Represents a vector in 3D-space.
/// </summary>
public class Vector3D {
    /// <summary>
    ///     Creates a new null vector.
    /// </summary>
    public Vector3D() {
		X = 0.0f;
		Y = 0.0f;
		Z = 0.0f;
	}

    /// <summary>
    ///     Creates a new vector with the given components.
    /// </summary>
    /// <param name="x">The x component to set.</param>
    /// <param name="y">The y component to set.</param>
    /// <param name="z">The z component to set.</param>
    public Vector3D(double x, double y, double z) {
		X = x;
		Y = y;
		Z = z;
	}

    /// <summary>
    ///     Creates a new vector that is equivalent to another vector.
    /// </summary>
    /// <param name="v">The vector to copy.</param>
    public Vector3D(Vector3D v) {
		X = v.X;
		Y = v.Y;
		Z = v.Z;
	}

    /// <summary>
    ///     Creates a new vector from the given point.
    /// </summary>
    /// <param name="p">The point to construct a position vector from.</param>
    public Vector3D(Point3D p) {
		X = p.X;
		Y = p.Y;
		Z = p.Z;
	}

    /// <summary>
    ///     Sets new value from the given vector.
    /// </summary>
    /// <param name="v"></param>
    public void Set(Vector3D v) {
		X = v.X;
		Y = v.Y;
		Z = v.Z;
	}

    /// <summary>
    ///     Sets new value from the given point.
    /// </summary>
    /// <param name="p"></param>
    public void Set(Point3D p) {
		X = p.X;
		Y = p.Y;
		Z = p.Z;
	}

    /// <summary>
    ///     Returns the dot product of this vector and another.
    /// </summary>
    /// <param name="v">The vector to multiply by.</param>
    /// <returns>the dot product of this vector and another.</returns>
    public double dotProduct(Vector3D v) => X * v.X + Y * v.Y + Z * v.Z;

    /// <summary>
    ///     Returns the cross product of this vector and another.
    /// </summary>
    /// <param name="v">The vector to multiply by.</param>
    /// <returns>the cross product of this vector and another.</returns>
    public Vector3D crossProduct(Vector3D v) => new(Y * v.Z - Z * v.Y, Z * v.X - X * v.Z, X * v.Y - Y * v.X);

    /// <summary>
    ///     Normalizes the vector (turning this vector into a unit vector).
    /// </summary>
    public void normalize() {
		var len = Math.Sqrt(X * X + Y * Y + Z * Z);

		X /= len;
		Y /= len;
		Z /= len;
	}

    /// <summary>
    ///     Returns the length of the vector.
    /// </summary>
    /// <returns>the length of the vector.</returns>
    public double length() => Math.Sqrt(X * X + Y * Y + Z * Z);

    /// <summary>
    ///     Returns the squared length of the vector.
    /// </summary>
    /// <returns>the squared length of the vector.</returns>
    public double squaredLength() => X * X + Y * Y + Z * Z;

    /// <summary>
    ///     Returns the quotient between this vector and another co-directional vector.
    /// </summary>
    /// <returns></returns>
    public double scalarDivision(Vector3D v) {
		if (X > 0.0001f || X < -0.0001f) {
			return Z / v.Z;
		}

		if (Y > 0.0001f || Y < -0.0001f) {
			return Y / v.Y;
		}

		return X / v.X;
	}

    /// <summary>
    ///     Returns true if the given vector is a scalar multiple of the other given vector.
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public bool isScalarMultipleOf(Vector3D v) {
		double a, b, c;

		if (X == 0) {
			a = v.X == 0 ? 1 : 0;
		} else {
			a = v.X / X;
		}

		if (Y == 0) {
			b = v.Y == 0 ? 1 : 0;
		} else {
			b = v.Y / Y;
		}

		if (Math.Abs(a - b) > double.Epsilon) {
			return false;
		}

		if (Z == 0) {
			c = v.Z == 0 ? 1 : 0;
		} else {
			c = v.Z / Z;
		}

		if (Math.Abs(b - c) > double.Epsilon) {
			return false;
		}

		return a != 0;
	}

    /// <summary>
    ///     Returns true if the two vectors are equivalent.
    /// </summary>
    /// <param name="v">The other vector to compare against.</param>
    public bool equals(Vector3D v) =>
		Math.Abs(v.X - X) <= double.Epsilon
		&& Math.Abs(v.Y - Y) <= double.Epsilon
		&& Math.Abs(v.Z - Z) <= double.Epsilon;

	public void scalarMultiply(double n) {
		X *= n;
		Y *= n;
		Z *= n;
	}

	public void add(Vector3D v) {
		X += v.X;
		Y += v.Y;
		Z += v.Z;
	}

    /// <summary>
    ///     Returns string to be used for tracing purpose.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"({X}, {Y}, {Z})";

	#region Getters_And_Setters
    /// <summary>
    ///     Gets or sets the x component of the vector.
    /// </summary>
    public double X { get; set; }

    /// <summary>
    ///     Gets or sets the y component of the vector.
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    ///     Gets or sets the z component of the vector.
    /// </summary>
    public double Z { get; set; }
	#endregion
}