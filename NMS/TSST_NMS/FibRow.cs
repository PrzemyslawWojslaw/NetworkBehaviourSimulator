using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSST_NMS
{
    class FibRow
    {
        public string portFrom;
        public string portTo;
        public int first; // numer pierwszej szczeliny
        public int last; // numer ostatniej szczeliny
        public int amount; // ilość szczelin

        public FibRow(string pF, string pT, int f, int l)
        {
            portFrom = pF;
            portTo = pT;
            first = f;
            last = l;
            amount = last - first + 1;
        }
    }
}
