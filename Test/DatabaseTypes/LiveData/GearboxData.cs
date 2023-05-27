        public class GearboxData{

        
        
        public float ClutchEngagement { get; set; }

        /// <summary>
        /// Basically same as engine rpm when in gear and the clutch pedal is not depressed.
        /// </summary>
        public float RPMFromClutchToGearbox { get; set; }

        /// <summary>
        /// Top Speed (as a Gear Ratio value)
        /// </summary>
        public float TransmissionTopSpeed { get; set; }

        /// <summary>
        /// Gear ratios for the car. Up to 7.
        /// </summary>
        public float[] GearRatios { get; set; } = new float[7];

         /// <summary>
        /// 0.0 to 1.0
        /// </summary>
        public float ClutchPedal { get; set; }

        /// <summary>
        /// Current Gear for the car.
        /// <para> This value will never be more than 15 (4 bits).</para>
        /// </summary>
        public byte CurrentGear { get; set; }

        /// <summary>
        /// (Assist) Suggested Gear to downshift to. 
        /// <para> This value will never be more than 15 (4 bits), All bits on (aka 15) implies there is no current suggested gear.</para>
        /// </summary>
        public byte SuggestedGear { get; set; }
        }