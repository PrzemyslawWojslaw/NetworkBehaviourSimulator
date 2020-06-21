using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSST_Cloud_v2
{
    class Cable
    {
        int distance;
        string nameA;
        string portA;
        string nameB;
        string portB;

        public Cable(string config)  // config ma budowę: nazwaA:portA|nazwaB:portB|distance
        {
            string[] temp = config.Split('|');

            string[] temp1 = temp[0].Split(':');
            nameA = temp1[0];
            portA = temp1[1];

            string[] temp2 = temp[1].Split(':');
            nameB = temp2[0];
            portB = temp2[1];

            Int32.TryParse(temp[2], out distance);
        }

        public bool Check(string name, string port)
        {
            if ((nameA == name && portA == port) || (nameB == name && portB == port))
                return true;
            else
                return false;
        }

        public string[] GetEnd(string name, string port)
        {
            string[] temp = { "-", "-" };
            if (nameA == name && portA == port)
            {
                temp[0] = nameB;
                temp[1] = portB;
            }
            else if (nameB == name && portB == port)
            {
                temp[0] = nameA;
                temp[1] = portA;
            }

            return temp;
        }

        public string GetCableData() // nazwaA:portA|nazwaB:portB|distance
        {
            string temp;
            temp = nameA + ":" + portA + "|" + nameB + ":" + portB + "|" + distance;
            return temp;
        }

        /*public string myProperty
        {
            get { return _myProperty; }
            set { _myProperty = value; }
        }*/


    }
}
