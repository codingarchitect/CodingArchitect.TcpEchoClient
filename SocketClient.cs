using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CodingArchitect.TcpEchoClient
{
  public class SocketClient
  {
    private readonly IPAddress serverIPAddress;
    private readonly int serverPort;
    public SocketClient(string server, int serverPort)
    {
      this.serverIPAddress = findIPAddress(server);
      this.serverPort = serverPort;
    }

    private IPAddress findIPAddress(string server)
    {
      IPAddress ipAddress = null;
      IPHostEntry ipHostInfo = Dns.GetHostEntry(server);
      
      for (int i = 0; i < ipHostInfo.AddressList.Length; ++i) 
      {
        if (ipHostInfo.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
        {
          ipAddress = ipHostInfo.AddressList[i];
          break;
        }
      }
      if (ipAddress == null)
        throw new Exception("No IPv4 address for server");
      
      return ipAddress;
    }
    public async Task<string> SendRequest(string request)
    {
      try {
        
        TcpClient client = new TcpClient();
        await client.ConnectAsync(serverIPAddress, serverPort); // Connect
        using(NetworkStream networkStream = client.GetStream())
        using(StreamWriter writer = new StreamWriter(networkStream))
        using(StreamReader reader = new StreamReader(networkStream))
        {
          writer.AutoFlush = true;
          await writer.WriteAsync(request);
          var buffer = new byte[4096];
          var byteCount = await networkStream.ReadAsync(buffer, 0, buffer.Length);
          var response = Encoding.UTF8.GetString(buffer, 0, byteCount);
          client.Close();
          return response;
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }
  }
}