using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace SynchroCore
{
    class main
    {
        //-----------
        static string configFileName;
        static string logFileName;


        //-----------


        static string IP;
        static IPEndPoint ipEndPoint;
        static Socket Listener;

        

        static bool initServer()
        {
            String host = Dns.GetHostName();
            IPAddress ipAddr = Dns.GetHostByName(host).AddressList[0];
            ipEndPoint = new IPEndPoint(ipAddr, 11000);
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IP = ipAddr.ToString();
            return true;
        }

        static bool startServer()
        {
            Listener.Bind(ipEndPoint);
                Listener.Listen(10);

                while (true)
                {
                    Socket Handler = Listener.Accept();
                }


            return true;
        }

        static bool initDefault()
        {
            configFileName = "SynchroCoreConfig.ini";
            logFileName = "SynchroCoreLog.log";

            return true;
        }

        static bool configWriteDefault()
        {
            StreamWriter off = new StreamWriter(configFileName);
            off.Write("#Its automaticaly generated config file, edit it if you need\r\nlogFileName gameLog.txt\r\n");
            off.Close();
            return true;
        }

        static bool configLoad()
        {
            if (!File.Exists(configFileName))
            {
                configWriteDefault();
            }
            StreamReader inf = new StreamReader(configFileName);
            while (!inf.EndOfStream)
            {
                string line = inf.ReadLine();
                string[] str = line.Split(' ');
                try
                {
                    if (str[0][0] == '#')
                    {
                        // Comentaries
                        continue;
                    }

                    if (str[0] == "logFileName")
                    {
                        logFileName = str[1];
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Filed load config\nWays to resolve it \n1)delete config file and programm create new valid config file\n2)Check file to valid\n");
                    return false;
                }
            }

            return true;
        }




        static void Main(string[] args)
        {

        }
    }
}
