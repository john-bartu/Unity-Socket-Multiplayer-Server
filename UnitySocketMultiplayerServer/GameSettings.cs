using System;
using System.Collections.Generic;
using System.Text;

namespace UnitySocketMultiplayerServer
{
    public static class GameSettings
    {
        static long startTime;
        public static void InitGame()
        {
            startTime = (DateTime.Now.Ticks / TimeSpan.TicksPerSecond);
        }

        public static float GetTime()
        {
            long currentTime = (DateTime.Now.Ticks / TimeSpan.TicksPerSecond);
            float time = (currentTime - startTime);
            return time;
        }
    }
}
