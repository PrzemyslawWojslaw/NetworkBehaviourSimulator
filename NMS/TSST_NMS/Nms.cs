using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;

namespace TSST_NMS
{
    class Nms
    {
        bool isOn = false;
        string domainName;
        string repair;

        TcpListener listener;
        Dictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();
        Dictionary<string, List<string>> toSend = new Dictionary<string, List<string>>();

        Netmap nmap;

        public Nms(ref Form1 win, string n)
        {
            if (n[0] != 's' && n[0] != 'd')
                domainName = "d" + n;
            else
                domainName = n;
            int tempI;
            if (domainName == "s1")
                tempI = 2;
            else if (domainName == "d2")
                tempI = 3;
            else
                tempI = Int32.Parse(domainName.Substring(1));
            nmap = new Netmap(ref win, domainName);
            repair = "";
            listener = new TcpListener(IPAddress.Any, 50000 + tempI);
            nmap.Form.SetLabel(domainName);



            //Tunnel tun = nmap.GetPathPorts("c1", "c2", "25 Gbit/s", "");
            //Tunnel tun2 = nmap.GetPathPorts("c1", "c2", "25 Gbit/s", "");
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
            isOn = true;
            nmap.Form.SetLog(GetTime() + "Uruchomiono węzeł sterujący domeny \"" + domainName + "\".");
            Thread thread = new Thread(new ThreadStart(ListenStart));
            thread.Start();
            if (domainName != "d1")
            {
                Thread thread2 = new Thread(new ThreadStart(WorkWithDomains));
                thread2.Start();
            }
        }

        public void Stop()
        {
            isOn = false;
            //nmap.Form.SetLog("Wyłączono NMS");  // okno (Form) już nie istnieje w tym momencie, patrz Form1.Dispose() (w "Designer.cs")
        }

