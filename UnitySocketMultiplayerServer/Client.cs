using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using static UnitySocketMultiplayerServer.Player;

namespace UnitySocketMultiplayerServer
{

    class Data
    {
        public string action;
        public Dictionary<string, string> data;
        public List<string> errors;

        public Data()
        {
            data = new Dictionary<string, string>();
            errors = new List<string>();
        }
    }

    class Client
    {
        Dictionary<string, Func<Data, Data, Data>> functionCaller;
        readonly TcpClient client;
        readonly NetworkStream stream;
        readonly StreamReader sr;
        readonly StreamWriter sw;
        private Player player;
        private Guid uid;
        private bool isConnected;

        public Client(TcpClient newClient, Guid guid)
        {
            InitParser();
            client = newClient;
            stream = client.GetStream();
            uid = guid;
            sr = new StreamReader(stream);
            sw = new StreamWriter(stream);
            isConnected = true;
            Debug.LogInfo($"{uid} Client Stream Tunel created");
        }

        public Guid GetUID()
        {
            return uid;
        }

        Data CMDPing(Data sendData, Data receivedData)
        {
            sendData.data.Add("ping", "pong");
            sendData.data.Add("clientTime", receivedData.data["clientTime"]);
            sendData.data.Add("serverTime", (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString());
            return sendData;
        }

        Data CMDLogIn(Data sendData, Data receivedData)
        {

            if (receivedData.data.ContainsKey("login"))
            {
                string login = receivedData.data["login"];
                if (login.Length > 3)
                {
                    LogIn(login);
                    sendData.data.Add("id", player.Id.ToString());
                }
                else
                    sendData.errors.Add("Login too short");
            }
            else
                sendData.errors.Add("Key login not found");

            return sendData;
        }

        private Data CMDInterract(Data sendData, Data receivedData)
        {
            int id = int.Parse(receivedData.data["id"]);

            if (player != null)
            {
                Plant plant = player.Plants[id];

                if (plant.CanHarvest())
                {
                    if (plant.Harvest())
                    {
                        sendData.data["id"] = id.ToString();
                        sendData.data["interract"] = "harvested";
                        player.Score += 1;
                    }
                    else
                    {
                        sendData.errors.Add("its no time to harvest");
                    }
                }
                else
                {

                    if (plant.Seed())
                    {
                        sendData.data["id"] = id.ToString();
                        sendData.data["interract"] = "seed";
                    }
                    else
                    {
                        sendData.errors.Add("its no time to seed");
                    }
                }
            }

            sendData.data["interract"] = "unable";

            return sendData;
        }

        private Data CMDStat(Data sendData, Data receivedData)
        {
            sendData.data.Add("time", GameSettings.GetTime().ToString());
            sendData.data.Add("plant1", player.Plants[0].GetTime());
            sendData.data.Add("plant2", player.Plants[1].GetTime());
            sendData.data.Add("plant3", player.Plants[2].GetTime());
            sendData.data.Add("plant4", player.Plants[3].GetTime());
            return sendData;
        }

        private Data CMDExit(Data sendData, Data receivedData)
        {
            sendData.data.Add("exit", "exit");
            onDisconnect();
            return sendData;
        }
        void InitParser()
        {
            functionCaller = new Dictionary<string, Func<Data, Data, Data>>
            {
                ["ping"] = CMDPing,
                ["login"] = CMDLogIn,
                ["stats"] = CMDStat,
                ["interract"] = CMDInterract,
                ["exit"] = CMDExit
            };
        }

        public void Update()
        {
            Debug.LogInfo("Thread Started");
            try
            {

                while (isConnected)
                {
                    string received = ReceiveData();

                    if (received == null)
                        break;

                    Data receivedData = JsonConvert.DeserializeObject<Data>(received);

                    string calledAction = receivedData.action;

                    Data sendData = new Data
                    {
                        action = calledAction
                    };

                    if (functionCaller.ContainsKey(calledAction))
                    {
                        sendData = functionCaller[calledAction].Invoke(sendData, receivedData);
                    }
                    else
                    {
                        Debug.LogError($"Unknown server action: {calledAction}");
                        sendData.errors.Add($"Unknown server action: {calledAction}");
                    }

                    string response = JsonConvert.SerializeObject(sendData);

                    SendData(response);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Connection error");
                Debug.LogError(e.Message);
                Debug.LogError(e.StackTrace);
                sw.Close();
                sr.Close();
                client.Close();
            }

            onDisconnect();

            Debug.LogInfo("Thread Ended");

        }

        void onDisconnect()
        {
            isConnected = false;
            if (player != null)
            {
                Database.PlayerSave(player);
            }
        }

        string ReceiveData()
        {
            try
            {
                string received = sr.ReadLine();
                Debug.LogData("SRV <-  Client:\t" + received);
                if (received == null)
                {
                    Debug.LogError("SRV <- Client: Send null message...");
                    return null;
                }
                Statistics.GetStat(uid).LogDownloaded(Encoding.ASCII.GetByteCount(received));
                return received;
            }
            catch
            {
                Debug.LogError("SRV <- Client: Connection forced to close during receiving...");
                onDisconnect();
                return null;
            }
     
        }

        void SendData(string json)
        {
            try
            {
                sw.WriteLine(json);
                sw.Flush();
                Debug.LogData("SRV -> Client:\t" + json);
                Statistics.GetStat(uid).LogUploaded(Encoding.ASCII.GetByteCount(json));
            }
            catch
            {
                Debug.LogError("SRV -> Client: Connection forced to close during sending...");
                onDisconnect();
            }
        }

        void LogIn(string login)
        {
            if (player != null)
            {
                Debug.LogError("Client is already logged in!");
                return;
            }

            int id = Database.PlayerGetID(login);

            if (id >= 0)
            {
                player = Database.PlayerGet(id);
            }
            else
            {
                player = Database.PlayerInit(login);
            }
        }
    }
}
