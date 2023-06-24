using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ServerHub{
     internal class MyHub : Hub
        {

            public async Task NewMessage(byte data)
            {
                //while (true){
                await Clients.All.SendAsync("messageReceived", data);
                //  await Task.Delay(1000);
                //}
            }
        }
}