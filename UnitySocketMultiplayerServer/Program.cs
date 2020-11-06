using System;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;

namespace UnitySocketMultiplayerServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 1302);
            listener.Start();

            ClientController.onServerStart();

            while (ClientController.isRunning())
            {
                Debug.LogInfo("Waiting for a connection.");
                TcpClient clientSocket = listener.AcceptTcpClient();
                Debug.LogInfo("Client accepted.");
                ClientController.AcceptClient(clientSocket);

                //Get some time for the server for another connection
                Thread.Sleep(1000);
            }
        }
    }
}