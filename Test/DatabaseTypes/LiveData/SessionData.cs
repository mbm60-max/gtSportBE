using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using System.Numerics;
namespace PDTools.SimulatorInterface
{
       public class SessionData{
        /// <summary>
        /// Internal code that identifies the car.
        /// <para>This value may be overriden if using a car that uses 9 or more gears (oversight).</para>
        /// </summary>
        public int CarCode { get; set; }

        /// <summary>
        /// Position of the car before the race has started.
        /// <para>Will be -1 once the race is started.</para>
        /// </summary>
        public short PreRaceStartPositionOrQualiPos { get; set; }

        /// <summary>
        /// Number of cars in the race before the race has started.
        /// <para>Will be -1 once the race is started.</para>
        /// </summary>
        public short NumCarsAtPreRace { get; set; }

        /// <summary>
        /// Current time of day on the track.
        /// </summary>
        public TimeSpan TimeOfDayProgression { get; set; }
        /// <summary>
        /// Laps to finish.
        /// </summary>
        public short LapsInRace { get; set; }
        /// <summary>
        /// Date when this packet was received.
        /// </summary>
        public DateTimeOffset DateReceived { get; private set; }

        public DateTimeOffset StartTime { get; private set; }

        public TimeSpan LengthOfSession { get; set; }

        /// <summary>
        /// Game Type linked to this packet.
        /// </summary>
        public SimulatorInterfaceGameType GameType { get; set; }
         public IPEndPoint RemoteEndPoint { get; private set; }
       }
}