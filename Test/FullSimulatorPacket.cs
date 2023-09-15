using PDTools.SimulatorInterface;
using System.Net;
using System.Numerics;

namespace FullSimulatorPacket{
   internal class ExtendedPacket
{
        /// <summary>
        /// Date when this packet was received.
        /// </summary>
        public string DateReceived { get; set; }
        public double distanceFromStart { get; set; }
        /// <summary>
        /// Game Type linked to this packet.
        /// </summary>
        public SimulatorInterfaceGameType GameType { get; set; }

        /// <summary>
        /// Position on the track. Track units are in meters.
        /// </summary>
        public string[] Position { get; set; }

        /// <summary>
        /// Velocity in track units (which are meters) for each axis.
        /// </summary>
        public string[] Velocity { get; set; }

        /// <summary>
        /// Rotation (Pitch/Yaw/Roll) from -1 to 1.
        /// </summary>
        public string[] Rotation { get; set; }

        /// <summary>
        /// Orientation to North. 1.0 is north, 0.0 is south.
        /// </summary>
        public float RelativeOrientationToNorth { get; set; }

        /// <summary>x
        /// How fast the car turns around axes. (In radians/second, -1 to 1).
        /// </summary>
        public string[] AngularVelocity { get; set; }

        /// <summary>
        /// Body height.
        /// </summary>
        public float BodyHeight { get; set; }

        /// <summary>
        /// Engine revolutions per minute
        /// </summary>
        public float EngineRPM { get; set; }

        /// <summary>
        /// Gas level for the current car (in liters, from 0 to <see cref="GasCapacity"/>).
        /// <para> Note: This may change from 0 when regenerative braking with electric cars, check accordingly with <see cref="GasCapacity"/>. </para>
        /// </summary>
        public float GasLevel { get; set; }

        /// <summary>
        /// Max gas capacity for the current car.
        /// Will be 100 for most cars, 5 for karts, 0 for electric cars
        /// </summary>
        public float GasCapacity { get; set; }

        /// <summary>
        /// Current speed in meters per second. <see cref="MetersPerSecond * 3.6"/> to get it in KPH.
        /// </summary>
        public float MetersPerSecond { get; set; }

        /// <summary>
        /// Value below 1.0 is below 0 ingame, so 2.0 = 1 x 100kPa
        /// </summary>
        public float TurboBoost { get; set; }

        /// <summary>
        /// Oil Pressure (in Bars)
        /// </summary>
        public float OilPressure { get; set; }

        /// <summary>
        /// Games will always send 85
        /// </summary>
        public float WaterTemperature { get; set; }

        /// <summary>
        /// Games will always send 110.
        /// </summary>
        public float OilTemperature { get; set; }

        /// <summary>
        /// Front Left Tire - Surface Temperature (in °C)
        /// </summary>
        public float TireFL_SurfaceTemperature { get; set; }

        /// <summary>
        /// Front Right - Surface Temperature (in °C)
        /// </summary>
        public float TireFR_SurfaceTemperature { get; set; }

        /// <summary>
        /// Rear Left - Surface Temperature (in °C)
        /// </summary>
        public float TireRL_SurfaceTemperature { get; set; }

        /// <summary>
        /// Rear Right - Surface Temperature (in °C)
        /// </summary>
        public float TireRR_SurfaceTemperature { get; set; }

        /// <summary>
        /// Id of the packet for proper ordering.
        /// </summary>
        public int PacketId { get; set; }

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
        public string BestLapTime { get; set; }

        /// <summary>
        /// Last Lap Time.
        /// <para>Defaults to -1 millisecond when not set.</para>
        /// </summary>
        public string LastLapTime { get; set; }

        /// <summary>
        /// Current time of day on the track.
        /// </summary>
        public string TimeOfDayProgression { get; set; }

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
        /// Packet flags.
        /// </summary>
        public SimulatorFlags Flags { get; set; }

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

        /// <summary>
        /// Throttle (0-255)
        /// </summary>
        public byte Throttle { get; set; }

        /// <summary>
        /// Brake Pedal (0-255)
        /// </summary>
        public byte Brake { get; set; }

        public byte Empty_0x93 { get; set; }

        public string[] RoadPlane { get; set; }

        public float RoadPlaneDistance { get; set; }

        /// <summary>
        /// Front Left Wheel - Revolutions Per Second (in Radians)
        /// </summary>
        public float WheelFL_RevPerSecond { get; set; }

        /// <summary>
        /// Front Right Wheel - Revolutions Per Second (in Radians)
        /// </summary>
        public float WheelFR_RevPerSecond { get; set; }

        /// <summary>
        /// Rear Left Wheel - Revolutions Per Second (in Radians)
        /// </summary>
        public float WheelRL_RevPerSecond { get; set; }

        /// <summary>
        /// Rear Right Wheel - Revolutions Per Second (in Radians)
        /// </summary>
        public float WheelRR_RevPerSecond { get; set; }

        /// <summary>
        /// Front Left Tire - Tire Radius (in Meters)
        /// </summary>
        public float TireFL_TireRadius { get; set; }

        /// <summary>
        /// Front Right Tire - Tire Radius (in Meters)
        /// </summary>
        public float TireFR_TireRadius { get; set; }

        /// <summary>
        /// Rear Left Tire - Tire Radius (in Meters)
        /// </summary>
        public float TireRL_TireRadius { get; set; }

        /// <summary>
        /// Rear Right Tire - Tire Radius (in Meters)
        /// </summary>
        public float TireRR_TireRadius { get; set; }

        /// <summary>
        /// Front Left Tire - Suspension Height
        /// </summary>
        public float TireFL_SusHeight { get; set; }

        /// <summary>
        /// Front Right Tire - Suspension Height
        /// </summary>
        public float TireFR_SusHeight { get; set; }

        /// <summary>
        /// Rear Left Tire - Suspension Height
        /// </summary>
        public float TireRL_SusHeight { get; set; }

        /// <summary>
        /// Rear Right Tire - Suspension Height
        /// </summary>
        public float TireRR_SusHeight { get; set; }

        /// <summary>
        /// 0.0 to 1.0
        /// </summary>
        public float ClutchPedal { get; set; }

        /// <summary>
        /// 0.0 to 1.0
        /// </summary>
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
        /// Internal code that identifies the car.
        /// <para>This value may be overriden if using a car that uses 9 or more gears (oversight).</para>
        /// </summary>
        public int CarCode { get; set; }

        public string LapTiming {get; set;}

        public int InLapShifts {get; set;}
} 
}