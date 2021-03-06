﻿using System;
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
                Thread.Sleep(1000);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("server:admin> ");

                string input = Console.ReadLine();
                string[] parts = input.Split(' ');
                string command = "";
                string argument = "";

                if (parts.Length > 0)
                    command = parts[0];
                if (parts.Length > 1)
                    argument = parts[1];

                switch (command)
                {
                    case "stop":
                        {
                            ClientController.StopServer();
                            break;
                        }

                    case "stat":
                        {
                            if (argument == "")
                                Statistics.PrintStatistics();
                            else
                            {
                                Guid guid = Guid.Parse(argument);
                                if (Statistics.IsExist(guid))
                                    Statistics.GetStat(guid).PrintStat();
                                else
                                    Debug.LogError("Provided");
                            }
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
