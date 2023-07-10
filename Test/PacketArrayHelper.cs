using System;
 using FullSimulatorPacket;
using System;
using System.Linq;
using FullSimulatorPacket;
namespace PacketArrayHelpers{
    internal class PacketArrayHelperClass{
      

    public void ProcessExtendedPacket(ExtendedPacket packet,  ExtendedPacketArrays packetArrays,Boolean IsNewLap)
    {
    packetArrays.DateReceived=packet.DateReceived;
    packetArrays.distanceFromStart = UpdateArray(packetArrays.distanceFromStart, packet.distanceFromStart, IsNewLap);
    packetArrays.Velocity = UpdateArray(packetArrays.Velocity, packet.Velocity, IsNewLap);
    packetArrays.Rotation = UpdateArray(packetArrays.Rotation, packet.Rotation, IsNewLap);
    packetArrays.AngularVelocity = UpdateArray(packetArrays.AngularVelocity, packet.AngularVelocity, IsNewLap);
    packetArrays.BodyHeight = UpdateArray(packetArrays.BodyHeight, packet.BodyHeight, IsNewLap);
    packetArrays.EngineRPM = UpdateArray(packetArrays.EngineRPM, packet.EngineRPM, IsNewLap);
    packetArrays.GasLevel = UpdateArray(packetArrays.GasLevel, packet.GasLevel, IsNewLap);
    packetArrays.GasCapacity = UpdateArray(packetArrays.GasCapacity, packet.GasCapacity, IsNewLap);
    packetArrays.MetersPerSecond = UpdateArray(packetArrays.MetersPerSecond, packet.MetersPerSecond, IsNewLap);
    packetArrays.TurboBoost = UpdateArray(packetArrays.TurboBoost, packet.TurboBoost, IsNewLap);
    packetArrays.OilPressure = UpdateArray(packetArrays.OilPressure, packet.OilPressure, IsNewLap);
    packetArrays.TireFL_SurfaceTemperature = UpdateArray(packetArrays.TireFL_SurfaceTemperature, packet.TireFL_SurfaceTemperature, IsNewLap);
    packetArrays.TireFR_SurfaceTemperature = UpdateArray(packetArrays.TireFR_SurfaceTemperature, packet.TireFR_SurfaceTemperature, IsNewLap);
    packetArrays.TireRL_SurfaceTemperature = UpdateArray(packetArrays.TireRL_SurfaceTemperature, packet.TireRL_SurfaceTemperature, IsNewLap);
    packetArrays.TireRR_SurfaceTemperature = UpdateArray(packetArrays.TireRR_SurfaceTemperature, packet.TireRR_SurfaceTemperature, IsNewLap);
    packetArrays.LapCount = UpdateArray(packetArrays.LapCount, packet.LapCount, IsNewLap);
    packetArrays.LapsInRace = UpdateArray(packetArrays.LapsInRace, packet.LapsInRace, IsNewLap);
    packetArrays.BestLapTime = UpdateArray(packetArrays.BestLapTime, packet.BestLapTime, IsNewLap);
    packetArrays.LastLapTime = UpdateArray(packetArrays.LastLapTime, packet.LastLapTime, IsNewLap);
    packetArrays.CalculatedMaxSpeed = UpdateArray(packetArrays.CalculatedMaxSpeed, packet.CalculatedMaxSpeed, IsNewLap);
    packetArrays.CurrentGear = UpdateArray(packetArrays.CurrentGear, packet.CurrentGear, IsNewLap);
    packetArrays.SuggestedGear = UpdateArray(packetArrays.SuggestedGear, packet.SuggestedGear, IsNewLap);
    packetArrays.Throttle = UpdateArray(packetArrays.Throttle, packet.Throttle, IsNewLap);
    packetArrays.Brake = UpdateArray(packetArrays.Brake, packet.Brake, IsNewLap);
    packetArrays.RoadPlane = UpdateArray(packetArrays.RoadPlane, packet.RoadPlane, IsNewLap);
    packetArrays.RoadPlaneDistance = UpdateArray(packetArrays.RoadPlaneDistance, packet.RoadPlaneDistance, IsNewLap);
    packetArrays.WheelFL_RevPerSecond = UpdateArray(packetArrays.WheelFL_RevPerSecond, packet.WheelFL_RevPerSecond, IsNewLap);
    packetArrays.WheelFR_RevPerSecond = UpdateArray(packetArrays.WheelFR_RevPerSecond, packet.WheelFR_RevPerSecond, IsNewLap);
    packetArrays.WheelRL_RevPerSecond = UpdateArray(packetArrays.WheelRL_RevPerSecond, packet.WheelRL_RevPerSecond, IsNewLap);
    packetArrays.WheelRR_RevPerSecond = UpdateArray(packetArrays.WheelRR_RevPerSecond, packet.WheelRR_RevPerSecond, IsNewLap);
    packetArrays.TireFL_SusHeight = UpdateArray(packetArrays.TireFL_SusHeight, packet.TireFL_SusHeight, IsNewLap);
    packetArrays.TireFR_SusHeight = UpdateArray(packetArrays.TireFR_SusHeight, packet.TireFR_SusHeight, IsNewLap);
    packetArrays.TireRL_SusHeight = UpdateArray(packetArrays.TireRL_SusHeight, packet.TireRL_SusHeight, IsNewLap);
    packetArrays.TireRR_SusHeight = UpdateArray(packetArrays.TireRR_SusHeight, packet.TireRR_SusHeight, IsNewLap);
    packetArrays.TireFL_TireRadius = UpdateArray(packetArrays.TireFL_TireRadius, packet.TireFL_TireRadius, IsNewLap);
    packetArrays.TireFR_TireRadius = UpdateArray(packetArrays.TireFR_TireRadius, packet.TireFR_TireRadius, IsNewLap);
    packetArrays.TireRL_TireRadius = UpdateArray(packetArrays.TireRL_TireRadius, packet.TireRL_TireRadius, IsNewLap);
    packetArrays.TireRR_TireRadius = UpdateArray(packetArrays.TireRR_TireRadius, packet.TireRR_TireRadius, IsNewLap);
    packetArrays.ClutchPedal = UpdateArray(packetArrays.ClutchPedal, packet.ClutchPedal, IsNewLap);
    packetArrays.ClutchEngagement = UpdateArray(packetArrays.ClutchEngagement, packet.ClutchEngagement, IsNewLap);
    packetArrays.RPMFromClutchToGearbox = UpdateArray(packetArrays.RPMFromClutchToGearbox, packet.RPMFromClutchToGearbox, IsNewLap);
    packetArrays.TransmissionTopSpeed = UpdateArray(packetArrays.TransmissionTopSpeed, packet.TransmissionTopSpeed, IsNewLap);
    packetArrays.LapTiming = UpdateArray(packetArrays.LapTiming, packet.LapTiming, IsNewLap);
    packetArrays.InLapShifts = UpdateArray(packetArrays.InLapShifts, packet.InLapShifts, IsNewLap);
    }

    private T[] UpdateArray<T>( T[] currentArray, T value,Boolean IsNewLap)
    {   
        if (currentArray == null)
        {
            currentArray = new T[0];
        }
        // Create a new array with the updated length
        var newArray = new T[currentArray.Length + 1];
        // Copy the values from the current array to the new array
        Array.Copy(currentArray, newArray, currentArray.Length);

        if (IsNewLap)
        {
            newArray = new T[0];
        }
        // Set the new value at the end of the array
        newArray[newArray.Length - 1] = value;

       
        // Set the updated array
        return newArray;
    }

    private T[][] UpdateArray<T>( T[][] currentArray, T[] value,Boolean IsNewLap)
    {
        if (currentArray == null)
        {
            currentArray = new T[0][];
        }
        // Create a new array with the updated length
        var newArray = new T[currentArray.Length + 1][];

        // Copy the values from the current array to the new array
        Array.Copy(currentArray, newArray, currentArray.Length);

        if (IsNewLap)
        {
            newArray = new T[0][];
        }
        // Set the new value at the end of the array
        newArray[newArray.Length - 1] = value;

        // Set the updated array
       // Console.WriteLine(newArray[0][0]);
        return newArray;
    }


    }
}