        public void ListenStart()
        {
            listener.Start();
            nmap.Form.SetLog(GetTime() + "Włączono nasłuchiwanie.");

            while (isOn)
            {
                if (listener.Pending())
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Thread clientThread = new Thread(new ParameterizedThreadStart(WorkWithClient));
                    nmap.Form.SetLog(GetTime() + "Połączono z nowym urządzeniem.");
                    clientThread.Start(client);
                }
                //Thread.Sleep(15);

                if (repair != "" && repair[0] == 'r' && (repair[1] == '5' || repair[1] == '8'))
                {
                    string[] repairTemp = repair.Split('|');

                    nmap.Form.SetOfficial(GetTime() + "LRM -> CC : Wykryto niesprawność węzła \"" + repairTemp[0] + "\"!");
                    nmap.Form.SetOfficial(GetTime() + "CC -> RC : Żądanie znalezienia drogi zastępczej dla węzła \"" + repairTemp[0] + "\".");
                    
                    if (repairTemp[0] == "r2")
                    {
                        Tunnel tempTunnel = nmap.GetTunnel("r2");
                        if(tempTunnel.Nodes.Count == 0)
                        {
                            //nic nie rób
                        }
                        else
                        {
                            foreach(KeyValuePair<string, string> entry in tempTunnel.Nodes)
                            {
                                toSend[entry.Key].Add("Remove|" + entry.Value);
                            }
                        }
                    }
                    else if (repairTemp[0] == "r5")
                    {
                        /*toSend["r4"].Add("Remove|1|" + repairTemp[1] + "|" + repairTemp[2] + "|3");
                        nmap.RemoveFib("r4", "1", "3", repairTemp[1], repairTemp[2]);
                        toSend["r6"].Add("Remove|1|" + repairTemp[1] + "|" + repairTemp[2] + "|3");
                        nmap.RemoveFib("r6", "1", "3", repairTemp[1], repairTemp[2]);
                        toSend["r4"].Add("Add|1|" + repairTemp[1] + "|" + repairTemp[2] + "|2");
                        nmap.AddFib("r4", "1", "2", repairTemp[1], repairTemp[2]);
                        toSend["r6"].Add("Add|2|" + repairTemp[1] + "|" + repairTemp[2] + "|3");
                        nmap.AddFib("r6", "2", "3", repairTemp[1], repairTemp[2]);*/

                        Tunnel tempTunnel = new Tunnel("-", "-", "0", "0");
                        do
                        {
                            tempTunnel = nmap.GetTunnel("r5");
                            if (tempTunnel.Start != "-")
                            {
                                for(int i = 0; i < tempTunnel.Nodes.Count; i++)
                                {
                                    if (tempTunnel.Nodes.ElementAt(i).Key == "r5" && tempTunnel.Nodes.ElementAt(i+1).Key == "r4" && tempTunnel.Nodes.ElementAt(i-1).Key == "r6")
                                    {
                                        toSend["r4"].Add("Remove|1|" + tempTunnel.First + "|" + tempTunnel.Last + "|3");
                                        nmap.RemoveFib("r4", "1", "3", tempTunnel.First, tempTunnel.Last);
                                        toSend["r6"].Add("Remove|1|" + tempTunnel.First + "|" + tempTunnel.Last + "|3");
                                        nmap.RemoveFib("r6", "1", "3", tempTunnel.First, tempTunnel.Last);
                                        toSend["r4"].Add("Add|1|" + tempTunnel.First + "|" + tempTunnel.Last + "|2");
                                        nmap.AddFib("r4", "1", "2", tempTunnel.First, tempTunnel.Last);
                                        toSend["r6"].Add("Add|2|" + tempTunnel.First + "|" + tempTunnel.Last + "|3");
                                        nmap.AddFib("r6", "2", "3", tempTunnel.First, tempTunnel.Last);

                                        nmap.ChangeTunnelConf(tempTunnel.Start, tempTunnel.End, tempTunnel.First, tempTunnel.Last, "r4", "1|" + tempTunnel.First + "|" + tempTunnel.Last + "|2");
                                        nmap.ChangeTunnelConf(tempTunnel.Start, tempTunnel.End, tempTunnel.First, tempTunnel.Last, "r6", "2|" + tempTunnel.First + "|" + tempTunnel.Last + "|3");
                                        nmap.ChangeTunnelConf(tempTunnel.Start, tempTunnel.End, tempTunnel.First, tempTunnel.Last, "r5", "Remove");
                                    }
                                    else if(tempTunnel.Nodes.ElementAt(i).Key == "r5" && tempTunnel.Nodes.ElementAt(i - 1).Key == "r4" && tempTunnel.Nodes.ElementAt(i + 1).Key == "r6")
                                    {
                                        toSend["r4"].Add("Remove|3|" + tempTunnel.First + "|" + tempTunnel.Last + "|1");
                                        nmap.RemoveFib("r4", "3", "1", tempTunnel.First, tempTunnel.Last);
                                        toSend["r6"].Add("Remove|3|" + tempTunnel.First + "|" + tempTunnel.Last + "|1");
                                        nmap.RemoveFib("r6", "3", "1", tempTunnel.First, tempTunnel.Last);
                                        toSend["r4"].Add("Add|2|" + tempTunnel.First + "|" + tempTunnel.Last + "|1");
                                        nmap.AddFib("r4", "2", "1", tempTunnel.First, tempTunnel.Last);
                                        toSend["r6"].Add("Add|3|" + tempTunnel.First + "|" + tempTunnel.Last + "|2");
                                        nmap.AddFib("r6", "3", "2", tempTunnel.First, tempTunnel.Last);

                                        nmap.ChangeTunnelConf(tempTunnel.Start, tempTunnel.End, tempTunnel.First, tempTunnel.Last, "r4", "2|" + tempTunnel.First + "|" + tempTunnel.Last + "|1");
                                        nmap.ChangeTunnelConf(tempTunnel.Start, tempTunnel.End, tempTunnel.First, tempTunnel.Last, "r6", "3|" + tempTunnel.First + "|" + tempTunnel.Last + "|2");
                                        nmap.ChangeTunnelConf(tempTunnel.Start, tempTunnel.End, tempTunnel.First, tempTunnel.Last, "r5", "Remove");
                                    }
                                }
                            }
                        }
                        while (tempTunnel.Start != "-");

                    }
                    else if (repairTemp[0] == "r8")
                    {
                        /*toSend["r7"].Add("Remove|1|" + repairTemp[1] + "|" + repairTemp[2] + "|3");
                        nmap.RemoveFib("r7", "1", "3", repairTemp[1], repairTemp[1]);
                        toSend["r9"].Add("Remove|2|" + repairTemp[1] + "|" + repairTemp[2] + "|3");
                        nmap.RemoveFib("r9", "2", "3", repairTemp[1], repairTemp[2]);
                        toSend["r7"].Add("Add|1|" + repairTemp[1] + "|" + repairTemp[2] + "|2");
                        nmap.AddFib("r7", "1", "2", repairTemp[1], repairTemp[2]);
                        toSend["r9"].Add("Add|1|" + repairTemp[1] + "|" + repairTemp[2] + "|3");
                        nmap.AddFib("r9", "1", "3", repairTemp[1], repairTemp[2]);*/


                        Tunnel tempTunnel = new Tunnel("-", "-", "0", "0");
                        do
                        {
                            tempTunnel = nmap.GetTunnel("r8");
                            if (tempTunnel.Start != "-")
                            {
                                for (int i = 0; i < tempTunnel.Nodes.Count; i++)
                                {
                                    if (tempTunnel.Nodes.ElementAt(i).Key == "r8" && tempTunnel.Nodes.ElementAt(i + 1).Key == "r7" && tempTunnel.Nodes.ElementAt(i - 1).Key == "r9")
                                    {
                                        toSend["r7"].Add("Remove|1|" + tempTunnel.First + "|" + tempTunnel.Last + "|3");
                                        nmap.RemoveFib("r7", "1", "3", tempTunnel.First, tempTunnel.Last);
                                        toSend["r9"].Add("Remove|2|" + tempTunnel.First + "|" + tempTunnel.Last + "|3");
                                        nmap.RemoveFib("r9", "2", "3", tempTunnel.First, tempTunnel.Last);
                                        toSend["r7"].Add("Add|1|" + tempTunnel.First + "|" + tempTunnel.Last + "|2");
                                        nmap.AddFib("r7", "1", "2", tempTunnel.First, tempTunnel.Last);
                                        toSend["r9"].Add("Add|1|" + tempTunnel.First + "|" + tempTunnel.Last + "|3");
                                        nmap.AddFib("r9", "1", "3", tempTunnel.First, tempTunnel.Last);

                                        nmap.ChangeTunnelConf(tempTunnel.Start, tempTunnel.End, tempTunnel.First, tempTunnel.Last, "r7", "1|" + tempTunnel.First + "|" + tempTunnel.Last + "|2");
                                        nmap.ChangeTunnelConf(tempTunnel.Start, tempTunnel.End, tempTunnel.First, tempTunnel.Last, "r9", "1|" + tempTunnel.First + "|" + tempTunnel.Last + "|3");
                                        nmap.ChangeTunnelConf(tempTunnel.Start, tempTunnel.End, tempTunnel.First, tempTunnel.Last, "r8", "Remove");
                                    }
                                    else if (tempTunnel.Nodes.ElementAt(i).Key == "r8" && tempTunnel.Nodes.ElementAt(i - 1).Key == "r7" && tempTunnel.Nodes.ElementAt(i + 1).Key == "r9")
                                    {
                                        toSend["r7"].Add("Remove|3|" + tempTunnel.First + "|" + tempTunnel.Last + "|1");
                                        nmap.RemoveFib("r7", "3", "1", tempTunnel.First, tempTunnel.Last);
                                        toSend["r9"].Add("Remove|3|" + tempTunnel.First + "|" + tempTunnel.Last + "|2");
                                        nmap.RemoveFib("r9", "3", "2", tempTunnel.First, tempTunnel.Last);
                                        toSend["r7"].Add("Add|2|" + tempTunnel.First + "|" + tempTunnel.Last + "|1");
                                        nmap.AddFib("r7", "2", "1", tempTunnel.First, tempTunnel.Last);
                                        toSend["r9"].Add("Add|3|" + tempTunnel.First + "|" + tempTunnel.Last + "|1");
                                        nmap.AddFib("r9", "3", "1", tempTunnel.First, tempTunnel.Last);

                                        nmap.ChangeTunnelConf(tempTunnel.Start, tempTunnel.End, tempTunnel.First, tempTunnel.Last, "r7", "2|" + tempTunnel.First + "|" + tempTunnel.Last + "|1");
                                        nmap.ChangeTunnelConf(tempTunnel.Start, tempTunnel.End, tempTunnel.First, tempTunnel.Last, "r9", "3|" + tempTunnel.First + "|" + tempTunnel.Last + "|1");
                                        nmap.ChangeTunnelConf(tempTunnel.Start, tempTunnel.End, tempTunnel.First, tempTunnel.Last, "r8", "Remove");
                                    }
                                }
                            }
                        }
                        while (tempTunnel.Start != "-");

                    }

                    nmap.Form.SetOfficial(GetTime() + "RC -> CC : Odnaleziono nową ścieżkę:");

                    nmap.Form.SetOfficial(GetTime() + "CC -> LRM : Żądanie alokacji zasobów na potrzeby przejścia połączenia na nową ścieżkę.");
                    nmap.Form.SetOfficial(GetTime() + "LRM -> RC : Uaktualnienie informacji o stanie podsieci.");

                    repair = "";
                }
            }

