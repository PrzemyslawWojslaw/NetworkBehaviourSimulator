using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace TSST_Cloud_v2
{
    class Cloud
    {
        Form1 form;
        string path;
        private bool isOn = false;
        List<Cable> cables = new List<Cable>();
        TcpListener listener = new TcpListener(IPAddress.Any, 60000);
        Dictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();
        //List<TcpClient> clients = new List<TcpClient>();
        //List<string> clientNames = new List<string>();

        public Cloud(ref Form1 win, string p)
        {
            form = win;
            path = p;
        }

        public string GetTime()
        {
            DateTime now = DateTime.Now;
            //string temp = now.ToString();
            string temp = now.ToString("yyyy.MM.dd - HH:mm.ss.ffff");
            return temp + "  >>>  ";
        }

        public void Start()
        {
            form.SetLog(GetTime() + "Uruchomiono chmurę kablową.");
            isOn = true;

            string conf = System.IO.File.ReadAllText("cloud_config.txt");
            string[] temp = conf.Split(' ');
            foreach (string part in temp)
            {
                cables.Add(new Cable(part));
            }
            form.SetLog(GetTime() + "Ustalono nową konfigurację.");

            Thread thread1 = new Thread(new ThreadStart(WorkWithNms));
            thread1.Start();
            Thread thread2 = new Thread(new ThreadStart(ListenStart));
            thread2.Start();        
        }

        public void Stop()
        {
            isOn = false;
            form.SetLog(GetTime() + "Wyłączono chmurę kablową.");
        }

        public void WorkWithNms()
        {
            TcpClient clientNms = new TcpClient("127.0.0.1", 50001);

            NetworkStream stream = clientNms.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            BinaryReader reader = new BinaryReader(stream);

            string message = "Hello|cloud"; // + cablesToString(); (gdy wczytane z pliku)
            writer.Write(message);
            writer.Flush();

            while (isOn)
            {
                message = "-";
                if (stream.DataAvailable)
                {
                    message = reader.ReadString();
                    string[] temp = message.Split('|');

                    if (temp[0] == "Config")
                    {
                        if (cables.Count != 0)
                            cables.Clear();

                        message = message.Substring(7);
                        temp = message.Split(' ');
                        foreach (string part in temp)
                        {
                            cables.Add(new Cable(part));
                        }

                        form.SetLog(GetTime() + "Wprowadzono nową konfigurację.");
                    }
                    else
                    {
                        form.SetLog(GetTime() + "BŁĄD! Otrzymano od systemu zarządzania wiadomość nieznanego typu!");
                    }
                }
                else
                {
                    if (clientNms.Client.Poll(0, SelectMode.SelectRead))
                    {
                        byte[] buff = new byte[1];
                        if (clientNms.Client.Receive(buff, SocketFlags.Peek) == 0)
                        {
                            break;
                        }
                    }
                }
            }

            form.SetLog(GetTime() + "Odłączono system zarządzania!");
        }

        public void ListenStart()
        {
            listener.Start();
            form.SetLog(GetTime() + "Włączono nasłuchiwanie.");
            while (isOn)
            {
                if (listener.Pending())
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Thread clientThread = new Thread(new ParameterizedThreadStart(WorkWithClient));
                    form.SetLog(GetTime() + "Połączono z nowym urządzeniem.");
                    clientThread.Start(client);
                }
                Thread.Sleep(15);
            }

            listener.Stop();
            form.SetLog(GetTime() + "Wyłaczono nasłuchiwanie.");
        }

        public void WorkWithClient(object client)
        {
            TcpClient tcpClient = client as TcpClient;
            NetworkStream stream = tcpClient.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            BinaryWriter writer = new BinaryWriter(stream);
            string name = "";

            // Zabezpieczenie, prawdopodobnie nie potrzebne
            if (tcpClient == null)
            {
                form.SetLog(GetTime() + "Klient TCP ma wartośc null - coś jest bardzo źle.");
                tcpClient.Close();
                return;
            }

            string msg;
            while (isOn)  // w tej pętli komunikacja w obie strony
            {
                try
                {
                    msg = "-";
                    
                    if (stream.DataAvailable)
                    {
                        msg = reader.ReadString();
                        string[] message = msg.Split('|');

                        if (message[0] == "Hello")
                        {
                            name = message[1];
                            form.SetLog(GetTime() + "Urządzenie przedstawiło się jako \"" + name + "\".");
                            clients.Add(name, tcpClient);
                        }
                        else if (message[0] == "Message")  // budowa wiadomości: "Message"|port|pierwsza_szczelina|ostatnia|nadawca|adresat|treść
                        {
                            form.SetLog(GetTime() + "Urządzenie \"" + name + "\" przysłało wiadomość:");
                            form.SetLog(msg);

                            for (int i = 0; i < cables.Count; i++)
                            {
                                if (cables[i].Check(name, message[1]))
                                {
                                    string[] temp = cables[i].GetEnd(name, message[1]); // temp[0] - nazwa adresata, temp2[1] - port adresata
                                    message[1] = temp[1];
                                    form.SetLog(GetTime() + "Odnaleziono odpowiednie połączenie - wiadomość zostanie przesłana na port " + temp[1] + " urządzenia  \"" + temp[0] + "\".");

                                    msg = message[0];
                                    for (int j = 1; j < message.Length; j++)
                                        msg = msg + "|" + message[j];
                                    TcpClient tempClient;
                                    if (clients.TryGetValue(temp[0], out tempClient))
                                    {
                                        try
                                        {
                                            NetworkStream n = tempClient.GetStream();
                                            BinaryWriter w = new BinaryWriter(n);
                                            w.Write(msg);
                                            w.Flush();
                                            form.SetLog(GetTime() + "Przesłano urządzeniu \"" + temp[0] + "\" wiadomość:");
                                            form.SetLog(msg);
                                        }
                                        catch
                                        {
                                            form.SetLog(GetTime() + "Urządzenie \"" + temp[0] + "\" nie jest już podłączone do chmury.");
                                        }
                                    }
                                    else
                                    {
                                        form.SetLog(GetTime() + "BŁĄD! Urządzenie \"" + temp[0] + "\" nie jest podłączone do chmury!");
                                    }

                                    break;
                                }
                            }
                        }
                    }
                    else  // czy klient jeszcze żyje
                    {
                        if (tcpClient.Client.Poll(0, SelectMode.SelectRead))
                        {
                            byte[] buff = new byte[1];
                            if (tcpClient.Client.Receive(buff, SocketFlags.Peek) == 0)
                            {
                                break;
                            }
                        }
                    }
                }
                catch
                {
                    form.SetLog(GetTime() + "Urządzenie \"" + name + "\" rozłączyło się.");
                    break;
                }
                
            }

            Thread.Sleep(15);
            DisconnectClient(name, tcpClient);
        }

        private void DisconnectClient(string name, TcpClient client)
        {
            if (client == null)
            {
                return;
            }

            client.Close();
            clients.Remove(name);

            form.SetLog(GetTime() + "Rozłączono klienta \"" + name + "\".");
        }

        public string cablesToString()
        {
            string temp = "";
            foreach(Cable cable in cables)
            {
                temp = temp + cable.GetCableData() + " ";
            }
            temp = temp.Substring(0, temp.Length - 1); // usunięcie ostatniej spacji
            return temp;
        }
    }
}
