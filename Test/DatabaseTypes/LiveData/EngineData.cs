       public class EngineData{
        /// <summary>
        /// Minimum RPM to which the rev limiter shows an alert.
        /// </summary>
        public short MinAlertRPM { get; set; }

        /// <summary>
        /// Maximum RPM to the rev limiter alert.
        /// </summary>
        public short MaxAlertRPM { get; set; }

        /// <summary>
        /// Possible max speed achievable using the current transmission settings.
        /// <para> Will change depending on transmission settings.</para>
        /// </summary>
        public short CalculatedMaxSpeed { get; set; }
        /// <summary>
        /// Value below 1.0 is below 0 ingame, so 2.0 = 1 x 100kPa
        /// </summary>
        public float TurboBoost { get; set; }

        /// <summary>
        /// Oil Pressure (in Bars)
        /// </summary>
        public float OilPressure { get; set; }

        /// <summary>
        /// Games will always send 85.
        /// </summary>
        public float WaterTemperature { get; set; }

        /// <summary>
        /// Games will always send 110.
        /// </summary>
        public float OilTemperature { get; set; }
        /// <summary>
        /// Engine revolutions per minute
        /// </summary>
        public float EngineRPM { get; set; }
       }