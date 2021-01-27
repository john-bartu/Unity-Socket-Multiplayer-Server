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

        public static int playerGetID(string findLogin)
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

        public static void playerSave(Player player)
        {

            var users = db.GetCollection<Player>("players");
            var result = users.FindOne(x => x.Login == player.Login);

            if (result != null)
            {

                string[] strPlants = new string[4];
                strPlants[0] = (JsonConvert.SerializeObject(player.plants[0]));
                strPlants[1] = (JsonConvert.SerializeObject(player.plants[1]));
                strPlants[2] = (JsonConvert.SerializeObject(player.plants[2]));
                strPlants[3] = (JsonConvert.SerializeObject(player.plants[3]));

                player.strPlantArray = strPlants;
                Debug.LogDB($"Player {player.Id} updated");
                users.Update(player);
            }
            else
            {
                string[] strPlants = new string[4];
                strPlants[0] = (JsonConvert.SerializeObject(player.plants[0]));
                strPlants[1] = (JsonConvert.SerializeObject(player.plants[1]));
                strPlants[2] = (JsonConvert.SerializeObject(player.plants[2]));
                strPlants[3] = (JsonConvert.SerializeObject(player.plants[3]));

                player.strPlantArray = strPlants;
                Debug.LogDB($"Player {player.Id} row created");
                users.Insert(player);
            }
        }

        public static Player playerGet(int id)
        {
            var users = db.GetCollection<Player>("players");
            var result = users.FindOne(x => x.Id == id);

            if (result != null)
            {
                Debug.LogDB($"Player {id} loaded");


                string[] strPlants = (result.strPlantArray);

                List<Player.Plant> plants = new List<Player.Plant>();
                plants.Add(JsonConvert.DeserializeObject<Player.Plant>(strPlants[0]));
                plants.Add(JsonConvert.DeserializeObject<Player.Plant>(strPlants[1]));
                plants.Add(JsonConvert.DeserializeObject<Player.Plant>(strPlants[2]));
                plants.Add(JsonConvert.DeserializeObject<Player.Plant>(strPlants[3]));

                result.plants = plants;
                return result;
            }
            else
            {
                Debug.LogDB($"Player {id} not exists");
                return null;
            }

        }

        public static Player playerInit(string login)
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
