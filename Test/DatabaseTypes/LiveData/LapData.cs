        
        public class LapData{

        /// <summary>
        /// Current lap count.
        /// </summary>
        public short LapCount { get; set; }

        /// <summary>
        /// Laps to finish.
        /// </summary>
        public short LapsInRace { get; set; }

        /// <summary>
        /// Best Lap Time. 
        /// <para>Defaults to -1 millisecond when not set.</para>
        /// </summary>
        public TimeSpan BestLapTime { get; set; }

        /// <summary>
        /// Last Lap Time.
        /// <para>Defaults to -1 millisecond when not set.</para>
        /// </summary>
        public TimeSpan LastLapTime { get; set; }
        }