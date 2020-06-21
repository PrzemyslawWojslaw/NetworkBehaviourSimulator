using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSST_Node
{
    public class FibRow
    {
        string portFrom;
        string portTo;
        string first;  // numer pierwszej szczeliny zajętej
        string last;

        public FibRow(string from, string fir, string las, string to)
        {
            portFrom = from;
            first = fir;
            last = las;
            portTo = to;
        }

        public string PortFrom
        {
            get { return portFrom; }
        }

        public string PortTo
        {
            get { return portTo; }
        }

        public string First
        {
            get { return first; }
        }

        public string Last
        {
            get { return last; }
        }

        public bool Check(string from, string f, string l)
        {
            if (portFrom == from && first == f && last == l)
                return true;

            return false;
        }
    }
}