            listener.Stop();
            //nmap.Form.SetLog(GetTime() + "Wyłaczono nasłuchiwanie.");  // okno (Form) już nie istnieje w tym momencie, patrz Form1.Dispose() (w "Designer.cs")
        }

        public void WorkWithClient(object client)
        {
            TcpClient tcpClient = client as TcpClient;
            NetworkStream stream = tcpClient.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            BinaryWriter writer = new BinaryWriter(stream);
            string name = "-";

            // Zabezpieczenie, prawdopodobnie nie potrzebne
            if (tcpClient == null)
            {
                nmap.Form.SetLog(GetTime() + "Klient TCP ma wartośc null - coś jest bardzo źle.");
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
                        nmap.Form.SetLog(GetTime() + "Od urządenia \"" + name + "\" otrzymano wiadomość:");
                        nmap.Form.SetLog("\t" + msg);
                        string[] message = msg.Split('|');

                        if(name[0] == 'd' || name[0] == 's')
                        {
                            string answer = HandleNmsMsg(msg);

                            if (answer != "-")
                            {
                                writer.Write(answer);
                                writer.Flush();
                                nmap.Form.SetLog(GetTime() + "Urządzeniu \"" + name + "\" przesłano wiadomość:");
                                nmap.Form.SetLog("\t" + answer);
                            }
                        }
                        else if (message[0] == "Hello")
                        {
                            name = message[1];
                            nmap.Form.SetLog(GetTime() + "Urządzenie przedstawiło się jako: \"" + name + "\".");
                            clients.Add(name, tcpClient);
                            toSend.Add(name, new List<string>());

                            /*
                            if(configurated)
                            {
                                string temp = nmap.CreateConfig(name);
                                if (temp != "-")
                                {
                                    writer.Write("Config|" + temp);
                                    writer.Flush();
                                    nmap.Form.SetLog(GetTime() + "Urządzeniu \"" + name + "\" przesłano konfigurację:");
                                    nmap.Form.SetLog(temp);
                                }
                                else
                                {
                                    nmap.Form.SetLog(GetTime() + "BŁĄD! Nie znaleziono konfiguracji dla urządzenia \"" + name + "\"!");
                                }
                            }*/
                        }
                        else if (message[0] == "CallRequest") // "CallRequest"|client-target|przepustowość|symbole/sek
                        {
                            string decoded1 = "";
                            string decoded2 = "";

                            nmap.Form.SetOfficial(GetTime() + "NCC : CallRequest() - Otrzymano żądanie zestawienia połączenia od użytkownika \"" + name + "\". Parametry:");
                            nmap.Form.SetOfficial("\tAdresat = \"" + message[1] + "\", Przepustowość = " + message[2]);
                            nmap.Form.SetOfficial("\tPrzepustowość symbolowa podnośnej = " + message[3]);
                            nmap.Form.SetOfficial(GetTime() + "NCC -> P : Policy() - Żądanie uwierzytelnienia i autoryzacji użytkownika \"" + name + "\".");
                            nmap.Form.SetOfficial(GetTime() + "P -> NCC : Tożsamość i uprawnienia użytkownika \"" + name + "\" potwierdzone.");
                            nmap.Form.SetOfficial(GetTime() + "NCC -> D : Translacja nazw użytkowników na adresy.");
                            if (name == "c1")
                            {
                                decoded1 = "1101";
                                decoded2 = "2101";
                                nmap.Form.SetOfficial(GetTime() + "D -> NCC : Adres nadawcy \"c1\" = 1101, adres odbiorcy \"c2\" = 2101");
                            }
                            else if (name == "c2")
                            {
                                decoded1 = "2101";
                                decoded2 = "1101";
                                nmap.Form.SetOfficial(GetTime() + "D -> NCC : Adres nadawcy \"c2\" = 2101, adres odbiorcy \"c1\" = 1101");
                            }
                            nmap.Form.SetOfficial(GetTime() + "NCC -> CC : ConnectionRequest() - Żądanie zestawienia połączenia. Parametry:");
                            nmap.Form.SetOfficial("\tUżytkownik początkowy (nadawca) = \"" + decoded1 + "\", Użytkownik końcowy = \"" + decoded2);
                            nmap.Form.SetOfficial("\tPrzepustowość = " + message[2] + ", Przepustowość symbolowa podnośnej = " + message[3]);

                            nmap.Form.SetOfficial(GetTime() + "CC -> RC : RouteTableQuery() - Żądanie znalezienia ścieżki. Parametry:");
                            nmap.Form.SetOfficial("\tod użytkownika =" + decoded1 + ", do użytkownika =" + decoded2);
                            nmap.Form.SetOfficial("\tprzepustowość =" + message[2] + " , ilość podnośnych = 1, przepływność symbolowa =" + message[3] + ", kodowanie QAM");

                            string trueTarget = nmap.CheckIfExDomain(message[1]);
                            if (trueTarget != message[1])
                            {
                                nmap.Form.SetOfficial(GetTime() + "RC -> NCC : Wykryto, że użytkownik docelowy należy do innej domeny (\"" + trueTarget + "\").");
                                if(domainName == "d1")
                                    toSend[trueTarget].Add("CallCoordination|" + domainName + "|1|" + trueTarget + "|1|" + message[1] + "|" + message[2]);
                                else
                                    toSend[trueTarget].Add("CallCoordination|" + domainName + "|1|" + trueTarget + "|2|" + message[1] + "|" + message[2]);

                                nmap.Form.SetOfficial(GetTime()+ "NCC : CallCoordination() - wysłano żądanie koordynacji do domeny \"" + trueTarget + "\" z parametrami:");
                                nmap.Form.SetOfficial("\tUżytkownik docelowy = \"" + message[1] + "\", Przepustowość = " + message[2]);
                                nmap.Form.SetOfficial("\tID routera brzegowego nadawcy (tej domeny) = " + "1" + ", ID routera brzegowego adresata = " + "1");
                                nmap.Form.SetOfficial(GetTime() + "NCC : Zakończono koordynację połączenia. Otrzymano informacje o dostępnych w domenie \"" + trueTarget + "\" szczelinach.");
                            }

                            Dictionary<string, string> tempDict = new Dictionary<string, string>(nmap.GetPathPorts(name, message[1], message[2], message[3]).Nodes);

                            string tempS = "";
                            string route = "";
                            for (int i = 0; i < tempDict.Count; i++)
                            {
                                tempS += "\t" + tempDict.ElementAt(i).Key + "===" + tempDict.ElementAt(i).Value + Environment.NewLine;
                                route = tempDict.ElementAt(i).Key + " -> " + route;
                            }
                            route = route.Substring(0, route.Length - 4);
                                
                            nmap.Form.SetLog(GetTime() + "Ścieżka i komendy konfiguracji:\n" + tempS);

                            if (tempDict.Count > 1)
                            {
                                nmap.Form.SetOfficial(GetTime() + "RC -> CC : Odnaleziono ścieżkę:");
                                nmap.Form.SetOfficial("\t" + route);
                                nmap.Form.SetOfficial(GetTime() + "CC : Podsieci \"s1\" wysłano żądanie zestawienia połączenia między interjesami 1 i 2.");
                                nmap.Form.SetOfficial(GetTime() + "CC -> LRM : Żądanie alokacji zasobów na potrzeby połącznia od użytkownika \"" + name + "\" do użytkownika \"" + message[1] + "\" ścieżką otrzymaną od RC.");

                                for (int i = 1; i < tempDict.Count - 1; i++)
                                {
                                    toSend[tempDict.ElementAt(i).Key].Add("Add|" + tempDict.ElementAt(i).Value);
                                }
                                writer.Write("Add|" + tempDict[name]);
                                writer.Flush();
                                string[] veryTemp = tempDict.ElementAt(tempDict.Count - 1).Value.Split('|');
                                nmap.ChangeTunnelConf(name, message[1], veryTemp[0], veryTemp[1], name, tempDict[name]);
                                nmap.Form.SetLog(GetTime() + "Urządzeniu \"" + name + "\" przesłano wiadomośc:");
                                nmap.Form.SetLog("\t" + "Add|" + tempDict[name]);

                                nmap.Form.SetOfficial(GetTime() + "LRM -> RC : Uaktualnienie informacji o stanie podsieci.");
                                // ma to już miejsce w funkcji GetPathPorts() (patrz AllocateChannels() i końcowe foreach)
                            }
                            else   // czyli jest tylko potencjalna wiadomość dla kienta lub nie ma nic - brak routerów po drodze
                            {
                                nmap.Form.SetOfficial(GetTime() + "RC -> CC : NIE odnaleziono ścieżki.");
                                writer.Write("Nope|" + message[1] + "|" + message[2]);
                                writer.Flush();
                            }
                        }
                        else if (message[0] == "CallTeardown")  // "CallTeardown"|target|first|last
                        {
                            nmap.Form.SetOfficial(GetTime() + "NCC : CallTeardown() - Otrzymano żądanie rozłączenia połączenia od użytkownika \"" + name + "\". Parametry:");
                            nmap.Form.SetOfficial("\tUżytkownik początkowy = \"" + name + "\", połączony z = " + message[1]);

                            string decoded1 = "";
                            string decoded2 = "";
                            if (name == "c1")
                            {
                                decoded1 = "1101";
                                decoded2 = "2101";
                            }
                            else if (name == "c2")
                            {
                                decoded1 = "2101";
                                decoded2 = "1101";
                            }
                            nmap.Form.SetOfficial(GetTime() + "NCC -> CC : ConnectionTeardown() - Żądanie zerwania połączenia. Parametry:");
                            nmap.Form.SetOfficial("\tUżytkownik początkowy = \"" + decoded1 + "\", użytkownik końcowy = " + decoded2 + ", szczeliny = " + message[2] + "-" + message[3]);
                            nmap.Form.SetOfficial(GetTime() + "CC : Podsieci \"s1\" wysłano żądanie zerwania połączenia między interjesami 1 i 2.");
                            nmap.Form.SetOfficial("\tSzczeliny: " + message[2] + "-" + message[3]);
                            nmap.Form.SetOfficial(GetTime() + "CC -> LRM : Żądanie dealokacji zasobów połączenia o parametrach:");
                            nmap.Form.SetOfficial("\tUżytkownik początkowy = \"" + decoded1 + "\", użytkownik końcowy = " + decoded2);



                            Tunnel tempTunnel = nmap.RemoveTunnel(name, message[1], message[2], message[3]);
                            foreach(KeyValuePair<string, string> entry in tempTunnel.Nodes)
                            {
                                if(toSend.ContainsKey(entry.Key) && entry.Key != "d1")
                                    toSend[entry.Key].Add("Remove|" + entry.Value);
                            }
                            if (domainName == "d2")
                                toSend["d1"].Add("Remove|2|" + message[2] + "|" + message[3] + "|1");
                        }
                        else if (message[0] == "ExNopeNMS")
                        {

                        }
                        else if (message[0] == "ExYupNMS")
                        {

                        }
                        else if (message[0] == "SubNopeNMS")
                        {

                        }
                        else if (message[0] == "SubYupNMS")
                        {

                        }
                        else
                        {
                            nmap.Form.SetLog(GetTime() + "BŁĄD! Otrzymano od urządzenia \"" + name + "\" wiadomość nieznanego typu!");
                        }
                    }
                    else  // test czy klient jeszcze żyje
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
                catch (Exception e)
                {
                    nmap.Form.SetLog(GetTime() + "Wyjątek w komunikacji z klientem \"" + name + "\": " + e.Message);
                    break;
                }

                if (name != "-" && toSend[name].Count != 0)
                { 
                    foreach(string text in toSend[name].ToList())
                    {
                        writer.Write(text);
                        writer.Flush();
                        nmap.Form.SetLog(GetTime() + "Urządzeniu \"" + name + "\" przesłano wiadomość:");
                        nmap.Form.SetLog("\t" + name + "::" + text);
                    }
                    toSend[name].Clear();

                    
                    /*if(toSend[name] == )
                    {
                        string[] repairTemp = repair.Split('|');
                        if (repairTemp[3] == "r5")
                        {
                            if (toSend["r7"] == "")
                            {
                                toSend["r7"] = "Add|1|" + repairTemp[1] + "|" + repairTemp[2] + "|2";
                                nmap.AddFib("r7", "1", "2", repairTemp[1], repairTemp[1]);
                                toSend["r9"] = "Add|1|" + repairTemp[1] + "|" + repairTemp[2] + "|3";
                                nmap.AddFib("r9", "1", "3", repairTemp[1], repairTemp[1]);

                            }
                        }
                        else if (repairTemp[3] == "r8")
                        {
                            if (toSend["r4"] != "")
                            {
                                toSend["r4"] = "Add|1|" + repairTemp[1] + "|" + repairTemp[2] + "|2";
                                nmap.AddFib("r4", "1", "2", repairTemp[1], repairTemp[1]);
                                toSend["r6"] = "Add|2|" + repairTemp[1] + "|" + repairTemp[2] + "|3";
                                nmap.AddFib("r6", "2", "3", repairTemp[1], repairTemp[1]);
                                repair = "";
                            }
                        }
                    }
                    else*/
                }
                
            }

