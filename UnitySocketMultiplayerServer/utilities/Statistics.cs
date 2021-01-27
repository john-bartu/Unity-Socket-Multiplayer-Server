using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace UnitySocketMultiplayerServer
{
    class Stat
    {
        readonly Stopwatch statStopwatch;
        readonly string client;
        long download;
        long upload;

        public Stat(string name)
        {
            download = 0;
            upload = 0;
            statStopwatch = new Stopwatch();
            statStopwatch.Start();
            client = name;
        }

        public void LogDownloaded(int bytes)
        {
            download += bytes;
        }
        public void LogUploaded(int bytes)
        {
            upload += bytes;
        }
        public double GetTime()
        {
            return statStopwatch.ElapsedMilliseconds / 1000;
        }

        public double GetDownload()
        {
            return download / 1024;
        }

        public double GetUpload()
        {
            return upload / 1024;

        }

        public void PrintStat()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("----------STAT----------");
            Console.WriteLine("[ID]: " + client);
            Console.WriteLine("[UP]: " + GetUpload() + " KB");
            Console.WriteLine("[DO]: " + GetDownload() + " KB");
            Console.WriteLine("[TIME]: " + statStopwatch.ElapsedMilliseconds / 1000 + " s");
            Console.WriteLine("------------------------");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    class Statistics
    {
        static readonly Dictionary<Guid, Stat> clientList = new Dictionary<Guid, Stat>();
        static readonly Stopwatch serverStopWatch = new Stopwatch();

        static Statistics()
        {
            serverStopWatch.Start();
        }

        public static Stat GetStat(Guid uid)
        {
            if (clientList.ContainsKey(uid))
                return clientList[uid];
            else
            {
                Stat newStat = new Stat(uid.ToString().Substring(0, 8));
                clientList.Add(uid, newStat);

                return newStat;
            }
        }

        public static void PrintStatistics()
        {
            double download = 0;
            double upload = 0;

            Dictionary<Guid, Stat>.Enumerator enumerator = clientList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                download += enumerator.Current.Value.GetDownload();
                upload += enumerator.Current.Value.GetUpload();
            }

            double time = serverStopWatch.ElapsedMilliseconds / 1000;

            if (time < 0.001) time = 1; // Avoid divide per zero

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("----------STAT----------");
            Console.WriteLine("[UP]: " + upload + " KB");
            Console.WriteLine("[DO]: " + download + " KB");
            Console.WriteLine("[ServerTime]: " + time + " s");
            Console.WriteLine("------------------------");
            Console.WriteLine();
            Console.WriteLine("--------STAT-AVG--------");
            Console.WriteLine("[UP]: " + upload / time + " KB/s");
            Console.WriteLine("[DO]: " + download / time + " KB/s");
            Console.WriteLine("------------------------");
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}
