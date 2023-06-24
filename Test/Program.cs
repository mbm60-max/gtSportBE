using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Numerics;
using PDTools.SimulatorInterface;
using System.Reflection;
using Syroot.BinaryData.Memory;
using System.IO;

namespace PDTools.SimulatorInterfaceTestTool
{
    internal class Program
    {
        private static bool _showUnknown = false;
        private static IMongoDatabase _database;
        private static string _collectionName;

        private static IHost host;
        private static IHubContext<MyHub> hubContext;
        static async Task Main(string[] args)
        {
            
            /* Mostly a test sample for using the Simulator Interface library */
            //var connectionString = "mongodb+srv://maxbm:Kismetuni66@pdtoolcluster.een0p7c.mongodb.net/?retryWrites=true&w=majority";
            //var client = new MongoClient(connectionString);
            //_database = client.GetDatabase("Test");
            //_collectionName = "Test Collection";
            //_database.CreateCollection(_collectionName);

            Console.WriteLine("Simulator Interface GT7/GTSport/GT6 - Nenkai#9075");
            Console.WriteLine();

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: SimulatorInterface.exe <IP address of PS4/PS5> ('--gtsport' for GT Sport support, --gt6 for GT6 support, optional: '--debug' to show unknown values)");
                return;
            }

            _showUnknown = args.Contains("--debug");
            bool gtsport = args.Contains("--gtsport");
            bool gt6 = args.Contains("--gt6");

            if (gtsport && gt6)
            {
                Console.WriteLine("Error: Both GT6 and GT Sport arguments are present.");
                return;
            }

            Console.WriteLine("Starting interface..");

            SimulatorInterfaceGameType type = SimulatorInterfaceGameType.GT7;
            if (gtsport)
                type = SimulatorInterfaceGameType.GTSport;
            else if (gt6)
                type = SimulatorInterfaceGameType.GT6;

            byte throttleValue = 0;
            Vector3 position = new Vector3(0, 0, 0);
            // Build the host
            host = CreateHostBuilder(args).Build();

            // Start the host in the background
            _ = host.RunAsync();

            // Get the hub context
            var serviceProvider = host.Services;
            hubContext = serviceProvider.GetRequiredService<IHubContext<MyHub>>();


            Stopwatch stopWatch = null;
            short previousLap = 0;
            float inLapDistance = 0;
            SimulatorPacket aggregation = new SimulatorPacket { };
            int packetCount = 0;
            SimulatorInterfaceClient simInterface = new SimulatorInterfaceClient(args[0], type);
            simInterface.OnReceive += (packet) => SimInterface_OnReceive(packet, ref throttleValue, hubContext, ref stopWatch, ref position, ref previousLap, ref inLapDistance, ref aggregation, ref packetCount);// ,ref stopWatch

            var cts = new CancellationTokenSource();

            // Cancel token from outside source to end simulator
            var task = simInterface.Start(cts.Token);


            try
            {
                await task;
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine($"Simulator Interface ending..");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Errored during simulation: {e.Message}");
            }
            finally
            {
                // Important to clear up underlying socket
                simInterface.Dispose();

                // Stop the host
                await host.StopAsync();
                host.Dispose();

            }
        }

        private delegate void SimInterfaceEventHandler(SimulatorPacket packet, byte throttleValue, IHubContext<MyHub> hubContext, ref Stopwatch stopwatch, ref Vector3 position, ref short previousLap, ref float inLapDistance, ref SimulatorPacket aggregation, ref int packetCount);//, ref Stopwatch stopwatch