            string tempSS = "";
            if(name == "r8" || name == "r5")
            {
                if (nmap.GetNodeFib(name).Count > 0)
                    tempSS = "|" + nmap.GetNodeFib(name)[0].first + "|" + nmap.GetNodeFib(name)[0].last;
                repair = name + tempSS;
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
            nmap.RemoveElement(name);

            nmap.Form.SetLog(GetTime() + "Rozłączono klienta \"" + name + "\".");
        }

        public void WorkWithDomains()
        {
            TcpClient clientNms = new TcpClient("127.0.0.1", 50001);

            NetworkStream stream = clientNms.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            BinaryReader reader = new BinaryReader(stream);

            string msg = "Hello|" + domainName;
            writer.Write(msg);
            writer.Flush();
            toSend.Add("d1", new List<string>());

            while (isOn)
            {
                msg = "-";
                try
                {
                    if (stream.DataAvailable)
                    {
                        msg = reader.ReadString();

                        string answer = HandleNmsMsg(msg);

                        if(answer != "-")
                        {
                            nmap.Form.SetLog(GetTime() + "Wysłano głównemu węzłowi wiadomość:");
                            nmap.Form.SetLog("\t" + answer);
                            writer.Write(answer);
                            writer.Flush();
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
                catch (Exception e)
                {
                    nmap.Form.SetLog(GetTime() + "Wyjątek w komunikacji z głównym systemem zarządzania w domenie \"" + domainName + "\": " + e.Message);
                }

                if(toSend["d1"].Count != 0)
                {
                    foreach (string text in toSend["d1"])
                    {
                        writer.Write(text);
                        writer.Flush();
                        nmap.Form.SetLog(GetTime() + "Urządzeniu \"" + "d1" + "\" przesłano wiadomość:");
                        nmap.Form.SetLog(text);
                    }
                    toSend["d1"].Clear();
                }
            }

            nmap.Form.SetLog(GetTime() + "Odłączono główny system zarządzania!");
        }

        public string HandleNmsMsg(string msg)
        {
            string[] message = msg.Split('|');
            string answer = "-";

            try
            {
                if (message[0] == "CallCoordination")  // "CallCoordination"|nazwa domeny-nadawcy|id routera nadawcy|nazwa domeny-odbiorcy|id routera odbiorcy|klient docelowy|przepustowość|
                {
                    if (domainName[0] != 'd')
                    {
                        nmap.Form.SetLog(GetTime() + "BŁĄD! Otrzymano wiadomość NIE przeznaczoną dla subdomeny!");
                        return "Error|" + domainName + "|Otrzymano wiadomość NIE przeznaczoną dla subdomeny!";
                    }

                    nmap.Form.SetOfficial(GetTime() + "NCC : Otrzymano żądanie koordynacji połączenia od domeny \"" + message[1] + "\". Parametry:");
                    nmap.Form.SetOfficial("\tID routera brzegowego nadawcy: " + message[2] + ", ID routera brzegowego adresata (tej domeny): " + message[4]);
                    nmap.Form.SetOfficial("\tUżytkownik docelowy: " + message[5] + ", Przepustowość: " + message[6]);


                    string tempClient = "";
                    if (message[5] == "c1")
                        tempClient = "c2";
                    else if (message[5] == "c2")
                        tempClient = "c1";
                    toSend[message[5]].Add("CallAccept|" + tempClient);

                    nmap.Form.SetOfficial(GetTime() + "NCC : Wysłano do klienta \"" + message[5] + "\" prośbę o akceptację połączenia od klienta \"" + tempClient + "\".");
                    // wstawić for() żeby zająć czas???
                    nmap.Form.SetOfficial(GetTime() + "NCC : Klient \"" + message[5] + "\" zaakceptował połączenie.");

                    string decoded = "";
                    nmap.Form.SetOfficial(GetTime() + "NCC -> D : Translacja nazw użytkowników na adresy.");
                    if (message[5] == "c1")
                    {
                        decoded = "1101";
                    }
                    else if (message[5] == "c2")
                    {
                        decoded = "2101";
                    }


                    nmap.Form.SetOfficial(GetTime() + "D -> NCC : Adres użytkownika docelowego (\"" + message[5] + "\") to " + decoded + ".");
                    nmap.Form.SetOfficial(GetTime() + "NCC -> CC : Żądanie koordynacji połączenia z domeną \"" + message[1] + "\" z powyższymi parametrami.");
                    nmap.Form.SetOfficial(GetTime() + "CC -> RC : Żądanie koordynacji topologii (dostępnych szczelin) z domeną \"" + message[1] + "\".");
                    nmap.Form.SetOfficial(GetTime() + "RC : Wymieniono informacja o dostępnych szczelinach z domeną \"" + message[1] + "\".");
                    // nmap robi Dijkstre ale bez wyznacznania najkrótszej z najkrótszych - zwraca szczeliny na których znalazł najkrótsze trasy
                    // tutaj nie robimy uzgadniania, od razu wyznaczamy co trzeba

                    Dictionary<string, string> tempDict = new Dictionary<string, string>(nmap.GetPathPorts(nmap.TranslateID(message[4]), message[5], message[6], "").Nodes);

                    string tempS = "";
                    string route = "";
                    for (int i = 0; i < tempDict.Count; i++)
                    {
                        tempS += "\t" + tempDict.ElementAt(i).Key + "===" + tempDict.ElementAt(i).Value + Environment.NewLine;
                        route = tempDict.ElementAt(i).Key + " -> " + route;
                    }
                    route = route.Substring(0, route.Length - 4);
                    nmap.Form.SetLog(GetTime() + "Ścieżka: " + tempS);

                    if (tempDict.Count > 1)
                    {
                        nmap.Form.SetOfficial(GetTime() + "RC -> CC : Odnaleziono ścieżkę.");
                        nmap.Form.SetOfficial("\t" + route);
                        nmap.Form.SetOfficial(GetTime() + "CC -> LRM : Żądanie alokacji zasobów na potrzeby połącznia pomiędzy routeram brzegowym o ID " + message[4] + " oraz użytkownikiem \"" + message[5] + "\" ścieżką otrzymaną od RC.");

                        for (int i = 1; i < tempDict.Count - 1; i++)
                        {
                            toSend[tempDict.ElementAt(i).Key].Add("Add|" + tempDict.ElementAt(i).Value);
                        }
                        string[] veryTemp = tempDict.ElementAt(tempDict.Count - 1).Value.Split('|');  // szczeliny first i last
                        // przesłanie do węzła brzegowego (zwykle jest to klient a nie węzeł):
                        if(domainName == "d1")
                        {
                            toSend[nmap.TranslateID(message[4])].Add("Add|2|" + veryTemp[veryTemp.Length - 2] + "|" + veryTemp[veryTemp.Length - 1] + "|" + nmap.FindPort(nmap.TranslateID(message[4]), tempDict.ElementAt(tempDict.Count - 2).Key));
                            nmap.AddFib(nmap.TranslateID(message[4]), "2", nmap.FindPort(nmap.TranslateID(message[4]), tempDict.ElementAt(1).Key), veryTemp[veryTemp.Length - 2], veryTemp[veryTemp.Length - 1]);
                            nmap.ChangeTunnelConf(nmap.TranslateID(message[4]), message[5], veryTemp[veryTemp.Length - 2], veryTemp[veryTemp.Length - 1], nmap.TranslateID(message[4]), "2|" + veryTemp[veryTemp.Length - 2] + "|" + veryTemp[veryTemp.Length - 1] + "|" + nmap.FindPort(nmap.TranslateID(message[4]), tempDict.ElementAt(tempDict.Count - 2).Key));
                        }
                        else
                        {
                            toSend[nmap.TranslateID(message[4])].Add("Add|1|" + veryTemp[veryTemp.Length - 2] + "|" + veryTemp[veryTemp.Length - 1] + "|" + nmap.FindPort(nmap.TranslateID(message[4]), tempDict.ElementAt(tempDict.Count - 2).Key));
                            nmap.AddFib(nmap.TranslateID(message[4]), "1", nmap.FindPort(nmap.TranslateID(message[4]), tempDict.ElementAt(1).Key), veryTemp[veryTemp.Length - 2], veryTemp[veryTemp.Length - 1]);
                            nmap.ChangeTunnelConf(nmap.TranslateID(message[4]), message[5], veryTemp[veryTemp.Length - 2], veryTemp[veryTemp.Length - 1], nmap.TranslateID(message[4]), "1|" + veryTemp[veryTemp.Length - 2] + "|" + veryTemp[veryTemp.Length - 1] + "|" + nmap.FindPort(nmap.TranslateID(message[4]), tempDict.ElementAt(tempDict.Count - 2).Key));
                        }

                        answer = "ExYupNMS|" + message[1] + "|" + message[2] + "|" + message[3] + "|" + message[4];

                        nmap.Form.SetOfficial(GetTime() + "LRM -> RC : Uaktualnienie informacji o stanie podsieci.");
                        // ma to już miejsce w funkcji GetPathPorts() (patrz AllocateChannels() i końcowe foreach)

                        
                    }
                    else   // czyli jest tylko potencjalna wiadomość dla kienta lub nie ma nic - brak routerów po drodze
                    {
                        nmap.Form.SetOfficial(GetTime() + "RC -> CC : NIE odnaleziono ścieżki.");
                        answer = "ExNopeNMS|" + message[1] + "|" + message[2] + "|" + message[3] + "|" + message[4];
                    }


                }
                else if (message[0] == "Connect")  // "Connect"|nazwa domeny-nadawcy|id routera wejściowego|id routera wyjściowego|przepustowość|
                {
                    // nieaktualne
                }
                else if (message[0] == "Add") // "Add"|id_wej|first|last|id_wyj
                {
                    if (domainName[0] == 's')
                    {
                        nmap.Form.SetOfficial(GetTime() + "CC : ConnectionRequest() - otrzymano żądanie zestawienia połączenia między routerami brzegowymi o ID " + message[1] + " oraz " + message[4] + ".");
                        nmap.Form.SetOfficial("\tSzczeliny: " + message[2] + "-" + message[3]);
                        nmap.Form.SetOfficial(GetTime() + "CC -> RC : Żądanie znalezienia ścieżki pomiędzy routerami brzegowymi o ID " + message[1] + " oraz " + message[4] + ".");

                        string tempChan = message[2] + "-" + message[3];
                        

                        Dictionary<string, string> tempDict = new Dictionary<string, string>(nmap.GetPathPorts(nmap.TranslateID(message[1]), nmap.TranslateID(message[4]), tempChan, "").Nodes);
                        string tempS = "";
                        string route = "";
                        for (int i = 0; i < tempDict.Count; i++)
                        {
                            tempS += "\t" + tempDict.ElementAt(i).Key + "===" + tempDict.ElementAt(i).Value + Environment.NewLine;
                            route = tempDict.ElementAt(i).Key + " -> " + route;
                        }
                        route = route.Substring(0, route.Length - 4);
                        nmap.Form.SetLog(GetTime() + "Ścieżka: " + tempS);

                        if (tempDict.Count > 1)
                        {
                            nmap.Form.SetOfficial(GetTime() + "RC -> CC : Odnaleziono ścieżkę:");
                            nmap.Form.SetOfficial("\t" + route);
                            nmap.Form.SetOfficial(GetTime() + "CC -> LRM : Żądanie alokacji zasobów na potrzeby połącznia pomiędzy routerami brzegowymi o ID " + message[2] + " oraz " + message[3] + " ścieżką otrzymaną od RC.");

                            for (int i = 1; i < tempDict.Count - 1; i++)
                            {
                                toSend[tempDict.ElementAt(i).Key].Add("Add|" + tempDict.ElementAt(i).Value);
                            }

                            string[] veryTemp = tempDict.ElementAt(tempDict.Count - 1).Value.Split('|');

                            string tempPort1 = "";
                            string tempPort2 = "";

                            if(nmap.TranslateID(message[1]) == "r4")
                            {
                                tempPort1 = "1";
                                tempPort2 = "3";
                            }
                            else
                            {
                                tempPort1 = "3";
                                tempPort2 = "1";
                            }
                            // przesłanie do węzła brzegowego (zwykle jest to klient a nie węzeł):
                            toSend[nmap.TranslateID(message[1])].Add("Add|" + tempPort1 + "|" + veryTemp[veryTemp.Length - 2] + "|" + veryTemp[veryTemp.Length - 1] + "|" + nmap.FindPort(nmap.TranslateID(message[1]), tempDict.ElementAt(tempDict.Count-2).Key));
                            toSend[nmap.TranslateID(message[4])].Add("Add|" + nmap.FindPort(nmap.TranslateID(message[4]), tempDict.ElementAt(tempDict.Count - 2).Key) + "|" + veryTemp[veryTemp.Length - 2] + "|" + veryTemp[veryTemp.Length - 1] + "|" + tempPort2);
                            nmap.AddFib(nmap.TranslateID(message[1]), tempPort1, nmap.FindPort(nmap.TranslateID(message[1]), tempDict.ElementAt(1).Key), veryTemp[veryTemp.Length - 2], veryTemp[veryTemp.Length - 1]);
                            nmap.AddFib(nmap.TranslateID(message[4]), nmap.FindPort(nmap.TranslateID(message[4]), tempDict.ElementAt(tempDict.Count - 2).Key), tempPort2, veryTemp[veryTemp.Length - 2], veryTemp[veryTemp.Length - 1]);
                            nmap.ChangeTunnelConf(nmap.TranslateID(message[1]), nmap.TranslateID(message[4]), message[2], message[3], nmap.TranslateID(message[1]), tempPort1 + "|" + veryTemp[veryTemp.Length - 2] + "|" + veryTemp[veryTemp.Length - 1] + "|" + nmap.FindPort(nmap.TranslateID(message[1]), tempDict.ElementAt(tempDict.Count - 2).Key));
                            nmap.ChangeTunnelConf(nmap.TranslateID(message[1]), nmap.TranslateID(message[4]), message[2], message[3], nmap.TranslateID(message[4]), nmap.FindPort(nmap.TranslateID(message[4]), tempDict.ElementAt(tempDict.Count - 2).Key) + "|" + veryTemp[veryTemp.Length - 2] + "|" + veryTemp[veryTemp.Length - 1] + "|" + tempPort2);
                            answer = "SubYupNMS|" + message[1] + "|" + message[2] + "|" + message[3] + "|" + message[4];

                            nmap.Form.SetOfficial(GetTime() + "LRM -> RC : Uaktualnienie informacji o stanie podsieci.");
                            // ma to już miejsce w funkcji GetPathPorts() (patrz AllocateChannels() i końcowe foreach)
                        }
                    }

                }
                else if (message[0] == "Remove")  // "Remove"|id_wej|first|last|id_wyj
                {
                    if (domainName == "d1")
                    {
                        nmap.Form.SetOfficial(GetTime() + "NCC : CallTeardown() - Otrzymano żądanie rozłączenia połączenia od węzła sterującego domeny \"d2\".");
                        nmap.Form.SetOfficial(GetTime() + "NCC -> CC : CallTeardown() - Przesłano żądanie rozłączenia połączenia. Parametry:");
                    }
                    else if (domainName == "d2")
                    {
                        nmap.Form.SetOfficial(GetTime() + "NCC : CallTeardown() - Otrzymano żądanie rozłączenia połączenia od węzła sterującego domeny \"d1\".");
                        nmap.Form.SetOfficial(GetTime() + "NCC -> CC : CallTeardown() - Przesłano żądanie rozłączenia połączenia. Parametry:");
                    }
                    else if (domainName == "s1")
                        nmap.Form.SetOfficial(GetTime() + "CC : CallTeardown() - Otrzymano żądanie rozłączenia połączenia. Parametry:");

                    nmap.Form.SetOfficial("\tID węzła wejściowego = " + message[1] + ", ID węzła wyjściowego = " + message[4]);
                    nmap.Form.SetOfficial("\tSzczeliny = " + message[2] + "-" + message[3]);
                    nmap.Form.SetOfficial(GetTime() + "CC -> LRM : Żądanie dealokacji zasobów połączenia o parametrach:");
                    nmap.Form.SetOfficial("\tUżytkownik początkowy = \"" + nmap.TranslateID(message[1]) + "\", użytkownik końcowy = " + nmap.TranslateID(message[4]));



                    Tunnel tempTunnel = nmap.RemoveTunnel(nmap.TranslateID(message[1]), nmap.TranslateID(message[4]), message[2], message[3]);
                    foreach (KeyValuePair<string, string> entry in tempTunnel.Nodes)
                    {
                        if (toSend.ContainsKey(entry.Key))
                            toSend[entry.Key].Add("Remove|" + entry.Value);
                    }

                    string tempClientStart = "";
                    string tempClientEnd = "";
                    if (domainName == "d1")
                    {
                        tempClientStart = "c2";
                        tempClientEnd = "c1";
                    }
                    else if (domainName == "d2")
                    {
                        tempClientStart = "c1";
                        tempClientEnd = "c2";
                    }
                    toSend[tempClientEnd].Add("CallTeardown|" + tempClientStart);
                    nmap.Form.SetOfficial(GetTime() + "NCC : Przeszłano klientowi \"" + tempClientEnd + "\" informację o zerwaniu połączenia przez klienta \"" + tempClientStart + "\".");
                }
                else if (message[0] == "Error")
                {
                    nmap.Form.SetLog(GetTime() + "UWAGA! Węzeł zarządzanie \"" + message[1] + "\" przysłał komunikat o błędzie:");
                    nmap.Form.SetLog("\t" + message[2]);
                }
                else if (message[0] == "ExNopeNMS")
                {

                }
                else if (message[0] == "ExYupNMS")
                {

                }
                else if (message[0] == "SubNopeNMS")
                {

                }
                else if (message[0] == "SubYupNMS")
                {

                }
                else
                {
                    nmap.Form.SetLog(GetTime() + "BŁĄD! Otrzymano od innego systemu zarządzania wiadomość nieznanego typu!");
                    nmap.Form.SetLog("\t" + msg);
                    return "Error|" + domainName + "|Otrzymano od innego węzła zarządzania wiadomość nieznanego typu!";
                }

                
            }
            catch(Exception e)
            {
                nmap.Form.SetLog(GetTime() + "Wyjątek w komunikacji (HandleMessage) z głównym systemem zarządzania w domenie \"" + domainName + "\": " + e.Message);
            }
            return answer;
        }
    }
}
