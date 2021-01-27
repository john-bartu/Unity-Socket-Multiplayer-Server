using System;
using System.Collections.Generic;
using System.Text;
using LiteDB;
using Newtonsoft.Json;

namespace UnitySocketMultiplayerServer
{
    static class Database
    {

        static LiteDatabase db;

        public static void Init()
        {
            db = new LiteDatabase(@"database.db");
        }

        public static int PlayerGetID(string findLogin)
        {
            var users = db.GetCollection<Player>("players");
            var result = users.FindOne(x => x.Login == findLogin);

            if (result != null)
            {
                Debug.LogDB($"Player with name: {findLogin} does exists In Database");
                return result.Id;
            }
            else
            {
                Debug.LogDB($"Player with name: {findLogin} does NOT exists In Database");
                return -1;
            }

        }

        public static void PlayerSave(Player player)
        {

            var users = db.GetCollection<Player>("players");
            var result = users.FindOne(x => x.Login == player.Login);

            if (result != null)
            {

                string[] strPlants = new string[4];
                strPlants[0] = (JsonConvert.SerializeObject(player.Plants[0]));
                strPlants[1] = (JsonConvert.SerializeObject(player.Plants[1]));
                strPlants[2] = (JsonConvert.SerializeObject(player.Plants[2]));
                strPlants[3] = (JsonConvert.SerializeObject(player.Plants[3]));

                player.StrPlantArray = strPlants;
                Debug.LogDB($"Player {player.Id} updated");
                users.Update(player);
            }
            else
            {
                string[] strPlants = new string[4];
                strPlants[0] = (JsonConvert.SerializeObject(player.Plants[0]));
                strPlants[1] = (JsonConvert.SerializeObject(player.Plants[1]));
                strPlants[2] = (JsonConvert.SerializeObject(player.Plants[2]));
                strPlants[3] = (JsonConvert.SerializeObject(player.Plants[3]));

                player.StrPlantArray = strPlants;
                Debug.LogDB($"Player {player.Id} row created");
                users.Insert(player);
            }
        }

        public static Player PlayerGet(int id)
        {
            var users = db.GetCollection<Player>("players");
            var result = users.FindOne(x => x.Id == id);

            if (result != null)
            {
                Debug.LogDB($"Player {id} loaded");


                string[] strPlants = (result.StrPlantArray);

                List<Player.Plant> plants = new List<Player.Plant>
                {
                    JsonConvert.DeserializeObject<Player.Plant>(strPlants[0]),
                    JsonConvert.DeserializeObject<Player.Plant>(strPlants[1]),
                    JsonConvert.DeserializeObject<Player.Plant>(strPlants[2]),
                    JsonConvert.DeserializeObject<Player.Plant>(strPlants[3])
                };

                result.Plants = plants;
                return result;
            }
            else
            {
                Debug.LogDB($"Player {id} not exists");
                return null;
            }

        }

        public static Player PlayerInit(string login)
        {
            var users = db.GetCollection<Player>("players");
            int id = users.Insert(new Player(login));
            var result = users.FindOne(x => x.Id == id);

            if (result != null)
            {
                Debug.LogDB($"Player {login} row initialized");
                return result;
            }
            else
            {
                Debug.LogDB($"Player {login} can't init");
                return null;
            }

        }
    }
}
