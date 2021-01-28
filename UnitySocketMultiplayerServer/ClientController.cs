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


        /// <summary>
        /// Accept client socket and create new Thread
        /// </summary>
        /// <param name="clientSocket">Connected client TCP socket</param>
        public static void AcceptClient(TcpClient clientSocket)
        {
            Guid uid = Guid.NewGuid();
            Client newclient = new Client(clientSocket, uid);
            clientMap.Add(uid, newclient);
            Thread InstanceCaller = new Thread(new ThreadStart(newclient.Update));
            InstanceCaller.Start();
        }

        /// <summary>
        /// Remove player from map, and print statistics
        /// </summary>
        /// <param name="client">Client object to remove</param>
        public static void FreeClient(Client client)
        {
            Statistics.GetStat(client.GetUID()).PrintStat();
            clientMap.Remove(client.GetUID());
        }

        /// <summary>
        /// Only on server starts
        /// </summary>
        public static void OnServerStart()
        {
            // Start InputHandler for console commands
            GameSettings.InitGame();
            Thread InstanceCaller = new Thread(new ThreadStart(InputHandler.Update));
            InstanceCaller.Start();
        }


        /// <summary>
        /// Only on server stop
        /// </summary>
        public static void StopServer()
        {
            isRun = false;
            OnServerClose();
        }

        /// <summary>
        /// Only on server close (after stop)
        /// </summary>
        public static void OnServerClose()
        {
            Statistics.PrintStatistics();
        }

        /// <summary>
        /// Check if servers is running
        /// </summary>
        /// <returns>True if server is running</returns>
        public static bool IsRunning()
        {
            return isRun;
        }

    }
}
