using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSST_NMS
{
    class Link  // symbolizuje również kabel jako taki
    {
        public int distance;
        public string nameA;
        public string portA;
        public string nameB;
        public string portB;
        public bool[] channels = new bool[320];

        public Link(int d, string nA, string pA, string nB, string pB)
        {
            distance = d;
            nameA = nA;
            portA = pA;
            nameB = nB;
            portB = pB;

            for(int i = 0; i < channels.Length; i++)
            {
                channels[i] = true;
            }
        }

        public bool Check(int first, int number)
        {
            for(int i = first; i < first + number; i++)
            {
                if (channels[i] == false)
                    return false;
            }

            return true;
        }

        public void ChangeChannels(int first, int last)
        {
            for (int i = first; i <= last; i++)
                channels[i] = !channels[i];
        }

        public void AllocateChannels(int first, int last)
        {
            for (int i = first; i <= last; i++)
                channels[i] = false;
        }

        public void FreeChannels(int first, int last)
        {
            for (int i = first; i <= last; i++)
                channels[i] = true;
        }
    }
}
