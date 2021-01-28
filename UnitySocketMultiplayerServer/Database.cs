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

        /// <summary>
        /// Create/Load Database file
        /// </summary>
        public static void Init()
        {
            db = new LiteDatabase(@"database.db");
        }

        /// <summary>
        /// Get Player ID with given name
        /// </summary>
        /// <param name="findLogin">Player Login</param>
        /// <returns>Player ID, -1 if not exists in database</returns>
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

        /// <summary>
        /// Save Player object to database
        /// </summary>
        /// <param name="player">Player object to save</param>
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

        /// <summary>
        /// Load player from Database
        /// </summary>
        /// <param name="id">ID row, where player is</param>
        /// <returns>Player from Database, null if not exists</returns>
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

        /// <summary>
        /// Init Player Object from database
        /// </summary>
        /// <param name="login">Provide player login</param>
        /// <returns>Initialized Player Object with values from database</returns>
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
