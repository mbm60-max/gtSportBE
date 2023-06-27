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
using PacketHelpers;
using PacketTimer;
using ServerMessages;
using LapCalculations;
using ServerHub;
using FullSimulatorPacket;

namespace PDTools.SimulatorInterfaceTestTool
{
    internal class Program
    {
        private static bool _showUnknown = false;
        private static IMongoDatabase _database;
        private static string _collectionName;

        private static IHost host;
        private static IHubContext<MyHub> hubContext;
        private static async Task Main(string[] args)
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
            ExtendedPacket extendedPacket = new ExtendedPacket();
            int packetCount = 0;
            SimulatorInterfaceClient simInterface = new SimulatorInterfaceClient(args[0], type);
            simInterface.OnReceive += (packet) => SimInterface_OnReceive(packet, ref throttleValue, hubContext, ref stopWatch, ref position, ref previousLap, ref inLapDistance, ref aggregation, ref packetCount, ref extendedPacket);// ,ref stopWatch

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

        private delegate void SimInterfaceEventHandler(SimulatorPacket packet, byte throttleValue, IHubContext<MyHub> hubContext, ref Stopwatch stopwatch, ref Vector3 position, ref short previousLap, ref float inLapDistance, ref SimulatorPacket aggregation, ref int packetCount,ref ExtendedPacket extendedPacket);//, ref Stopwatch stopwatch

        private static void SimInterface_OnReceive(SimulatorPacket packet, ref byte throttleValue, IHubContext<MyHub> hubContext, ref Stopwatch stopwatch, ref Vector3 position, ref short previousLap, ref float inLapDistance, ref SimulatorPacket aggregation, ref int packetCount,ref ExtendedPacket extendedPacket)//, ref Stopwatch stopwatch
        {
            // Print the packet contents to the console
            Console.SetCursorPosition(0, 0);
            //packet.PrintPacket(_showUnknown);
           // packet.PrintBasic(_showUnknown);
            byte x = packet.giveThrottle();
            short currentLap = packet.LapCount;
            packetCount++;
            if (LapCalc.LapCompleted(ref currentLap, ref previousLap))
            {
                //start timer,calc distance
                previousLap = currentLap;
                inLapDistance = 0;

            }
            //inLapDistance += getElapsedDistance(packet.MetersPerSecond, ElapsedTime);


            //Console.WriteLine("Current Throttle value: " +  throttleValue.ToString().PadLeft(3));
            if (stopwatch == null)
            {
                PacketTimerClass.StartTimer(ref stopwatch);
                PacketHelper.AggregatePacket(ref packet, ref aggregation);
                return;
            }
            else if (stopwatch.Elapsed.TotalSeconds > 0.1 && stopwatch != null)
            {
                PacketTimerClass.EndTimer(ref stopwatch);
                //throttleValue=packet.Throttle;
                //Message.TestMessage(throttleValue, hubContext);
                PacketHelper.AggregatePacket(ref packet, ref aggregation);
                Console.WriteLine($"Position:  {packet.Position}");
                Console.WriteLine();
                Console.WriteLine("Packet Count: " + packetCount);
                Console.WriteLine();
                // Reset packet count to 0
                PacketHelper.SummarizePacket(ref aggregation, ref packetCount);
                

                // Get the type of the SimulatorPacket and ExtendedPacket
                Type simulatorPacketType = typeof(SimulatorPacket);
                Type extendedPacketType = typeof(ExtendedPacket);

                // Get the properties of the SimulatorPacket
                PropertyInfo[] properties = simulatorPacketType.GetProperties();

                // Loop through the properties and copy their values
                foreach (PropertyInfo property in properties)
                {
                    object value = property.GetValue(packet);

                    // Find the corresponding property in the ExtendedPacket
                    PropertyInfo extendedProperty = extendedPacketType.GetProperty(property.Name);

                    // Copy the value to the ExtendedPacket if the property exists
                    if (extendedProperty != null && extendedProperty.PropertyType == property.PropertyType)
                    {
                        extendedProperty.SetValue(extendedPacket, value);
                    }else if(property.PropertyType == typeof(Vector3)){
                        Vector3 vectorValue = (Vector3)value;
                        string[] storageArray = new string[3];
                        storageArray[0] = vectorValue.X.ToString();
                        storageArray[1] = vectorValue.Y.ToString();
                        storageArray[2] = vectorValue.Z.ToString();
                        extendedProperty.SetValue(extendedPacket, storageArray);
                    }else if(extendedProperty != null){
                        extendedProperty.SetValue(extendedPacket, value.ToString());
                    }
                }
                extendedPacket.distanceFromStart = 5.0f;
                Console.WriteLine(extendedPacket.DateReceived);
                Console.WriteLine(extendedPacket.RoadPlane[0]);
                Message.PositionMessage(extendedPacket.Position,hubContext);
                //Message.StringMessage(extendedPacket.DateReceived,hubContext);
                //Message.FloatMessage(extendedPacket.distanceFromStart,hubContext);
                //Message.SimGameMessage(extendedPacket.GameType,hubContext);
                //Message.IntMessage(extendedPacket.PacketId,hubContext);
                //Message.ShortMessage(extendedPacket.LapCount,hubContext);
                //Message.FlagsMessage(extendedPacket.Flags,hubContext);
                //Message.FloatArrayMessage(extendedPacket.GearRatios,hubContext);
                Message.FullPacketMessage(extendedPacket, hubContext);
               // PropertyInfo[] properties2 = typeof(ExtendedPacket).GetProperties();
               // foreach (PropertyInfo property in properties2)
               // {
                 //    Console.WriteLine($"{property.Name}: {property.GetValue(extendedPacket)}");
               // }
                // extendedPacket now contains the values from originalPacket

                packetCount = 0;
                //Message.PositionMessage(packet.Position,hubContext);//seems to not be working 

                aggregation = new SimulatorPacket();
                //sendpacket
                //reset aggregation packet to default
                return;
            }
            PacketHelper.AggregatePacket(ref packet, ref aggregation);
        }

        //public static void InsertDocument(SimulatorPacket packet)
        //{
        //  var collection = _database.GetCollection<SimulatorPacket>(_collectionName);
        //collection.InsertOne(packet);
        //}

        private static IHostBuilder CreateHostBuilder(string[] args) =>
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



    }
}
