using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchroCore
{
    class main
    {
        //----------- Global Vars
        static int tickCount;
        static int tickCurrent;
        static int playerCount;


        //----------- Config vars

        static string configFileName;
        static string logFileName;


        //----------- Network vars

        static string IP;
        static int port; //config
        static IPEndPoint ipEndPoint;
        static Socket listener;

        static List<Socket> players;
        static Socket world;
        static bool worldConnected;
        //-----------------------------------

        /// <summary>
        /// Initialiasing server network components, prepearing to server starting
        /// </summary>
        /// <returns></returns>
        static bool initServer()
        {
            String host = Dns.GetHostName();
            IPAddress ipAddr = Dns.GetHostByName(host).AddressList[0];
            ipEndPoint = new IPEndPoint(ipAddr, port);
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IP = ipAddr.ToString();
            return true;
        }

        /// <summary>
        /// Starts server and processing
        /// </summary>
        /// <returns></returns>
        static bool startServer()
        {
            listener.Bind(ipEndPoint);
            listener.Listen(10);
            cout("Server started at " + IP + ":"+port.ToString());
            
            Socket client = listener.Accept();

            byte[] buf = new byte[20];
            int count = client.Receive(buf);
            string msg = Encoding.ASCII.GetString(buf,0,count);
            if (msg == "world")
            {
                world = client;
                world.SendTimeout = 5;
                worldConnected = true;
                cout("world connected");
            }
            else
            {
                if (msg == "bot")
                {
                    players.Add(client);
                    cout("bot connected");
                }
                cout("WTF IS CONNECTED TO ME??!!!!! HEEELLLPPP!!! ITS OMG!!!!!");
            }

            //Send to world init data
            sendToWorld("init");
            if (!initWorld())
            {
                listener.Close();
                return false;
            }
            worker();

            //listener.Shutdown(SocketShutdown.Both);
            listener.Close();
            return true;
        }

        static bool sendToWorld(string msg)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(msg);
            world.Send(bytes);
            Thread.Sleep(100);
            return true;
        }

        static string receiveByWorld()
        {
            byte[] bytes = new byte[1024];
            int c = world.Receive(bytes);
            return Encoding.ASCII.GetString(bytes, 0, c);
        }

        static bool initWorld()
        {
            //send vars
            sendToWorld(playerCount.ToString());
            sendToWorld(tickCount.ToString());
            //Init players
///// Need replace to talk with bots by network////////////////////////////////////////////---------------!!!!!!!!!!!!!!!!!!
            for (int i = 0; i < playerCount; i++)
            {
                sendToWorld("player1");
                sendToWorld((i+1*15).ToString());
                sendToWorld("200");
                sendToWorld("320");
                cout("player1 sent");
            }

            if(receiveByWorld() != "OK")
            {
                return false;
            }
            return true;
        }

        static bool worker()
        {
            sendToWorld("START");
            while (tickCurrent < tickCount)
            {
                sendToWorld("15 MOVETO 100 " + (300+tickCurrent).ToString()); // debug string
                cout("sent to world '15 MOVETO 100 " + (300 + tickCurrent).ToString()+"'"); // debug
                if (receiveByWorld() == "EXIT")
                {
                    cout("world sent me 'EXIT'");
                    return false;
                }
                tickCurrent++;
            }
            sendToWorld("END");
            return true;
        }


        /// <summary>
        /// PreInitialisation before call loadconfig, for case when config not cpntaints something of vars
        /// </summary>
        /// <returns>bool</returns>
        static bool initDefault()
        {
            players = new List<Socket>();
            worldConnected = false;
            configFileName = "SynchroCoreConfig.ini";
            logFileName = "SynchroCoreLog.log";
            port = 11000;
            tickCount = 3000;
            tickCurrent = 0;
            return true;
        }

        /// <summary>
        /// Creates new config file, if config file not exists.
        /// </summary>
        /// <returns>bool</returns>
        static bool configWriteDefault()
        {
            StreamWriter off = new StreamWriter(configFileName);
            off.Write("#Its automaticaly generated config file, edit it if you need\r\nlogFileName synchroCore.log\r\ntickCount 2000\r\nplayerCount 1\r\n");
            off.Close();
            return true;
        }


        /// <summary>
        /// loading config from config file
        /// </summary>
        /// <returns>bool</returns>
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

                    if (str[0] == "tickCount")
                    {
                        tickCount =Convert.ToInt32(str[1]);
                        continue;
                    }
                    
                    if (str[0] == "playerCount")
                    {
                        playerCount = Convert.ToInt32(str[1]);
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

        static bool cout(string msg)
        {
            Console.WriteLine(msg);
            // logwrite(msg);
            return true;
        }

        static void shutdown()
        {
            
            

        }

        static void Main(string[] args)
        {
            cout("SynchroCore starting...");
            cout("Initilisation...");
            initDefault();
            cout("Loading config");
            if (!configLoad())
            {
                cout("Config load fail");
                return;
            }
            cout("Initing server...");
            initServer();
            cout("Starting server...");
            startServer();



            cout("Server stoped. press enter to exit.");
            Console.ReadLine();
        }
    }
}
