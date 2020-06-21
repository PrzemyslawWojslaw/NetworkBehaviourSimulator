using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSST_NMS
{
    class Netmap
    {
        Form1 form;
        string msgToCloud;

        List<Node> nodes = new List<Node>();
        List<Client> clients = new List<Client>();
        List<Link> links = new List<Link>();   // reprezentacja chmury kablowej
        List<Tunnel> tunnels = new List<Tunnel>();  // reprezentacja ZESTAWIONYCH połączeń

        public Netmap(ref Form1 win, string n)
        {
            form = win;
            LoadConfig(n);
        }

        public Form1 Form
        {
            get { return form; }
        }

        public List<FibRow> GetNodeFib(string name)
        {
            foreach(Node n in nodes)
            {
                if (n.Name == name)
                    return n.Fib;
            }
            return new List<FibRow>();
        }

        public List<string> GetPath(string start, string end, string bits, string bods)
        {
            Dictionary<int, List<Link>> allLinks = new Dictionary<int, List<Link>> ();
            
            int channels;

            if(bits.Split('-').Length > 1)
            {
                string[] chann = bits.Split('-');
                int first = Int32.Parse(chann[0]);
                int last = Int32.Parse(chann[1]);
                List<Link> newLinks = new List<Link>();
                foreach (Link link in links)
                {
                    if (link.Check(first, last - first + 1))
                        newLinks.Add(link);
                }
                allLinks.Add(first, newLinks);


                Dictionary<int, List<string>> allShortest = new Dictionary<int, List<string>>();
                foreach (KeyValuePair<int, List<Link>> list in allLinks)
                {
                    Dijkstra graph = new Dijkstra(ref form);

                    foreach (Node node in nodes)
                    {
                        graph.AddVertex(node.Name, FindLinks(list.Value, node.Name));
                    }
                    foreach (Client client in clients)
                    {
                        graph.AddVertex(client.Name, FindLinks(list.Value, client.Name));
                    }
                    allShortest.Add(list.Key, graph.ShortestPath(start, end));
                }

                string startChannel = ""; // aby zapamiętać na których szczelinach to ma lecieć
                string endChannel = "";

                List<string> temp = new List<string>();
                for (int i = 0; i < allShortest.Count; i++)
                {
                    if ((temp.Count == 0 && allShortest.ElementAt(i).Value.Count != 0) || (allShortest.ElementAt(i).Value.Count != 0 && allShortest.ElementAt(i).Value.Count < temp.Count))
                    {
                        temp = allShortest.ElementAt(i).Value;
                        startChannel = chann[0];
                        endChannel = chann[1];
                        break;
                    }
                }

                temp.Insert(0, end);
                temp.Add(start);
                temp.Add(startChannel);
                temp.Add(endChannel);

                return temp;
            }
            else
            {
                switch (bits)
                {
                    case ("50 Gbit/s"):
                        channels = 4;
                        break;
                    case ("75 Gbit/s"):
                        channels = 6;
                        break;
                    case ("100 Gbit/s"):
                        channels = 8;
                        break;
                    case ("125 Gbit/s"):
                        channels = 10;
                        break;
                    case ("150 Gbit/s"):
                        channels = 12;
                        break;
                    default:
                        channels = 2;
                        break;
                }

                for (int i = 0; i < 320 - channels + 1; i++)
                {
                    List<Link> newLinks = new List<Link>();
                    foreach (Link link in links)
                    {
                        if (link.Check(i, channels))
                            newLinks.Add(link);
                    }
                    allLinks.Add(i, newLinks);
                }

                Dictionary<int, List<string>> allShortest = new Dictionary<int, List<string>>();
                foreach (KeyValuePair<int, List<Link>> list in allLinks)
                {
                    Dijkstra graph = new Dijkstra(ref form);

                    foreach (Node node in nodes)
                    {
                        graph.AddVertex(node.Name, FindLinks(list.Value, node.Name));
                    }
                    foreach (Client client in clients)
                    {
                        graph.AddVertex(client.Name, FindLinks(list.Value, client.Name));
                    }
                    allShortest.Add(list.Key, graph.ShortestPath(start, end));
                }

                string startChannel = ""; // aby zapamiętać na których szczelinach to ma lecieć
                string endChannel = "";

                List<string> temp = new List<string>();
                for (int i = 0; i < allShortest.Count; i++)
                {
                    if ((temp.Count == 0 && allShortest.ElementAt(i).Value.Count != 0) || (allShortest.ElementAt(i).Value.Count != 0 && allShortest.ElementAt(i).Value.Count < temp.Count))
                    {
                        temp = allShortest.ElementAt(i).Value;
                        startChannel = allShortest.ElementAt(i).Key.ToString();
                        int tempI = allShortest.ElementAt(i).Key + channels - 1;
                        endChannel = tempI.ToString();
                        break;
                    }
                }

                temp.Insert(0, end);
                temp.Add(start);
                temp.Add(startChannel);
                temp.Add(endChannel);

                return temp;
            }
            
        }

        public Tunnel GetPathPorts(string start, string end, string bits, string bods)
        {
            List<string> temp = GetPath(start, end, bits, bods);  // najkrótsza ścieżka + pierwsza szczelina + ostatnia (2 ostatne stringi)
            string config1 = "";
            string config2 = "";
            //if (temp.Count < 5) // nie odnaleziono ścieżki
            //    return answer;
            Tunnel tunnel = new Tunnel(start, end, temp[temp.Count - 2], temp[temp.Count - 1]);

            for (int i = 1; i < temp.Count - 3; i++) // nie chcemy 3 ostatnich wartości - bo to są: pierwszy (nasz) klient, pierwsza szczelina, ostatnia szczelina
            {
                foreach (Link link in links)
                {
                    if (link.nameA == temp[i-1] && link.nameB == temp[i])  // w temp są W ODWROTNEJ KOLEJNOŚCI WĘZŁY, czyli i-1 to ten DO którego wysyłamy
                    {
                        config2 = link.portB;
                        link.AllocateChannels(Int32.Parse(temp[temp.Count - 2]), Int32.Parse(temp[temp.Count - 1]));
                        
                    }
                    else if (link.nameB == temp[i - 1] && link.nameA == temp[i])
                    {
                        config2 = link.portA;
                        link.AllocateChannels(Int32.Parse(temp[temp.Count - 2]), Int32.Parse(temp[temp.Count - 1]));
                    }

                    if(link.nameA == temp[i + 1] && link.nameB == temp[i])
                    {
                        config1 = link.portB;
                        link.AllocateChannels(Int32.Parse(temp[temp.Count - 2]), Int32.Parse(temp[temp.Count - 1]));
                    }
                    else if (link.nameB == temp[i + 1] && link.nameA == temp[i])
                    {
                        config1 = link.portA;
                        link.AllocateChannels(Int32.Parse(temp[temp.Count - 2]), Int32.Parse(temp[temp.Count - 1]));
                    }
                }

                foreach(Node n in nodes)
                {
                    if (n.Name == temp[i])
                        n.AddToFib(config1, config2, Int32.Parse(temp[temp.Count - 2]), Int32.Parse(temp[temp.Count - 1]));
                }
                // "Add"|portForm|First|Last|portTo
                tunnel.AddNode(temp[i], config1 + "|" + temp[temp.Count - 2] + "|" + temp[temp.Count - 1] + "|" + config2);
            }
            // przypomnienie: 3 ostatnie wartości temp - nasz klient (start), pierwsza szczelina, ostatnia szczelina
            tunnel.AddNode(start, end + "|" + temp[temp.Count - 2] + "|" + temp[temp.Count - 1]);
            foreach(Client c in clients)
            {
                if(c.Name == start)
                {
                    c.AddTarget(end, temp[temp.Count - 2], temp[temp.Count - 1]);
                    break;
                }
            }

            tunnels.Add(tunnel);
            UpdateNetwindow();
            return tunnel;
        }

        public Dictionary<string, int> FindLinks(List<Link> allowedLinks, string name)
        {
            Dictionary<string, int> temp = new Dictionary<string, int>();
            foreach(Link link in allowedLinks)
            {
                if(link.nameA == name)
                {
                    temp.Add(link.nameB, link.distance);
                }
                else if (link.nameB == name)
                {
                    temp.Add(link.nameA, link.distance);
                }
            }

            return temp;
        }

        public void ChangeChannels(string name1, string name2, int start, int number)
        {
            foreach (Link link in links)
            {
                if ((link.nameA == name1 && link.nameB == name2) || (link.nameB == name1 && link.nameA == name2))
                {
                    for (int i = start; i < start + number; i++)
                        link.channels[i] = !link.channels[i];
                }
            }
        }

        public void AddElement(string name)
        {
            if(name[0] == 'r')
            {
                Node temp = new Node(name);
                nodes.Add(temp);
                form.Netwindow.AddElement(name);
            }
            else if(name[0] == 'c' && name[1] != 'l')
            {
                string tempS = "";

                foreach (Link link in links)
                {
                    if (link.nameA == name)
                        tempS = link.nameB;
                    else if (link.nameB == name)
                        tempS = link.nameA;
                }

                Client temp = new Client(name, tempS);
                clients.Add(temp);
                form.Netwindow.AddElement(name);
            }
            else if(name[0] == 'd')
            {
                
            }
        }

        public void RemoveElement(string name)
        {
            if (name[0] == 'r')
            {
                Node temp = new Node(name);
                nodes.Remove(temp);
                form.Netwindow.RemoveElement(name);
            }
            else if (name[0] == 'c' && name[1] != 'l')
            {
                string tempS = "";

                foreach (Link link in links)
                {
                    if (link.nameA == name)
                        tempS = link.nameB;
                    else if (link.nameB == name)
                        tempS = link.nameA;
                }

                Client temp = new Client(name, tempS);
                clients.Remove(temp);
                form.Netwindow.RemoveElement(name);
            }
        }

        public void LoadConfig(string number)
        {
            List<string> conf = new List<string>(System.IO.File.ReadAllLines("config_" + number + ".txt"));

            // chmura kabli z configa, np. c1:1|r1:1|10 r1:2|r2:1|20 r1:3|r3:1|30
            msgToCloud = conf[1];
            string[] cloudConf = conf[1].Split(' ');
            foreach(string part in cloudConf)
            {
                string[] temp = part.Split('|');

                string[] temp1 = temp[0].Split(':');
                //nameA = temp1[0];
                //portA = temp1[1];

                string[] temp2 = temp[1].Split(':');
                //nameB = temp2[0];
                //portB = temp2[1];

                //distance = temp[2];
                int tempInt;
                Int32.TryParse(temp[2], out tempInt);

                Link tempLink = new Link(tempInt, temp1[0], temp1[1], temp2[0], temp2[1]);
                links.Add(tempLink);
            }

            for (int i = 3; i < conf.Count; i++)
            {
                if(conf[i][0] == 'r')
                {
                    string[] tempS = conf[i].Split('|');
                    Node tempN = new Node(tempS[0]);
                    if (tempS.Length > 1)
                    {
                        tempN.EdgeID = tempS[2];
                    }
                    nodes.Add(tempN);
                    form.Netwindow.AddElement(tempS[0]);
                }
                else if(conf[i][0] == 'c')
                {
                    AddElement(conf[i]);
                }
                else if (conf[i][0] == 'd' || conf[i][0] == 's')  // domena lub subdomena
                {
                    string[] tempS = conf[i].Split('|');
                    Node tempN = new Node(tempS[0]);
                    
                    for(int j = 1; j < tempS.Length; j++)
                    {
                        Client tempC = new Client(tempS[j], tempS[0]);
                        clients.Add(tempC);
                        form.Netwindow.AddElement(tempS[j]);
                        tempN.AddClientInDomain(tempS[j]);
                    }

                    nodes.Add(tempN);
                    form.Netwindow.AddElement(tempS[0]);
                }
            }

            UpdateNetwindow();
        }

        public void UpdateNetwindow()
        {
            foreach(Node n in nodes)
            {
                form.Netwindow.EditElementFib(n.Name, n.GetFibTable());
                form.Netwindow.EditElementCable(n.Name, GetCables(n.Name));
            }

            foreach(Client c in clients)
            {
                form.Netwindow.EditElementFib(c.Name, c.GetFibTable());
                form.Netwindow.EditElementCable(c.Name, GetCables(c.Name));
            }
        }

        public List<string> GetCables(string s)
        {
            List<string> temp = new List<string>();
            temp.AddRange(new string[] { "router A", "port A", "router B", "port B", "odległość  [km]" });

            foreach (Link link in links)
            {
                if (link.nameA == s)
                {
                    temp.AddRange(new string[] { s, link.portA, link.nameB, link.portB, link.distance.ToString() });
                }
                else if (link.nameB == s)
                {
                    temp.AddRange(new string[] { s, link.portB, link.nameA, link.portA, link.distance.ToString() });
                }
            }

            return temp;
        }

        public string CreateConfig(string name)
        {
            string answer = "";

            if(name[0] == 'n')
            {
                foreach (Node node in nodes) // node
                {
                    if (node.Name == name)
                    {
                        List<string> temp = new List<string>(node.GetFib());

                        foreach (string s in temp)
                            answer += "|" + s;

                        return answer;
                    }
                }
            }
            else if(name[0] == 'c' && name[1] != 'l') // client, np. c1 (a nie cloud)
            {
                foreach (Client client in clients)
                {
                    if (client.Name == name)
                    {
                        List<string> temp = new List<string>(client.GetFib());

                        foreach (string s in temp)
                            answer += "|" + s;
                    }
                }
            }
            else if(name[0] == 'd')
            {

            }
            else if(name[0] == 'c' && name[1] == 'l') // cloud
            {
                answer = msgToCloud;
            }
           
            return answer;
        }

        public string CheckIfExDomain(string name)
        {
            foreach(Node node in nodes)
            {
                if(node.Name[0] == 'd')
                {
                    if (node.CheckClientInDomain(name))
                        return node.Name;
                }
            }

            return name;
        }

        public string TranslateID(string id)
        {
            foreach(Node node in nodes)
            {
                if (node.EdgeID == id)
                    return node.Name;
            }

            return "";
        }

        public string FindPort(string start, string end)
        {
            foreach (Link link in links)
            {
                if (link.nameA == start && link.nameB == end)
                {
                    return link.portA;
                }
                else if (link.nameB == start && link.nameA == end)
                {
                    return link.portB;
                }
            }
            return "";
        }

        public bool isFibClear(string name)
        {
            foreach (Node node in nodes)
            {
                if (node.Name == name && node.Fib.Count != 0)
                {
                    if (node.Fib.Count > 0)
                        return false;
                    else
                        return true;
                }
            }
            return true;
        }

        public void RemoveFib(string name, string pfrom, string pto, string fir, string las)
        {
            foreach (Node node in nodes)
            {
                if (node.Name == name && node.Fib.Count != 0)
                {
                    node.RemoveFromFib(pfrom, pto, Int32.Parse(fir), Int32.Parse(las));
                }
            }
            foreach (Tunnel tunnel in tunnels)
            {
                if (tunnel.Nodes.ContainsKey(name) && tunnel.Nodes[name] == pfrom + "|" + fir + "|" + las + "|" + pto)
                {
                    tunnel.Nodes.Remove(name);
                    if (name == "r5")
                    {
                        tunnel.Nodes["r4"] = "1|" + fir + "|" + las + "|2";
                        tunnel.Nodes["r6"] = "2|" + fir + "|" + las + "|3";
                    }
                    else if (name == "r8")
                    {
                        tunnel.Nodes["r7"] = "1|" + fir + "|" + las + "|2";
                        tunnel.Nodes["r9"] = "1|" + fir + "|" + las + "|3";
                    }
                    else if (name == "r2")
                        tunnels.Remove(tunnel);
                }
            }
            UpdateNetwindow();
        }

        public void AddFib(string name, string pfrom, string pto, string fir, string las)
        {
            foreach (Node node in nodes)
            {
                if (node.Name == name && node.Fib.Count != 0)
                {
                    node.AddToFib(pfrom, pto, Int32.Parse(fir), Int32.Parse(las));
                }
            }
            UpdateNetwindow();
        }

        public Tunnel GetTunnel(string name)
        {
            Tunnel temp = new Tunnel("-", "-", "0", "0");

            foreach (Tunnel tunnel in tunnels)
            {
                if (tunnel.Nodes.ContainsKey(name))
                {
                    temp = tunnel;
                    if (name == "r2")
                        tunnels.Remove(tunnel);
                    break;
                }
                    
            }

            return temp;
        }

        public void ChangeTunnelConf(string start, string end, string first, string last, string name, string conf)
        {
            foreach (Tunnel tunnel in tunnels)
            {
                if (tunnel.Start == start && tunnel.End == end && tunnel.First == first && tunnel.Last == last)
                {
                    if (conf != "Remove")
                        tunnel.Nodes[name] = conf;
                    else
                        tunnel.Nodes.Remove(name);
                }
            }
        }

        public Tunnel RemoveTunnel(string start, string end, string first, string last)
        {
            Tunnel temp = new Tunnel("-", "-", "0", "0");

            foreach (Tunnel tunnel in tunnels)
            {
                if (tunnel.Start == start && tunnel.End == end && tunnel.First == first && tunnel.Last == last)
                {
                    temp = tunnel;
                    // usunięcie tunelu z pamięci?
                    break;
                }
                else if(start[0] == 'r' && end[0] == 'r' && tunnel.First == first && tunnel.Last == last)
                {
                    temp = tunnel;
                    // usunięcie tunelu z pamięci?
                    break;
                }

            }

            return temp;
        }

        /*public string Repair(string name)
        {
            foreach(Tunnel tunnel in tunnels)
            {
                if(tunnel.Nodes.ContainsKey(name))
                {
                    if (tunnel.Nodes.ElementAt(0).Key == name || tunnel.Nodes.ElementAt(tunnel.Nodes.Count - 1).Key == name)
                        return "-";

                    Dijkstra graph = new Dijkstra(ref form);

                    foreach (Node node in nodes)
                    {
                        graph.AddVertex(node.Name, FindLinks(list, node.Name));
                    }
                    foreach (Client client in clients)
                    {
                        graph.AddVertex(client.Name, FindLinks(list, client.Name));
                    }
                    graph.ShortestPath(start, end);
                }
            }
        }*/
    }
}
