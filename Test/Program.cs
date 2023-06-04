using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PDTools.SimulatorInterface;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;



namespace PDTools.SimulatorInterfaceTestTool
{
    internal class Program
    {
        private static bool _showUnknown = false;
        private static IMongoDatabase _database;
        private static string _collectionName;

        private static IHost host;
        private static IHubContext<ThrottleHub> hubContext;
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
            Stopwatch stopWatch = null;
            SimulatorInterfaceClient simInterface = new SimulatorInterfaceClient(args[0], type);
            simInterface.OnReceive += (packet) => SimInterface_OnReceive(packet, ref throttleValue, ref stopWatch);

            var cts = new CancellationTokenSource();

            // Cancel token from outside source to end simulator
            var task = simInterface.Start(cts.Token);
            
            // Build the host
            var host = CreateHostBuilder(args).Build();

            // Start the host in the background
            _ = host.RunAsync();

            // Get the hub context
            var serviceProvider = host.Services;
            //Console.WriteLine(throttleValue);
            hubContext = serviceProvider.GetRequiredService<IHubContext<ThrottleHub>>();
            await hubContext.Clients.All.SendAsync("SendMessage", "max");
           // await hubContext.Clients.All.SendAsync("ReceiveThrottleValue", throttleValue);
           // Console.WriteLine(hubContext.Clients.All);
           

            try
            {
                //hubContext.Clients.All.SendAsync("ReceiveThrottleValue", throttleValue);
               // Console.WriteLine(hubContext);

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

        private delegate void SimInterfaceEventHandler(SimulatorPacket packet, byte throttleValue, ref Stopwatch stopwatch);

        private static void SimInterface_OnReceive(SimulatorPacket packet,ref byte throttleValue, ref Stopwatch stopwatch)
        {
            // Print the packet contents to the console
            Console.SetCursorPosition(0, 0);
            //packet.PrintPacket(_showUnknown);
            packet.PrintBasic(_showUnknown);
            byte x = packet.giveThrottle();
            if (x > 0){
                throttleValue = x;
                Console.WriteLine();  
                Console.WriteLine("Current Throttle value: " +  throttleValue.ToString().PadLeft(3));
                if (stopwatch == null){
                    StartTimer( ref stopwatch);
                }
                else if (x > 100 && stopwatch != null){
                    EndTimer(ref stopwatch);
                }
                 hubContext.Clients.All.SendAsync("ReceiveThrottleValue", throttleValue);
            }

            // Get the game type the packet was issued from
            SimulatorInterfaceGameType gameType = packet.GameType;
             //InsertDocument(packet);
            // Check on flags for whether the simulation is active
            if (packet.Flags.HasFlag(SimulatorFlags.CarOnTrack) && !packet.Flags.HasFlag(SimulatorFlags.Paused) && !packet.Flags.HasFlag(SimulatorFlags.LoadingOrProcessing)){
                // Do stuff with packet
                //InsertDocument(packet);
            }
             // Send throttle value to connected clients
             //if(throttleValue!= null){
               // hubContext.Clients.All.SendAsync("ReceiveThrottleValue", throttleValue);
             //}
        }

        public static void InsertDocument(SimulatorPacket packet)
        {
            var collection = _database.GetCollection<SimulatorPacket>(_collectionName);
            collection.InsertOne(packet);
        }

        public static void StartTimer( ref Stopwatch stopwatch)
        {
            //update the timer refrence to be a new timer object miliseconds
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public static void EndTimer(ref Stopwatch stopwatch){
            if (stopwatch != null)
            {
                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                Console.WriteLine("RunTime " + elapsedTime);
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
                    endpoints.MapHub<ThrottleHub>("/throttlehub");
                });
            });
        });

    }
    

    public class ThrottleHub : Hub
    {
        //public async Task ReceiveThrottleValue(byte throttleValue)
        //{
          //  Console.WriteLine($"Received throttle value: {throttleValue}");
            //await Clients.All.SendAsync("ReceiveThrottleValue", throttleValue);
        //}
        public async Task SendMessage(string name)
        {
            try{
                Console.WriteLine($"Received throttle value: {name}");
                //await Clients.All.SendAsync("SendMessage", name);
            }catch (Exception ex)
    {
        // Handle the exception and log the error
        Console.WriteLine($"Error in ReceiveThrottleValue: {ex.Message}");
    }
        }
    }

}