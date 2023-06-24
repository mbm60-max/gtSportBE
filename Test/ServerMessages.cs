using Microsoft.AspNetCore.SignalR;
using System.Numerics;
using System.Threading.Tasks;
using ServerHub;

namespace ServerMessages{
    internal static class Message{
        private static IHubContext<MyHub> hubContext;
        internal static async void TestMessage(byte data, IHubContext<MyHub> hubContext)
        {
            await hubContext.Clients.All.SendAsync("messageReceived", data);
        }
        internal static async void PositionMessage(Vector3 position, IHubContext<MyHub> hubContext)
        {
            await hubContext.Clients.All.SendAsync("positionMessage", position);
        }
    }
}