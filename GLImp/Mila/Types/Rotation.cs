using System;

namespace GLImp.Mila.Types;

/// <summary>
///     Represents a free rotation in 3D space.
/// </summary>
public class Rotation {
	public Rotation() { }

    /// <summary>
    ///     Creates a new rotation object by cloning another.
    /// </summary>
    /// <param name="other"></param>
    public Rotation(Rotation other) {
		Yaw = other.Yaw;
		Pitch = other.Pitch;
		Tilt = other.Tilt;
	}

    /// <summary>
    ///     Returns true if the two rotations are equivalent.
    /// </summary>
    /// <param name="other">The other rotation to compare against.</param>
    /// <returns>true if the two rotations are equivalent.</returns>
    public bool equals(Rotation other) =>
		Math.Abs(other.Yaw - Yaw) <= double.Epsilon
		&& Math.Abs(other.Pitch - Pitch) <= double.Epsilon
		&& Math.Abs(other.Tilt - Tilt) <= double.Epsilon;

    /// <summary>
    ///     Sets the rotation to the same values as the other given rotation.
    /// </summary>
    /// <param name="other"></param>
    public void set(Rotation other) {
		Yaw = other.Yaw;
		Pitch = other.Pitch;
		Tilt = other.Tilt;
	}

	#region Getters_And_Setters
    /// <summary>
    ///     Gets or sets the yaw (azimuth) of the rotation.
    /// </summary>
    public double Yaw { get; set; }

    /// <summary>
    ///     Gets or sets the pitch (zenith) of the rotation.
    /// </summary>
    public double Pitch { get; set; }

    /// <summary>
    ///     Gets or sets the tilt of the rotation.
    /// </summary>
    public double Tilt { get; set; }
	#endregion
}