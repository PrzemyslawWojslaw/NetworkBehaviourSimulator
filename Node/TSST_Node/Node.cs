using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace TSST_Node
{
    class Node
    {
        nodeForm form;
        bool isOn;
        static TcpClient clientCloud;
        static TcpClient clientNms;
        string name;
        List<FibRow> fib = new List<FibRow>();

        public Node(ref nodeForm win, string domain, string n)
        {
            form = win;
            form.SetLabel("Węzeł nr " + n);
            name = "r" + n;
            clientCloud = new TcpClient("127.0.0.1", 60000);
            clientNms = new TcpClient("127.0.0.1", 50000 + Int32.Parse(domain));
        }

        public void Start()
        {
            isOn = true;
            
            form.SetLog(GetTime() + "Uruchomiono węzeł.");
            Thread thread = new Thread(new ThreadStart(Work));
            thread.Start();

            string message = "Hello|" + name;
            NetworkStream stream = clientNms.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(message);
            writer.Flush();

            stream = clientCloud.GetStream();
            writer = new BinaryWriter(stream);
            writer.Write(message);
            writer.Flush();
        }

        public void Stop()
        {
            isOn = false;
            clientNms.Close();
            clientCloud.Close();

            form.SetLog(GetTime() + "Wyłączono węzeł");
        }

        public string GetTime()
        {
            DateTime now = DateTime.Now;
            //string temp = now.ToString();
            string temp = now.ToString("yyyy.MM.dd - HH:mm.ss.ffff");
            return temp + "  >>>  ";
        }

        public void Work()
        {
            string message;

            NetworkStream streamNms = clientNms.GetStream();
            BinaryReader readerNms = new BinaryReader(streamNms);
            BinaryWriter writerNms = new BinaryWriter(streamNms);

            NetworkStream streamCloud = clientCloud.GetStream();
            BinaryReader readerCloud = new BinaryReader(streamCloud);
            BinaryWriter writerCloud = new BinaryWriter(streamCloud);           

            while (isOn)
            {
                if (streamNms.DataAvailable)
                {
                    message = readerNms.ReadString();
                    string[] temp = message.Split('|');

                    if(temp[0] == "Config")
                    {
                        fib.Clear();

                        for (int i = 1; i < temp.Length-3; i+=4)
                        {
                            FibRow f = new FibRow(temp[i], temp[i + 1], temp[i + 2], temp[i + 3]);
                            fib.Add(f);
                            form.AddGrid(f);
                        }

                        form.SetLog(GetTime() + "Ustalono nową konfigurację.");
                    }
                    else if(temp[0] == "Add")
                    {
                        FibRow f = new FibRow(temp[1], temp[2], temp[3], temp[4]);
                        fib.Add(f);
                        form.AddGrid(f);
                        form.SetLog(GetTime() + "Otrzymano polecenie DODANIA wpisu do tablicy:");
                        form.SetLog("\tPortIn: " + temp[1] + "  Szczeliny: " + temp[2] + "-" + temp[3] + "  PortOut: " + temp[4]);
                    }
                    else if(temp[0] == "Remove")
                    {
                        FibRow f = new FibRow(temp[1], temp[2], temp[3], temp[4]);
                        for(int i = 0; i < fib.Count; i++)
                        {
                            if (fib[i].PortFrom == f.PortFrom && fib[i].PortTo == f.PortTo && fib[i].First == f.First && fib[i].Last == f.Last)
                                fib.RemoveAt(i);
                        }
                        form.RemoveGrid(f);
                        form.SetLog(GetTime() + "Otrzymano polecenie USUNIĘCIA wpisu do tablicy:");
                        form.SetLog("\tPortIn: " + temp[1] + "  Szczeliny: " + temp[2] + "-" + temp[3] + "  PortOut: " + temp[4]);
                    }
                    else
                    {
                        form.SetLog(GetTime() + "BŁĄD! Otrzymano od systemu zarządzania wiadomość nieznanego typu!");
                    }
                }

                if (streamCloud.DataAvailable)
                {
                    message = readerCloud.ReadString();
                    string[] temp = message.Split('|');

                    if (temp[0] == "Message")
                    {
                        form.SetLog(GetTime() + "Orzymano nową wiadomość:");
                        form.SetLog(message);
                        form.SetLog(">> Wiadomość przyszła na porcie " + temp[1] + " i szczelinami o numerach " + temp[2] + "-" + temp[3] + ".");
                        foreach(FibRow row in fib)
                        {
                            if (row.Check(temp[1], temp[2], temp[3]))
                                temp[1] = row.PortTo;
                        }

                        message = temp[0];
                        for (int i = 1; i < temp.Length; i++)
                        {
                            message = message + "|" + temp[i];
                        }

                        writerCloud.Write(message);
                        writerCloud.Flush();
                        form.SetLog(GetTime() + "Odesłano wiadomość:");
                        form.SetLog(message);
                    }
                    else
                    {
                        form.SetLog(GetTime() + "BŁĄD! Otrzymano od chmury kablowej wiadomość nieznanego typu!");
                    }
                }
            }

            string tempS = "";
            if (fib.Count > 0)
                tempS = fib[0].First + "|" + fib[0].Last;
            writerNms.Write("Repair|" + name + "|" + tempS);
            Thread.Sleep(10);
        }
    }
}
