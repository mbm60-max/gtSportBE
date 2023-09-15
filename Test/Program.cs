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
using PacketArrayHelpers;
using MongoHelpers;
using FileUtils;
using YouTubeHandlers;
using ChallengeHandlers;
namespace PDTools.SimulatorInterfaceTestTool
{
    
    internal class Program
    {   
        private static string username;

        public static string Username
        {
            get { return username; }
            set { username = value; }
        }

        private static bool _showUnknown = false;
        private static IMongoDatabase _database;
        private static string _collectionName;

        private static IHost host;
        private static IHubContext<MyHub> hubContext;
        
        private static async Task Main(string[]args)
        {   
//int initialDelayMilliseconds = 1000; // Initial delay in milliseconds
     //   int maxAttempts = 1000; // Maximum number of attempts

     //  List<string> argArray;
      //  int attempt = 1;
       //   do
       // {
         //   Console.WriteLine("running");
         //   argArray = await DataHelper.ReadDataFromFile("data.txt");

          // if (argArray.Count == 0 || argArray[0] == "")
          //  {
                // Exponential backoff
             //   int currentDelay = initialDelayMilliseconds * (int)Math.Pow(2, attempt - 1);
              //  await Task.Delay(currentDelay);
              //  attempt++;
//
             //   if (attempt > maxAttempts)
              //  {
                //    Console.WriteLine("Maximum number of attempts reached. Exiting.");
                //    return;
              //  }
           // }
       // }
        //while (argArray.Count == 0);

        //foreach (string value in argArray)
        //{
        //     Console.WriteLine(argArray.Count);
// to test 
// 192.168.1.200, 255.255.255.0, 192.168.1.1, 212.132.163.178
        //tested
        //
        //}
    //string[]args= {"212.132.163.178","--gt","sport"};// was for ps4 "212.132.163.178"
            byte throttleValue = 0;
            Vector3 position = new Vector3(0, 0, 0);
            // Build the host
            host = CreateHostBuilder().Build();

            // Start the host in the background
            _ = host.RunAsync();

            // Get the hub context
            var serviceProvider = host.Services;

            hubContext = serviceProvider.GetRequiredService<IHubContext<MyHub>>();
          
            ExtendedPacketArrays ExtendedPacketArrays = new ExtendedPacketArrays();
            PacketArrayHelperClass packetArrayhelper = new PacketArrayHelperClass();

            MongoDBHelper MongoHelper= new MongoDBHelper();
            Stopwatch stopWatch = null;
            Stopwatch lapTimeStopWatch = null;
            double inLapDistance = 0.0;
            SimulatorPacket aggregation = new SimulatorPacket { };
            ExtendedPacket extendedPacket = new ExtendedPacket();
            int packetCount = 0;
            int inLapShifts = 0;
            double packetReceivalTime = 0.0;
            byte GearSelected = 0;
            
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
YouTubeHandler YouTubeHandler= new YouTubeHandler();
        YouTubeHandler.SearchAndUploadVideos();
                   
ChallengeHandler ChallengeHandler= new ChallengeHandler();
        ChallengeHandler.UploadChallenges();
         Console.WriteLine("hehe");
            // Use the data received from the hub
          
            SimulatorInterfaceGameType type = SimulatorInterfaceGameType.GT7;
            if (gtsport)
                type = SimulatorInterfaceGameType.GTSport;
            else if (gt6)
                type = SimulatorInterfaceGameType.GT6;


            SimulatorInterfaceClient simInterface = new SimulatorInterfaceClient(args[0], type);
            short previousLap = 0;
            simInterface.OnReceive += (packet) => SimInterface_OnReceive(packet, ref throttleValue, hubContext, ref stopWatch, ref position,  ref inLapDistance, ref aggregation, ref packetCount, ref extendedPacket, ref previousLap, ref lapTimeStopWatch, ref inLapShifts, ref GearSelected,ref packetReceivalTime, ref ExtendedPacketArrays, ref packetArrayhelper, ref MongoHelper);

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

        private delegate void SimInterfaceEventHandler(SimulatorPacket packet, byte throttleValue, IHubContext<MyHub> hubContext, ref Stopwatch stopwatch, ref Vector3 position,  ref double inLapDistance, ref SimulatorPacket aggregation, ref int packetCount,ref ExtendedPacket extendedPacket, ref short previousLap, ref Stopwatch lapTimeStopWatch,ref int inLapShifts, ref byte GearSelected, ref double packetReceivalTime, ref ExtendedPacketArrays ExtendedPacketArrays, ref PacketArrayHelperClass packetArrayhelper, ref MongoDBHelper MongoHelper);//, ref Stopwatch stopwatch

        private static void SimInterface_OnReceive(SimulatorPacket packet, ref byte throttleValue, IHubContext<MyHub> hubContext, ref Stopwatch stopwatch, ref Vector3 position, ref double inLapDistance, ref SimulatorPacket aggregation, ref int packetCount,ref ExtendedPacket extendedPacket, ref short previousLap, ref Stopwatch lapTimeStopWatch, ref int inLapShifts, ref byte GearSelected, ref double packetReceivalTime, ref ExtendedPacketArrays ExtendedPacketArrays, ref PacketArrayHelperClass packetArrayhelper, ref MongoDBHelper MongoHelper)//, ref Stopwatch stopwatch
        {
            //string username = hubContext.usernameResponse;
            string username = "5";
            Boolean mongoReady = false;
            if(username != null){
               mongoReady = true;
            }
            Boolean IsNewLap = false;
            // Print the packet contents to the console
            //Console.SetCursorPosition(0, 0);
            //packet.PrintPacket(_showUnknown);
            //packet.PrintBasic(_showUnknown);
            //byte x = packet.giveThrottle();
            short currentLap = packet.LapCount;
            if (packet.CurrentGear != GearSelected)
            {
                bool currentGearIsLarger = packet.CurrentGear > GearSelected;
                byte dif;
                if (currentGearIsLarger)
                {
                     dif = (byte)(packet.CurrentGear - GearSelected);
                }
                else
                {
                     dif = (byte)(GearSelected - packet.CurrentGear);
                }
                GearSelected = packet.CurrentGear;
                inLapShifts += dif;
            }

            if ((packet.Flags & SimulatorFlags.Paused) != 0)
            {
                PacketTimerClass.PauseTimer(ref lapTimeStopWatch);
            }
            else if ((packet.Flags & SimulatorFlags.Paused) == 0)
            {
                PacketTimerClass.ResumeTimer(ref lapTimeStopWatch);
            }
            if(currentLap>previousLap){
                previousLap=currentLap;
                lapTimeStopWatch=null;
                IsNewLap = true;
            }
            if (lapTimeStopWatch == null)
            {
                PacketTimerClass.StartTimer(ref lapTimeStopWatch);
            }
            packetCount++;
            if (LapCalc.LapCompleted(ref currentLap, ref previousLap))
            {
                //start timer,calc distance
                previousLap = currentLap;
                inLapDistance = 0;

            }
            if(stopwatch!=null){
                            inLapDistance += LapCalc.RecordDistance(packet.MetersPerSecond, stopwatch.Elapsed.TotalSeconds, packetReceivalTime);
            }
            if (stopwatch == null)
            {
                PacketTimerClass.StartTimer(ref stopwatch);
                PacketHelper.AggregatePacket(ref packet, ref aggregation);
                return;
            }
            else if (stopwatch.Elapsed.TotalSeconds > 0.1 && stopwatch != null)
            {
                PacketTimerClass.EndTimer(ref stopwatch);
                PacketHelper.AggregatePacket(ref packet, ref aggregation);
                //Console.WriteLine($"Position:  {packet.Position}");
                //Console.WriteLine();
               // Console.WriteLine("Packet Count: " + packetCount);
               // Console.WriteLine();
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
                extendedPacket.distanceFromStart = inLapDistance;
                extendedPacket.LapTiming=lapTimeStopWatch.Elapsed.TotalSeconds.ToString("0.########");;//convert to string
                extendedPacket.InLapShifts=inLapShifts;
                packetArrayhelper.ProcessExtendedPacket(extendedPacket, ExtendedPacketArrays, IsNewLap);
                if(IsNewLap && mongoReady){
                 MongoHelper.InsertExtendedPacket(username,ExtendedPacketArrays,"UserSessions");
                }
                //Message.PositionMessage(extendedPacket.Position,hubContext);
                Message.FullPacketMessage(extendedPacket, hubContext);

                packetCount = 0;

                aggregation = new SimulatorPacket();
                //sendpacket
                //reset aggregation packet to default
                packetReceivalTime=0.0;//set elapsed time back to 0
                return;
            }
            if(IsNewLap){
                inLapShifts=0;
            }
            if(IsNewLap && mongoReady){
                if(currentLap>1){
                MongoHelper.InsertExtendedPacket(username,ExtendedPacketArrays,"UserSessions");
                }
                }
            PacketHelper.AggregatePacket(ref packet, ref aggregation);
            packetReceivalTime = stopwatch.Elapsed.TotalSeconds;
        }

        private static IHostBuilder CreateHostBuilder() =>
    Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.ConfigureServices(services =>
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",builder =>
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

                app.UseCors("CorsPolicy"); // Add this line to enable CORS

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<MyHub>("/server");
                });
            });
        });



    }
}
