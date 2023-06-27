using Microsoft.AspNetCore.SignalR;
using System.Numerics;
using System.Threading.Tasks;
using ServerHub;
using PDTools.SimulatorInterface;
using FullSimulatorPacket;


namespace ServerMessages{
    internal static class Message{
        private static IHubContext<MyHub> hubContext;
        internal static async void TestMessage(byte data, IHubContext<MyHub> hubContext)
        {
            await hubContext.Clients.All.SendAsync("messageReceived", data);
        }
        internal static async void PositionMessage(string[] position, IHubContext<MyHub> hubContext)
        {
            await hubContext.Clients.All.SendAsync("positionMessage", position);
        }
        internal static async void FullPacketMessage(ExtendedPacket packet, IHubContext<MyHub> hubContext)
        {
            await hubContext.Clients.All.SendAsync("fullPacketMessage", packet);
        }
        internal static async void StringMessage(string String, IHubContext<MyHub> hubContext)
        {
            await hubContext.Clients.All.SendAsync("stringMessage", String);
        }
        internal static async void FloatMessage(float Float, IHubContext<MyHub> hubContext)
        {
            await hubContext.Clients.All.SendAsync("floatMessage", Float);
        }
        internal static async void SimGameMessage(SimulatorInterfaceGameType SimGame, IHubContext<MyHub> hubContext)
        {
            await hubContext.Clients.All.SendAsync("simGameMessage", SimGame);
        }
        internal static async void IntMessage(int Int, IHubContext<MyHub> hubContext)
        {
            await hubContext.Clients.All.SendAsync("intMessage", Int);
        }
        internal static async void ShortMessage(short Short, IHubContext<MyHub> hubContext)
        {
            await hubContext.Clients.All.SendAsync("shortMessage", Short);
        }
        internal static async void FlagsMessage(SimulatorFlags flags, IHubContext<MyHub> hubContext)
        {
            await hubContext.Clients.All.SendAsync("flagsMessage", flags);
        }
        internal static async void FloatArrayMessage(float[] floatArr, IHubContext<MyHub> hubContext)
        {
            await hubContext.Clients.All.SendAsync("floatArrMessage", floatArr);
        }
    }
}