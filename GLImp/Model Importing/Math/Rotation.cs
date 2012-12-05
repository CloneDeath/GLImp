using System;
using System.Collections.Generic;
using System.Text;

namespace Mila.Types
{
    /// <summary>
    /// Represents a free rotation in 3D space.
    /// </summary>
    public class Rotation
    {
        private float yaw = 0.0f, pitch = 0.0f, tilt = 0.0f;

        #region Getters_And_Setters

        /// <summary>
        /// Gets or sets the yaw (azimuth) of the rotation.
        /// </summary>
        public float Yaw
        {
            get { return yaw; }
            set { yaw = value; }
        }

        /// <summary>
        /// Gets or sets the pitch (zenith) of the rotation.
        /// </summary>
        public float Pitch
        {
            get { return pitch; }
            set { pitch = value; }
        }

        /// <summary>
        /// Gets or sets the tilt of the rotation.
        /// </summary>
        public float Tilt
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
        public bool equals(Rotation other)
        {
            return other.Yaw == yaw && other.Pitch == pitch && other.Tilt == tilt;
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
}
