using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace UnitySocketMultiplayerServer
{
    class Client
    {
        TcpClient client;
        NetworkStream stream;
        StreamReader sr;
        StreamWriter sw;
        Guid uid;


        public Client(TcpClient newClient, Guid guid)
        {
            client = newClient;
            stream = client.GetStream();
            uid = guid;
            sr = new StreamReader(stream);
            sw = new StreamWriter(stream);
            Debug.LogInfo("Client Stream Tunel created");
        }

        public Guid GetUID()
        {
            return uid;
        }

        public void Update()
        {
            Debug.LogInfo("Thread Started");
            try
            {

                /*
                  byte[] buffer = new byte[1024];
                  stream.Read(buffer, 0, buffer.Length);
                  int recv = 0;
                  foreach (byte b in buffer)
                  {
                      if (b != 0)
                      {
                          recv++;
                      }
                  }
                  string request = Encoding.UTF8.GetString(buffer, 0, recv);*/
                while (true)
                {
                    string received = sr.ReadLine();
                    Statistics.GetStat(uid).LogDownloaded(Encoding.ASCII.GetByteCount(received));
                    //Debug.LogData("SRV «  Client: " + received);

                    string response = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.Nam ac suscipit orci, non pharetra elit. Maecenas eu auctor augue. Integer rutrum euismod enim efficitur pharetra. Sed eu ex vulputate, dignissim eros eu, gravida risus.Donec iaculis libero quis nunc fringilla lobortis.Curabitur tincidunt sit amet leo id malesuada.Donec porta massa vel massa facilisis, sit amet varius eros lobortis. hasellus mattis eget turpis id vestibulum. Integer iaculis sollicitudin pellentesque. Proin velit nulla, scelerisque sit amet malesuada ac, rutrum sit amet nibh. Maecenas ac sollicitudin metus, nec scelerisque quam. Aliquam nec vehicula quam. Nullam pretium malesuada magna, pulvinar dapibus libero dictum nec. Nulla fringilla neque ut posuere commodo. Praesent vel blandit risus, non vulputate ipsum. Nullam dui risus, facilisis ac molestie ut, luctus nec libero. Fusce eget luctus purus. Morbi eget rhoncus elit. Morbi nec elit malesuada, lobortis dolor ut, hendrerit lacus.Donec sit amet congue mi.Morbi in lobortis dolor, ut auctor leo. Donec tristique dolor sed porttitor malesuada. Morbi ante felis, ultricies non auctor a, venenatis sit amet nunc.Mauris efficitur felis sed ipsum ultricies convallis.Proin consectetur justo ac lorem vehicula porta.Morbi vel augue condimentum, varius odio quis, euismod dui.Pellentesque consectetur magna sit amet fermentum gravida.Praesent ac interdum lacus. Ut ac egestas ante. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas.Curabitur non elementum diam. Suspendisse maximus turpis a erat luctus congue.Vestibulum lobortis dictum risus eu malesuada. Pellentesque feugiat erat vel sapien accumsan fringilla.Aenean non dictum elit. Ut non ultricies neque. Nunc sapien odio, blandit quis convallis eget, sodales vel mi. In in erat magna. Duis non feugiat sapien. In tincidunt turpis augue, non dignissim nisi pretium at. Mauris sed tristique mauris. Donec pulvinar, sem sit amet lacinia commodo, dui felis sodales sem, eu consequat arcu velit vel metus. Praesent lobortis, leo eget cursus tincidunt, felis magna commodo felis, quis varius neque sapien vitae sem.Cras suscipit dolor ex, at viverra magna finibus eu. Morbi volutpat auctor metus sed pharetra. Nullam auctor, erat elementum laoreet tincidunt, elit ante ornare mi, quis mattis dolor quam cursus ligula.Suspendisse ullamcorper venenatis mi vitae efficitur. Proin lorem odio, pretium eu nunc quis, cursus elementum augue. Phasellus lobortis eget leo nec molestie. Suspendisse ante nisl, ultrices a erat ac, congue lobortis orci. Aenean volutpat dignissim libero. Duis ultrices lacus ut neque bibendum porta.Pellentesque lobortis tincidunt elit, et laoreet arcu placerat eget. Maecenas feugiat, nibh tempor interdum dignissim, massa augue molestie justo, non interdum mauris purus at metus.Interdum et malesuada fames ac ante ipsum primis in faucibus.Nam erat dui, mattis id viverra id, consequat eget nisi. Quisque lacinia augue sit amet massa ultricies malesuada. Donec dignissim eu lorem at feugiat. Mauris fermentum nibh lacus. Vivamus accumsan ipsum et maximus viverra.";
                    sw.WriteLine(response);
                    sw.Flush();
                    //Debug.LogData("SRV » Client: " + response);
                    Statistics.GetStat(uid).LogUploaded(Encoding.ASCII.GetByteCount(response));
                }

            }
            catch (Exception e)
            {
                Debug.LogError("Something went wrong.");
                sw.WriteLine(e.ToString());
            }

            Debug.LogInfo("Thread Ended");

            ClientController.FreeClient(this);
        }
    }
}
