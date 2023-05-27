
public class PositionalData{


        public byte Empty_0x93 { get; set; }

        public Vector3 RoadPlane { get; set; }

        public float RoadPlaneDistance { get; set; }

        /// <summary>
        /// Current speed in meters per second. <see cref="MetersPerSecond * 3.6"/> to get it in KPH.
        /// </summary>
        public float MetersPerSecond { get; set; }
         /// <summary>
        /// Position on the track. Track units are in meters.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Velocity in track units (which are meters) for each axis.
        /// </summary>
        public Vector3 Velocity { get; set; }

        /// <summary>
        /// Rotation (Pitch/Yaw/Roll) from -1 to 1.
        /// </summary>
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// Orientation to North. 1.0 is north, 0.0 is south.
        /// </summary>
        public float RelativeOrientationToNorth { get; set; }

        /// <summary>
        /// How fast the car turns around axes. (In radians/second, -1 to 1).
        /// </summary>
        public Vector3 AngularVelocity { get; set; }
}