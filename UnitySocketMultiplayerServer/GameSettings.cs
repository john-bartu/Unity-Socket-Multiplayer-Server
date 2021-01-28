using System;
using System.Collections.Generic;
using System.Text;

namespace UnitySocketMultiplayerServer
{
    public static class GameSettings
    {
        static long startTime;

        /// <summary>
        /// Init world and check current time
        /// </summary>
        public static void InitGame()
        {
            startTime = (DateTime.Now.Ticks / TimeSpan.TicksPerSecond);
        }

        /// <summary>
        /// Calculate time elapsed from start of game
        /// </summary>
        /// <returns>Time elapsed of game</returns>
        public static float GetTime()
        {
            long currentTime = (DateTime.Now.Ticks / TimeSpan.TicksPerSecond);
            float time = (currentTime - startTime);
            return time;
        }
    }
}
