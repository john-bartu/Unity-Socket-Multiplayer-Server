using System;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;

namespace UnitySocketMultiplayerServer
{
    class Client
    {
        TcpClient client;
        NetworkStream stream;
        StreamReader sr;
        StreamWriter sw;


        public Client(TcpClient newClient)
        {
            client = newClient;
            stream = client.GetStream();
            sr = new StreamReader(stream);
            sw = new StreamWriter(stream);
            Console.WriteLine("Client Stream Tunel created");
        }

        public void Update()
        {
            Console.WriteLine("Thread Started");
            try
            {
                while (true)
                {
                    /*
                      byte[] buffer = new byte[1024];
                      stream.Read(buffer, 0, buffer.Length);
                      int recv = 0;
                      foreach (byte b in buffer)
                      {
                          if (b != 0)
                          {
                              recv++;
                          }
                      }
                      string request = Encoding.UTF8.GetString(buffer, 0, recv);*/

                    string received = sr.ReadLine();
                    Console.WriteLine("CLIENT->SRV: " + received);


                    string response = "You rock!";
                    sw.WriteLine(response);
                    sw.Flush();
                    Console.WriteLine("SRV->Client: " + response);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong.");
                sw.WriteLine(e.ToString());
            }

            Console.WriteLine("Thread Ended");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 1302);
            listener.Start();
            while (true)
            {
                Console.WriteLine("Waiting for a connection.");
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client accepted.");

                Client newClient = new Client(client);

                Thread InstanceCaller = new Thread(new ThreadStart(newClient.Update));
                InstanceCaller.Start();
            }
        }
    }
}