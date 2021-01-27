using System;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace UnitySocketMultiplayerServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing...");
            Debug.LogInfo("Creating Database...");
            Database.Init();
            Debug.LogInfo("...created.");

            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 3389);
            listener.Start();

            ClientController.onServerStart();
            Debug.SetSilent(false);

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