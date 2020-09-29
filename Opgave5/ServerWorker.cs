using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Opgave1;

namespace Opgave5
{
    public class ServerWorker
    {
        public void Start()
        {
            //Ip er egen computer, port er ekke server
            TcpListener server = new TcpListener(IPAddress.Loopback, 4646);
            server.Start();
            Console.WriteLine("Server Open");

            bool keepOpen = true;

            while (keepOpen)
            {
                TcpClient socket = server.AcceptTcpClient();
                Task.Run(() => keepOpen = DoClient(socket));
            }
        }

        private bool DoClient(TcpClient socket)
        {
            NetworkStream nr = socket.GetStream();
            StreamReader sr = new StreamReader(nr);
            StreamWriter sw = new StreamWriter(nr);

            bool keepOpen = true;

            while (true)
            {
                string message = sr.ReadLine();
                string body = sr.ReadLine();
                if (message == null)
                    break;

                message = message.ToLower();
                message = message.Replace(" ", "");
                if (message.StartsWith("close"))
                {
                    keepOpen = false;
                    break;
                }

                string response = "";

                if (message == "hentalle")
                {
                    response = JsonConvert.SerializeObject(BikeLibrary.Get());
                }
                else if (message == "hent")
                {
                    if (Int32.TryParse(body, out int id))
                    {
                        Bike bike = BikeLibrary.Get(id);
                        if (bike == null)
                            response = $"Ingen cykel har id'et {id}";
                        else
                            response = JsonConvert.SerializeObject(bike);
                    }
                    else
                        response = "Id'et er ikke et tal";
                }
                else if (message == "gem")
                {
                    Bike bike = JsonConvert.DeserializeObject<Bike>(body);
                    if (bike == null)
                        response = $"Forkert format -> {body}";
                    else
                    {
                        int result = BikeLibrary.Post(bike);
                        if (result > 0)
                            response = $"Gemte Cyklen med Id'et {result}";
                        else
                            response = "Kunne ikke gemme cyklen";
                    }
                }

                Console.WriteLine($"Server input: {message}");
                Console.WriteLine($"Server output: {response}");

                sw.WriteLine(response);
                sw.Flush();
            }

            socket.Close();
            return keepOpen;
        }
    }
}
