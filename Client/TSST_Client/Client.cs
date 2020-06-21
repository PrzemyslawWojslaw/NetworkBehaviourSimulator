using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace TSST_Client
{
    class Client
    {
        Form1 form;
        bool isOn;
        static TcpClient clientCloud;
        static TcpClient clientNms;
        string name;
        //List<string> targets = new List<string>();
        //List<string> channels = new List<string>();
        Dictionary<string, string> targets = new Dictionary<string, string>();

        public Client(ref Form1 win, string domain, string n)
        {
            form = win;
            form.SetLabel("Klient nr " + n);
            name = "c" + n;
            clientCloud = new TcpClient("127.0.0.1", 60000);
            clientNms = new TcpClient("127.0.0.1", 50000 + Int32.Parse(domain));
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
            
            form.SetLog(GetTime() + "Uruchomiono klienta.");

            Thread thread = new Thread(new ThreadStart(Receive));
            thread.Start();
            Thread thread2 = new Thread(new ThreadStart(Send));
            thread2.Start();

            //loadConfig(name);

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

            form.SetLog(GetTime() + "Wyłączono klienta");
        }

        public void Receive()
        {
            NetworkStream streamNms = clientNms.GetStream();
            BinaryReader readerNms = new BinaryReader(streamNms);
            NetworkStream streamCloud = clientCloud.GetStream();
            BinaryReader readerCloud = new BinaryReader(streamCloud);
            string message;

            while(isOn)
            {
                if (streamNms.DataAvailable)
                {
                    try
                    {
                        message = readerNms.ReadString();
                        string[] temp = message.Split('|');
                        if (temp[0] == "Config")
                        {
                            /*targets.Clear();
                            channels.Clear();
                            for (int i = 1; i < temp.Length; i++)
                            {
                                if (i % 2 == 1)
                                    targets.Add(temp[i]);
                                else
                                    channels.Add(temp[i]);
                            }
                            form.SetLog(GetTime() + "Ustalono nową konfigurację.");*/
                        }
                        else if (temp[0] == "Add")
                        {
                            form.SetLog(GetTime() + "System zarządzania zestawił połączenie z użytkownikiem \"" + temp[1] + "\".");
                            targets.Add(temp[1] + " (" + temp[2] + "-" + temp[3] + ")", temp[2] + "|" + temp[3]);
                            form.AddTarget(temp[1], temp[2], temp[3]);
                        }
                        else if (temp[0] == "Remove")
                        {
                            targets.Remove(temp[1] + " (" + temp[2] + "-" + temp[3] + ")");
                            form.RemoveTarget(temp[1], temp[2], temp[3]);
                            form.SetLog(GetTime() + "Zerwano połączenie z użytkownikiem \"" + temp[1] + "\" na szczelinach " + temp[2] + "-" + temp[3] + ".");
                        }
                        else if(temp[0] == "CallAccept")
                        {
                            form.SetLog(GetTime() + "Otrzymano prośbę o akceptację połączenia od użytkownika \"" + temp[1] + "\".");
                            form.SetLog(GetTime() + "Zaakceptowano połączenie od użytkownika \"" + temp[1] + "\".");
                        }
                        else if (temp[0] == "CallTeardown")
                        {
                            form.SetLog(GetTime() + "Otrzymano informację o zerwaniu połączenia przez użytkownika \"" + temp[1] + "\".");
                        }
                        else if (temp[0] == "Nope")
                        {
                            form.SetLog(GetTime() + "Zestawienie połączenia o przepustowości " + temp[2] + " z użytkownikiem \"" + temp[1] + "\" NIE powiodło się!");
                        }
                        else
                        {
                            form.SetLog(GetTime() + "BŁĄD! Otrzymano od systemu zarządzania wiadomość nieznanego typu!");
                            form.SetLog(message);
                        }
                    }
                    catch (Exception e)
                    {
                        form.SetLog(GetTime() + "Wyjątek-NMS:" + e.Message);
                        break;
                    }
                }

                if(streamCloud.DataAvailable)
                {
                    try
                    {
                        message = readerCloud.ReadString();
                        string[] temp = message.Split('|');
                        if (temp[0] == "Message")
                        {
                            form.SetLog(GetTime() + "NOWA WIADOMOŚĆ : Od użytkownika \"" + temp[4] + "\" otrzymano następującą wiadomość: ");
                            form.SetLog("\t" + temp[5]);
                        }
                        else
                        {
                            form.SetLog(GetTime() + "BŁĄD! Otrzymano od chmury kablowej wiadomość nieznanego typu!");
                        }
                    }
                    catch (Exception e)
                    {
                        form.SetLog(GetTime() + "Wyjątek-chmura:" + e.Message);
                        break;
                    } 
                }

            }
            
        }

        public void Send()  // budowa wiadomości: "Message"|1(port)|firt-channel|last|nadawca|adresat|treść
        {
            NetworkStream streamNms = clientNms.GetStream();
            BinaryWriter writerNms = new BinaryWriter(streamNms);
            NetworkStream streamCloud = clientCloud.GetStream();
            BinaryWriter writerCloud = new BinaryWriter(streamCloud);
            string message;

            while (isOn)
            {
                try
                {
                    if (form.msg != "")
                    {
                        string[] temp = form.msg.Split('|');
                        string adress = temp[0];

                        temp[0] = targets[temp[0]]; // targets = first|last

                        message = "Message|1|" + temp[0] + "|" + name + "|" + temp[1];
                        writerCloud.Write(message);
                        writerCloud.Flush();
                        form.SetLog(GetTime() + "Wysłano do użytkownika \"" + adress + "\" wiadomość o treści:");
                        form.SetLog("\t" + temp[1]);
                        form.msg = "";
                    }

                    if (form.request != "" && form.request[0] != 'x')
                    {
                        string[] temp = form.request.Split('|');

                        writerNms.Write("CallRequest|" + form.request);
                        form.SetLog(form.request);
                        writerNms.Flush();
                        form.SetLog(GetTime() + "Wysłano do systemu zarządzania żądanie zestawienia połączenia o przeupstowości " + temp[1] + " (symbolowa - " + temp[2] + ") z użytkownikiem \"" + temp[0] + "\".");
                        form.request = "";
                    }
                    else if(form.request != "" && form.request[0] == 'x') // x|c2 (0-1)
                    {
                        string[] temp = form.request.Split('|');
                        temp = temp[1].Split(' ');
                        string[] tempChann = temp[1].Substring(1, temp[1].Length - 2).Split('-');

                        writerNms.Write("CallTeardown|" + temp[0] + "|" + tempChann[0] + "|" + tempChann[1]);
                        form.SetLog(form.request);
                        writerNms.Flush();
                        form.SetLog(GetTime() + "Wysłano do systemu zarządzania żądanie zerwania połączenia. Parametry:");
                        form.SetLog("\tużytkownik docelowy = \"" + temp[0] + "\", szczeliny = " + tempChann[0]+"-"+tempChann[1]);
                        form.request = "";
                    }
                }
                catch (Exception e)
                {
                    form.SetLog(GetTime() + "Wyjątek-wysyłanie:" + e.Message);
                    break;
                }
                
            }
        }

        public void loadConfig(string n)
        {
            /*string conf = System.IO.File.ReadAllText(name + "_config.txt");
            string[] temp = conf.Split('|');

            targets.Clear();
            channels.Clear();
            for (int i = 0; i < temp.Length; i++)
            {
                if (i % 2 == 0)
                    targets.Add(temp[i]);
                else
                    channels.Add(temp[i]);
            }
            form.SetLog(GetTime() + "Ustalono nową konfigurację.");*/
        }
    }
}
