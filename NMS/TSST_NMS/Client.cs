using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSST_NMS
{
    class Client
    {
        string name;
        string gate; // nazwa node'a do którego jest podłączony
        bool isGood;  // czy nie uszkodzony
        List<string> targets = new List<string>(); // znani inni klienci (z którymi możemy pogadać)
        Dictionary<string, string> channels = new Dictionary<string, string>(); // szczeliny dla danych celów

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Client(string n, string g)
        {
            name = n;
            gate = g;
        }

        public void AddTargetName(string name)
        {
            targets.Add(name);
        }

        public void AddTargetTunnel(string name, string first, string last)
        {
            channels.Add(name+" ("+first+")", first + "-" + last);
        }

        public void AddTarget(string name, string first, string last)
        {
            AddTargetName(name);
            AddTargetTunnel(name, first, last);
        }

        public List<string> GetFibTable()
        {
            List<string> temp = new List<string>();
            temp.AddRange(new string[] {"client", "cel", "szczeliny"});
            foreach (KeyValuePair<string, string> entry in channels)
            {
                temp.AddRange(new string[] { entry.Key, entry.Value });
            }

            return temp;
        }

        public List<string> GetFib()
        {
            /*List<string> temp = new List<string>();
            foreach (KeyValuePair<string, string> entry in channels)
            {
                temp.AddRange(new string[] { entry.Key, entry.Value });
            }*/

            List<string> temp = new List<string>(GetFibTable());
            temp.RemoveRange(0, 3);

            return temp;
        }
    }
}
