using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UnitySocketMultiplayerServer
{
    class ClientController
    {
        static bool isRun = true;
        static readonly Dictionary<Guid, Client> clientMap = new Dictionary<Guid, Client>();

        public static void AcceptClient(TcpClient clientSocket)
        {
            Guid uid = Guid.NewGuid();
            Client newclient = new Client(clientSocket, uid);
            clientMap.Add(uid, newclient);
            Thread InstanceCaller = new Thread(new ThreadStart(newclient.Update));
            InstanceCaller.Start();
        }

        public static void FreeClient(Client client)
        {
            Statistics.GetStat(client.GetUID()).PrintStat();
            clientMap.Remove(client.GetUID());
        }

        public static void OnServerStart()
        {
            // Start InputHandler for console commands
            GameSettings.InitGame();
            Thread InstanceCaller = new Thread(new ThreadStart(InputHandler.Update));
            InstanceCaller.Start();
        }

        public static void StopServer()
        {
            isRun = false;
            OnServerClose();
        }

        public static void OnServerClose()
        {
            Statistics.PrintStatistics();
        }

        public static bool IsRunning()
        {
            return isRun;
        }

    }
}
