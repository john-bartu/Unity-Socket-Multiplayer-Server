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

        public string[] StrPlantArray { get; set; }

        public int Score { get; set; }

        public List<Plant> Plants { get; set; }

        public class Plant
        {
            public float begin;
            public float end;

            public Plant()
            {
                begin = -1;
                end = -1;
            }


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

            public bool CanHarvest()
            {
                if (GameSettings.GetTime() > end)
                    return true;
                else
                    return false;

            }

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
