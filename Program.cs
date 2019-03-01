using System;

namespace CodingArchitect.TcpEchoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 6789;
            Console.WriteLine("Hello World!");
            SetupClient(port);
            Console.WriteLine("Press any key to exit");
            Console.Read();
        }

        public static async void SetupClient(int port)
        {
          SocketClient client = new SocketClient("localhost", port);
          var request = "Hi from programmatic client";
          var response = await client.SendRequest(request);
          Console.WriteLine("[Client] Server responded with '{0}' for the programmatic client's Request '{1}'", 
              request, response);
        }
    }
}
