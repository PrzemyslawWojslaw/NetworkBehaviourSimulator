using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSST_NMS
{
    class Node
    {
        string name;
        string edgeID; // jeśli router jest brzegowy
        bool isGood;  // czy nie uszkodzony
        List<string> clientsInDomain = new List<string>();  // jeśli węzeł to domena ("d1" zamiast "r1" w nazwie) to to jest lista klientów do których można się dostać
        List<FibRow> fib = new List<FibRow>(); // tablica "skąd-dokąd"

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string EdgeID
        {
            get { return edgeID; }
            set { edgeID = value; }
        }

        public List<FibRow> Fib
        {
            get { return fib; }
        }

        public Node(string n)
        {
            name = n;
            isGood = true;
            edgeID = "";
        }

        public List<string> GetFibTable()
        {
            List<string> temp = new List<string>();
            temp.AddRange(new string[] { "node", "in-port", "szczeliny", "out-port"});
            foreach (FibRow t in fib)
            {
                temp.AddRange(new string[] { t.portFrom, t.first.ToString() + "-" + t.last.ToString(), t.portTo });
            }

            return temp;
        }

        public List<string> GetFib()
        {
            List<string> temp = new List<string>();
            foreach (FibRow t in fib)
            {
                temp.AddRange(new string[] { t.portFrom, t.first.ToString(), t.last.ToString(), t.portTo });
            }

            return temp;
        }

        public void AddToFib(string sFrom, string sTo, int first, int last)
        {
            FibRow temp = new FibRow(sFrom, sTo, first, last);
            fib.Add(temp);
        }

        public void RemoveFromFib(string sFrom, string sTo, int first, int last)
        {
            foreach(FibRow t in fib)
            {
                if (t.portFrom == sFrom && t.portTo == sTo && t.first == first && t.last == last)
                {
                    fib.Remove(t);
                    break;
                }
            }
        }

        public void AddClientInDomain(string c)
        {
            clientsInDomain.Add(c);
        }

        public bool CheckClientInDomain(string n)
        {
            foreach(string s in clientsInDomain)
            {
                if (s == n)
                    return true;
            }
            return false;
        }
    }
}
