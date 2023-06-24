using System;
using System.Net;
using System.Numerics;
using System.Reflection;
using PDTools.SimulatorInterface;

namespace PacketHelpers{
internal static class PacketHelper
{
    internal static SimulatorPacket SummarizePacket(ref SimulatorPacket aggregation, ref int packetCount)
        {
            Type packetType = typeof(SimulatorPacket);
            PropertyInfo[] properties = packetType.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                var aggregationValue = property.GetValue(aggregation);

                switch (aggregationValue)
                {
                    case byte aggregationByte:
                        property.SetValue(aggregation, (byte)(aggregationByte / packetCount));
                        break;

                    case float aggregationFloat:
                        property.SetValue(aggregation, (float)(aggregationFloat / packetCount));
                        break;

                    case double aggregationDouble:
                        property.SetValue(aggregation, (double)(aggregationDouble / packetCount));
                        break;

                    case int aggregationInt:
                        property.SetValue(aggregation, (int)(aggregationInt / packetCount));
                        break;

                    case short aggregationShort:
                        property.SetValue(aggregation, (short)(aggregationShort / packetCount));
                        break;

                    case long aggregationLong:
                        property.SetValue(aggregation, (long)(aggregationLong / packetCount));
                        break;

                    case Vector3 aggregationVector3 :
                        property.SetValue(aggregation, (Vector3)(aggregationVector3 / packetCount));
                        break;
                    default:
                        property.SetValue(aggregation, aggregationValue);
                        break;
                }
            }

            return aggregation;
        }
        internal static SimulatorPacket AggregatePacket(ref SimulatorPacket packet, ref SimulatorPacket aggregation)
        {
            Type packetType = typeof(SimulatorPacket);
            PropertyInfo[] properties = packetType.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                var packetValue = property.GetValue(packet);
                var aggregationValue = property.GetValue(aggregation);
                switch (packetValue)
                {
                    case byte packetByte when aggregationValue is byte:
                        // Handle Vector3 case
                        byte packetByteData = (byte)packetValue;
                        byte aggregationByte = (byte)aggregationValue;
                        aggregationValue = (byte)(aggregationByte + packetByteData);
                        property.SetValue(aggregation, aggregationValue);
                        break;
                    case float packetFloat when aggregationValue is float:

                        float packetFloatData = (float)packetValue;
                        float aggregationFloat = (float)aggregationValue;
                        aggregationValue = (float)(aggregationFloat + packetFloatData);
                        property.SetValue(aggregation, aggregationValue);
                        break;

                    case int packetInt when aggregationValue is int:

                        int packetIntData = (int)packetValue;
                        int aggregationInt = (int)aggregationValue;
                        aggregationValue = (int)(aggregationInt + packetIntData);
                        property.SetValue(aggregation, aggregationValue);
                        break;

                    case short packetShort when aggregationValue is short:

                        short packetShortData = (short)packetValue;
                        short aggregationShort = (short)aggregationValue;
                        aggregationValue = (short)(aggregationShort + packetShortData);
                        property.SetValue(aggregation, aggregationValue);
                        break;

                    case long packetLong when aggregationValue is long:

                        long packetLongData = (long)packetValue;
                        long aggregationLong = (long)aggregationValue;
                        aggregationValue = (long)(aggregationLong + packetLongData);
                        property.SetValue(aggregation, aggregationValue);
                        break;

                    case double packetDouble when aggregationValue is double:

                        double packetDoubleData = (double)packetValue;
                        double aggregationDouble = (double)aggregationValue;
                        aggregationValue = (double)(aggregationDouble + packetDoubleData);
                        property.SetValue(aggregation, aggregationValue);
                        break;

                    case SimulatorFlags packetFlags when aggregationValue is SimulatorFlags:

                        aggregationValue = (SimulatorFlags)packetValue;
                        property.SetValue(aggregation, aggregationValue);
                        break;

                    case Vector3 packetVector3 when aggregationValue is Vector3:

                        Vector3 packetVector3Data = (Vector3)packetValue;
                        Vector3 aggregationVector3 = (Vector3)aggregationValue;
                        aggregationValue = Vector3.Add(aggregationVector3, packetVector3Data);
                        property.SetValue(aggregation, aggregationValue);
                        break;

                    case TimeSpan packetTimeSpan when aggregationValue is TimeSpan:
                        aggregationValue = (TimeSpan)packetValue;
                        property.SetValue(aggregation, aggregationValue);
                        break;

                    case SimulatorInterfaceGameType packetGameType when aggregationValue is SimulatorInterfaceGameType:
                        aggregationValue = (SimulatorInterfaceGameType)packetValue;
                        property.SetValue(aggregation, aggregationValue);
                        break;

                    case DateTimeOffset packetDateTimeOffset when aggregationValue is DateTimeOffset:
                        aggregationValue = (DateTimeOffset)packetValue;
                        property.SetValue(aggregation, aggregationValue);
                        break;

                    case IPEndPoint packetIPEndPoint when aggregationValue is IPEndPoint:
                        aggregationValue = (IPEndPoint)packetValue;
                        Console.WriteLine(aggregationValue);
                        property.SetValue(aggregation, aggregationValue);
                        break;

                    case float[] packetFloatArray when aggregationValue is float[]:
                        aggregationValue = (float[])packetValue;
                        property.SetValue(aggregation, aggregationValue);
                        break;

                    default:
                        // Default case when none of the above conditions match
                        break;
                }
            }



            return aggregation;
        }
}
}