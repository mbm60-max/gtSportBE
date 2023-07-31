using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ServerHub{
     internal class MyHub : Hub
        {

             private static string receivedIPAddress;
        private static string receivedUsername;
        private static string receivedGameType;
        private static string usernameResponse;
        private static bool hasWrittenToFile = false;

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
            usernameResponse=response;

            // You can also send a response back to the client if needed
            // For example:
            // await Clients.Caller.SendAsync("responseReceived", "Response received successfully");
        }
        public async void ConnectionUserName(string userName)
    {
       receivedUsername = userName;
            await CheckAndWriteToFile();
        // Implement the logic for handling the 'ConnectionEstablished' method here.
        // You can save the userName or perform any other required actions.
        // For example, you might want to associate the user with their SignalR connection.
        Console.WriteLine("Wow"+userName);
        
    }
      public async void ConnectionIPAddress(string IPAddress)
    {
   receivedIPAddress = IPAddress;
            await CheckAndWriteToFile();
        // Implement the logic for handling the 'ConnectionEstablished' method here.
        // You can save the userName or perform any other required actions.
        // For example, you might want to associate the user with their SignalR connection.
        Console.WriteLine("Wow"+IPAddress);
     
    }
     public async void ConnectionGameType(string gameType)
    {
         receivedGameType = gameType;
            await CheckAndWriteToFile();
        // Implement the logic for handling the 'ConnectionEstablished' method here.
        // You can save the userName or perform any other required actions.
        // For example, you might want to associate the user with their SignalR connection.
        Console.WriteLine("Wow"+gameType);  
      
    }
    private async Task CheckAndWriteToFile()
        {
             Console.WriteLine(!hasWrittenToFile);
             Console.WriteLine(!string.IsNullOrEmpty(receivedUsername));
             Console.WriteLine( !string.IsNullOrEmpty(receivedIPAddress));
             Console.WriteLine(!string.IsNullOrEmpty(receivedGameType));
            if (!hasWrittenToFile && !string.IsNullOrEmpty(receivedUsername)
                && !string.IsNullOrEmpty(receivedIPAddress)
                && !string.IsNullOrEmpty(receivedGameType))
            {
            string fileName = "data.txt";
            string dataToWrite = $"Username: {receivedUsername}, IPAddress: {receivedIPAddress}, GameType: {receivedGameType}";

            // Check if the file already exists
            if (File.Exists(fileName))
            {
                // If the file exists, remove it before creating a new one
                File.Delete(fileName);
            }

            // Create a new file and write the data to it
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                await writer.WriteLineAsync(dataToWrite);
            }

            hasWrittenToFile = true;

            // Optionally, you can send a response back to the client after writing to the file
        }
        }
         }
}