        private static void SimInterface_OnReceive(SimulatorPacket packet, ref byte throttleValue, IHubContext<MyHub> hubContext, ref Stopwatch stopwatch, ref Vector3 position, ref short previousLap, ref float inLapDistance, ref SimulatorPacket aggregation, ref int packetCount)//, ref Stopwatch stopwatch
        {
            // Print the packet contents to the console
            Console.SetCursorPosition(0, 0);
            //packet.PrintPacket(_showUnknown);
            //packet.PrintBasic(_showUnknown);
            byte x = packet.giveThrottle();
            short currentLap = packet.LapCount;
            packetCount++;
            if (LapCompleted(ref currentLap, ref previousLap))
            {
                //start timer,calc distance
                previousLap = currentLap;
                inLapDistance = 0;

            }
            //inLapDistance += getElapsedDistance(packet.MetersPerSecond, ElapsedTime);


            //Console.WriteLine("Current Throttle value: " +  throttleValue.ToString().PadLeft(3));
            if (stopwatch == null)
            {
                StartTimer(ref stopwatch);
                AggregatePacket(ref packet, ref aggregation);
                return;
            }
            else if (stopwatch.Elapsed.TotalSeconds > 0.1 && stopwatch != null)
            {
                EndTimer(ref stopwatch);
                TestMessage(throttleValue, hubContext);
                AggregatePacket(ref packet, ref aggregation);
                Console.WriteLine($"Position:  {packet.Position}");
                Console.WriteLine();
                Console.WriteLine("Packet Count: " + packetCount);
                Console.WriteLine();
                // Reset packet count to 0
                SummarizePacket(ref aggregation, ref packetCount);
                packetCount = 0;
                //PositionMessage(packet.Position,hubContext);//seems to not be working 
                
                aggregation = new SimulatorPacket();
                //sendpacket
                //reset aggregation packet to default
                return;
            }
            AggregatePacket(ref packet, ref aggregation);
            


            // Get the game type the packet was issued from
            //SimulatorInterfaceGameType gameType = packet.GameType;
            //InsertDocument(packet);
            // Check on flags for whether the simulation is active
            //if (packet.Flags.HasFlag(SimulatorFlags.CarOnTrack) && !packet.Flags.HasFlag(SimulatorFlags.Paused) && !packet.Flags.HasFlag(SimulatorFlags.LoadingOrProcessing)){
            // Do stuff with packet
            //InsertDocument(packet);
            //}
        }

        //public static void InsertDocument(SimulatorPacket packet)
        //{
        //  var collection = _database.GetCollection<SimulatorPacket>(_collectionName);
        //collection.InsertOne(packet);
        //}

        public static void StartTimer(ref Stopwatch stopwatch)
        {
            //update the timer refrence to be a new timer object miliseconds
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }
        public static Boolean LapCompleted(ref short currentLap, ref short previousLap)
        {
            if ((currentLap > 0) && (currentLap > previousLap))
            {
                return true;
            }
            return false;
        }

        public static void RecordDistance(float MetersPerSecond, TimeSpan ElapsedTime)
        {


        }

        public static SimulatorPacket AggregatePacket(ref SimulatorPacket packet, ref SimulatorPacket aggregation)
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

        public static void EndTimer(ref Stopwatch stopwatch)
        {
            if (stopwatch != null)
            {
                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                Console.WriteLine("RunTime " + elapsedTime);
                stopwatch = null;
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.ConfigureServices(services =>
            {
                services.AddCors(options =>
                {
                    options.AddDefaultPolicy(builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
                });

                services.AddSignalR();
            });

            webBuilder.ConfigureLogging(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Information);
                logging.AddConsole();
            });

            webBuilder.Configure(app =>
            {
                app.UseRouting();
                app.UseDeveloperExceptionPage();

                app.UseCors(); // Add this line to enable CORS

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<MyHub>("/throttlehub");
                });
            });
        });
        public class MyHub : Hub
        {

            public async Task NewMessage(byte data)
            {
                //while (true){
                await Clients.All.SendAsync("messageReceived", data);
                //  await Task.Delay(1000);
                //}
            }
        }
        private static async void TestMessage(byte data, IHubContext<MyHub> hubContext)
        {
            await hubContext.Clients.All.SendAsync("messageReceived", data);
        }
        private static async void PositionMessage(Vector3 position, IHubContext<MyHub> hubContext)
        {
            await hubContext.Clients.All.SendAsync("positionMessage", position);
        }
        public static SimulatorPacket SummarizePacket(ref SimulatorPacket aggregation, ref int packetCount)
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
    }
}
