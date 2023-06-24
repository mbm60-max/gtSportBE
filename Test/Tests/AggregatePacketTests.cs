using System;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using PDTools.SimulatorInterfaceTestTool;
using PDTools.SimulatorInterface;
using PacketHelpers;

namespace PDTools.SimulatorInterfaceTestTool.Tests
{
    public class AggregatePacketTests
    {
        [Fact]
        public void AggregatePacket_ShouldAggregatePropertiesCorrectly()
        {
            // Arrange
            SimulatorPacket packet = new SimulatorPacket
            {
                // Set the properties of the packet object here for testing
                RemoteEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.1"), 1234),
                DateReceived = new DateTimeOffset(2023, 6, 20, 10, 30, 0, TimeSpan.Zero),
                GameType = SimulatorInterfaceGameType.GTSport,
                Position = new Vector3(10.5f, 5.2f, -3.7f),
                Velocity = new Vector3(2.3f, 0.1f, -1.8f),
                Rotation = new Vector3(0.5f, 0.2f, 0.8f),
                RelativeOrientationToNorth = 0.75f,
                AngularVelocity = new Vector3(0.1f, -0.3f, 0.2f),
                BodyHeight = 1.75f,
                EngineRPM = 6000.0f,
                GasLevel = 0.75f,
                GasCapacity = 60.0f,
                MetersPerSecond = 25.0f,
                TurboBoost = 1.8f,
                OilPressure = 4.2f,
                WaterTemperature = 80.0f,
                OilTemperature = 100.0f,
                TireFL_SurfaceTemperature = 32.5f,
                TireFR_SurfaceTemperature = 30.2f,
                TireRL_SurfaceTemperature = 33.8f,
                TireRR_SurfaceTemperature = 31.6f,
                PacketId = 123,
                LapCount = 5,
                LapsInRace = 10,
                BestLapTime = TimeSpan.FromMinutes(1.45),
                LastLapTime = TimeSpan.FromMinutes(1.55),
                TimeOfDayProgression = TimeSpan.FromHours(3),
                PreRaceStartPositionOrQualiPos = 2,
                NumCarsAtPreRace = 12,
                MinAlertRPM = 3000,
                MaxAlertRPM = 7000,
                CalculatedMaxSpeed = 200,
                Flags = SimulatorFlags.None,
                CurrentGear = 4,
                SuggestedGear = 3,
                Throttle = 80,
                Brake = 50,
                Empty_0x93 = 0,
                RoadPlane = new Vector3(0.0f, -1.0f, 0.0f),
                RoadPlaneDistance = 10.0f,
                WheelFL_RevPerSecond = 20.5f,
                WheelFR_RevPerSecond = 21.0f,
                WheelRL_RevPerSecond = 20.2f,
                WheelRR_RevPerSecond = 20.8f,
                TireFL_TireRadius = 0.3f,
                TireFR_TireRadius = 0.3f,
                TireRL_TireRadius = 0.3f,
                TireRR_TireRadius = 0.3f,
                TireFL_SusHeight = 0.2f,
                TireFR_SusHeight = 0.2f,
                TireRL_SusHeight = 0.2f,
                TireRR_SusHeight = 0.2f,
                ClutchPedal = 0.8f,
                ClutchEngagement = 0.9f,
                RPMFromClutchToGearbox = 5500.0f,
                TransmissionTopSpeed = 250.0f,
                GearRatios = new float[] { 2.8f, 2.2f, 1.8f, 1.4f, 1.2f, 1.0f, 0.8f }
            };

            SimulatorPacket aggregation = new SimulatorPacket
            {
                // Set the properties of the aggregation object here for testing
                // Example:
                // Property1 = 5,
                // Property2 = 10.5f,
                // Property3 = new Vector3(2, 4, 6),
                // ...
            };

            SimulatorPacket expectedAggregation = new SimulatorPacket
            {
                // Calculate the expected aggregation result manually
                // based on the properties of the packet and aggregation objects
                // Example:
                // Property1 = packet.Property1 + aggregation.Property1,
                // Property2 = packet.Property2 + aggregation.Property2,
                // Property3 = Vector3.Add(packet.Property3, aggregation.Property3),
                // ...
            };

            // Act
            SimulatorPacket result = PacketHelper.AggregatePacket(ref packet, ref aggregation);

            // Assert
            Assert.Equal(expectedAggregation, result);
        }
    }
}
