using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;

namespace UnitySocketMultiplayerServer
{
    class InputHandler
    {

        public static void Update()
        {

            while (Thread.CurrentThread.IsAlive)
            {
                Console.ForegroundColor=ConsoleColor.Yellow;
                Console.Write("server:admin> ");

                string input = Console.ReadLine();
                string[] parts = input.Split(' ');
                string command = "";
                string argument = "";

                if(parts.Length>0)
                    command = parts[0];
                if (parts.Length>1 )
                    argument = parts[1];
                


                switch (command)
                {
                    case "stop":
                        {
                            ClientController.stopServer();
                            break;
                        }

                    case "stat":
                        {
                                Statistics.PrintStatistics();
                            break;
                        }

                    default:
                        {
                            Debug.LogError("Nierozpoznana komenda.");
                            break;
                        }
                }
                Console.ForegroundColor = ConsoleColor.White;

            }
        }
    }
}
