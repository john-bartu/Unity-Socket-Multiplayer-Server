using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitySocketMultiplayerServer
{
    public class Player
    {
        public int Id { get; set; }
        public string Login { get; set; }

        /// <summary>
        /// Array of serialized Plants for Database
        /// </summary>
        public string[] StrPlantArray { get; set; }

        public int Score { get; set; }

        public List<Plant> Plants { get; set; }

        public class Plant
        {
            public float begin;
            public float end;

            /// <summary>
            /// initialzie plant with begin/end -1 (globaly not seeded)
            /// </summary>
            public Plant()
            {
                begin = -1;
                end = -1;
            }

            /// <summary>
            /// Check if plant is ready for seed
            /// </summary>
            /// <returns>True if can be seed</returns>
            public bool CanSeed()
            {
                if (begin > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            /// <summary>
            /// Check if plant can be seeded and seed with end time "Server Time + 18"
            /// </summary>
            /// <returns>If seeding done with success</returns>
            public bool Seed()
            {
                if (CanSeed())
                {
                    begin = GameSettings.GetTime();
                    end = begin + 18;
                    Debug.LogInfo("Plant seeded");
                    return true;
                }

                Debug.LogInfo("Plant cannot be seeded");
                return false;
            }

            /// <summary>
            /// Check if plant is ready for harvest
            /// </summary>
            /// <returns>True if ready to harvest</returns>
            public bool CanHarvest()
            {
                if (GameSettings.GetTime() > end)
                    return true;
                else
                    return false;

            }

            /// <summary>
            /// If can harvest, change time of plants.
            /// </summary>
            /// <returns>If harvesting was done with success</returns>
            public bool Harvest()
            {
                if (CanHarvest())
                {
                    begin = -1;
                    end = -1;
                    Debug.LogInfo("Plant harvested.");
                    return true;
                }
                else
                {
                    Debug.LogInfo("Plant not ready to harvest.");
                    return false;
                }

            }

            /// <summary>
            /// Time plant begin|end for Database purposes
            /// </summary>
            /// <returns>String of "begin , end" times</returns>
            public string GetTime()
            {
                return begin + "," + end;
            }
        }

        public Player(string login)
        {
            this.Login = login;
            this.Score = 0;
            this.StrPlantArray = new string[4];
            this.Plants = new List<Plant>
            {
                new Plant(),
                new Plant(),
                new Plant(),
                new Plant()
            };
        }
    }
}
