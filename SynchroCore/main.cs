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
using Definitions;

namespace SynchroCore
{
    class main
    {
        //----------- Global Vars
        static int tickCount;
        static int tickCurrent;
        static int playerCount;
        static string botsInitializationString;


        //----------- Config vars

        static string configFileName;
        static string logFileName;


        //----------- Network vars

        static string IP;
        static int port; //config
        static IPEndPoint ipEndPoint;
        static Socket listener;

        //static List<Socket> players;
        static Socket botServer;
        static Socket world;
        static bool worldConnected;
        static bool botServerConnected;
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

            while (botServerConnected == false || worldConnected == false)
            {
                Socket client = listener.Accept();

                byte[] buf = new byte[25];
                int count = client.Receive(buf);
                string msg = Encoding.ASCII.GetString(buf, 0, count);
                if (msg == Vals.NETWORK_NAME_GAMEWORLD)
                {
                    world = client;
                    //world.SendTimeout = 10;
                    worldConnected = true;
                    cout("world connected");
                    sendToWorld(Vals.NETWORK_COMMAND_OK);
                }
                else
                {
                    if (msg == Vals.NETWORK_NAME_BOTSERVER)
                    {
                        botServer = client;
                        //botServer.SendTimeout = 10;
                        botServerConnected = true;
                        cout("BotServer connected");
                        sendToBotServer(Vals.NETWORK_COMMAND_OK);
                    }
                    else
                    {
                        cout("WTF HAS BEEN CONNECTED TO ME??!!!!! HEEELLLPPP!!! IT'S OMG!!!!!");
                    }
                }
            }

            // Initialization of clients and sending and receiving data
            Thread.Sleep(Vals.VALUE_NETWORK_SYNC_PAUSETIME);
            
            if (!InitBotServer())
            {
                listener.Close();
                return false;
            }
            cout("BotServer inited [ok]");
            cout("Starting init GameWorld");
            if (!initWorld())
            {
                listener.Close();
                return false;
            }
            cout("GameWorld Inited [ok]");

            worker();

            //listener.Shutdown(SocketShutdown.Both);
            listener.Close();
            return true;
        }

        static bool sendToWorld(string msg)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(msg);
            //Thread.Sleep(Vals.VALUE_NETWORK_SYNC_PAUSETIME);
            world.Send(bytes);
            
            return true;
        }

        static bool sendToBotServer(string msg)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(msg);
            //Thread.Sleep(Vals.VALUE_NETWORK_SYNC_PAUSETIME);
            botServer.Send(bytes);
            
            return true;
        }

        static string receiveByWorld()
        {
            byte[] bytes = new byte[1024];
            int c = world.Receive(bytes);
            return Encoding.ASCII.GetString(bytes, 0, c);
        }

        static string receiveByBotServer()
        {
            byte[] bytes = new byte[1024];
            int c = botServer.Receive(bytes);
            return Encoding.ASCII.GetString(bytes, 0, c);
        }

        static bool initWorld()
        {
            sendToWorld(Vals.NETWORK_COMMAND_INIT);
            cout("Init command sent to world [ok]");
            
            //send vars

            sendToWorld(tickCount.ToString());
            //cout("tick count sent [ok]");
            receiveByWorld();

            sendToWorld(botsInitializationString);

            cout("bots string sent [ok]");
            //Thread.Sleep(Vals.VALUE_NETWORK_SYNC_PAUSETIME);
            receiveByWorld();

            return true;
        }

        static bool InitBotServer()
        {
            sendToBotServer(Vals.NETWORK_COMMAND_INIT);

            string botsStr = receiveByBotServer();
            sendToBotServer(Vals.NETWORK_COMMAND_OK);
            if (botsStr.Length < 3)
            {
                cout("bad data received by BotServer while network init");
                return false;
            }
            else
            {
                cout(botsStr);
                //Console.ReadLine();
            }

            botsInitializationString = botsStr;
            return true;
        }

        static bool worker()
        {
            sendToWorld("START");
            
            while (tickCurrent < tickCount)
            {
                //sendToWorld("15 MOVETO 100 " + (300+tickCurrent).ToString()); // debug string
                //cout("sent to world '15 MOVETO 100 " + (300 + tickCurrent).ToString()+"'"); // debug
                receiveByWorld();
                string state = receiveByWorld();
                
                if (state == "EXIT")
                {
                    cout("gameWorld sent me 'EXIT'");
                    return false;
                }

                receiveByBotServer();

                sendToBotServer(Vals.NETWORK_COMMAND_TICK);
                receiveByBotServer();


                sendToBotServer(state);
                //receiveByBotServer();


                string ans = receiveByBotServer();

                sendToWorld(ans);

                tickCurrent++;
                cout("tick : " + tickCurrent.ToString());
            }
            receiveByWorld();
            sendToWorld(Vals.NETWORK_COMMAND_END);
            receiveByWorld();
            sendToBotServer(Vals.NETWORK_COMMAND_END);
            return true;
        }


        /// <summary>
        /// PreInitialisation before call loadconfig, for case when config not cpntaints something of vars
        /// </summary>
        /// <returns>bool</returns>
        static bool initDefault()
        {
            botServerConnected = false;
            worldConnected = false;
            configFileName = "SynchroCoreConfig.ini";
            logFileName = "SynchroCoreLog.log";
            botsInitializationString = "";
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
