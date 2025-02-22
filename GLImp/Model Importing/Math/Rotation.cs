using System;

namespace Mila.Types;

/// <summary>
/// Represents a free rotation in 3D space.
/// </summary>
public class Rotation
{
    private double yaw, pitch, tilt;

    #region Getters_And_Setters

    /// <summary>
    /// Gets or sets the yaw (azimuth) of the rotation.
    /// </summary>
    public double Yaw
    {
        get { return yaw; }
        set { yaw = value; }
    }

    /// <summary>
    /// Gets or sets the pitch (zenith) of the rotation.
    /// </summary>
    public double Pitch
    {
        get { return pitch; }
        set { pitch = value; }
    }

    /// <summary>
    /// Gets or sets the tilt of the rotation.
    /// </summary>
    public double Tilt
    {
        get { return tilt; }
        set { tilt = value; }
    }

    #endregion

    public Rotation()
    {
    }

    /// <summary>
    /// Creates a new rotation object by cloning another.
    /// </summary>
    /// <param name="other"></param>
    public Rotation(Rotation other)
    {
        yaw = other.Yaw;
        pitch = other.Pitch;
        tilt = other.Tilt;
    }

    /// <summary>
    /// Returns true if the two rotations are equivalent.
    /// </summary>
    /// <param name="other">The other rotation to compare against.</param>
    /// <returns>true if the two rotations are equivalent.</returns>
    public bool equals(Rotation other) {
        return Math.Abs(other.Yaw - yaw) <= double.Epsilon
               && Math.Abs(other.Pitch - pitch) <= double.Epsilon
               && Math.Abs(other.Tilt - tilt) <= double.Epsilon;
    }

    /// <summary>
    /// Sets the rotation to the same values as the other given rotation.
    /// </summary>
    /// <param name="other"></param>
    public void set(Rotation other)
    {
        yaw = other.Yaw;
        pitch = other.Pitch;
        tilt = other.Tilt;
    }
}