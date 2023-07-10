using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ServerHub{
     internal class MyHub : Hub
        {
            public string usernameResponse;

            public async Task NewMessage(byte data)
            {
                //while (true){
                await Clients.All.SendAsync("messageReceived", data);
                //  await Task.Delay(1000);
                //}
            }
            public async Task SendResponse(string response)
        {
            // Process the received response
            // For example, you can log it or perform any desired actions
            Console.WriteLine("Received response from client: " + response);
            this.usernameResponse=response;

            // You can also send a response back to the client if needed
            // For example:
            // await Clients.Caller.SendAsync("responseReceived", "Response received successfully");
        }
        }
}