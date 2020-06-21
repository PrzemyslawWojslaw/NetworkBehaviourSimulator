using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSST_NMS
{
    class Tunnel
    {
        Dictionary<string, string> nodes = new Dictionary<string, string>();  // elementy tunelu i wytyczne dla wiadomości "Add"/"Remove" (czyli config)
        string startClient;
        string endClient;
        int first; // numer pierwszej szczeliny
        int last; // numer ostatniej szczeliny
        int amount; // ilość szczelin

        public Tunnel(string start, string end, string fir, string las)
        {
            startClient = start;
            endClient = end;
            first = Int32.Parse(fir);
            last = Int32.Parse(las);
            amount = last - first + 1;
        }

        public Dictionary<string, string> Nodes
        {
            get { return nodes; }
        }

        public string Start
        {
            get { return startClient; }
        }

        public string End
        {
            get { return endClient; }
        }

        public string First
        {
            get { return first.ToString(); }
        }

        public string Last
        {
            get { return last.ToString(); }
        }

        public void AddNode(string name, string config)
        {
            nodes.Add(name, config);
        }

        public void RemoveNode(string name)
        {
            if(nodes.ContainsKey(name))
                nodes.Remove(name);
        }

        public void ChangeConfig(string name, string config)
        {
            nodes[name] = config;
        }
    }
}
