using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameWorld.GameObjects;
using GameWorld.GameTypes;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace GameWorld
{
    class main
    {
        //-------CONFIG VARES------
        static string logFileName;
        const string configFileName = "config.ini";
        static int tickCount;
        static int tickCurrent;
        static int worldWidthSize;
        static int worldHeightSize;

        //-------NetWork Config----
        static Socket handler;
        static string IP;
        static int port; //config
        static IPEndPoint ipEndPoint;
        static bool clientConnected;

        //-------------------------

        static Socket netCore;
        static StreamWriter logWriter;

        //-------- Game vars
        static GameState world; //Global state variable describes current game world state

        static void Run()
        {
            logTopWriter();
            tDebug();

            for (tickCurrent = 0; tickCurrent < tickCount; tickCurrent++)
            {
                // talk with Core
                calculatePhysics();
                logStateWriter();
            }

            logBotWriter();
        }

        /// <summary>
        /// Send to network data
        /// </summary>
        /// <returns></returns>
        static bool sendToNetWork(string msg)
        {
            return true;
        }

        static string getFromNetWork()
        {

            return "NULL";
        }

        static void calculatePhysics()
        {

            //move all players

            for (int i = 0; i < world.players.Count; i++)
            {
                if (world.players[i].position.x >= worldWidthSize - world.players[i].radius || world.players[i].position.x <= 0 + world.players[i].radius)
                {
                    world.players[i].currentSpeed.x *= -1;
                }
                world.players[i].move();
            }
        }

        static bool logTopWriter()
        {
            logWriter.WriteLine("[START CONFIG]");
            logWriter.WriteLine("tickCount " + tickCount);
            logWriter.WriteLine("playersCount " + world.players.Count);
            logWriter.WriteLine("playersCount " + world.players.Count);
            logWriter.WriteLine("worldWidthSize " + worldWidthSize);
            logWriter.WriteLine("worldHeightSize " + worldHeightSize);
            logWriter.WriteLine("[END]");
            logWriter.WriteLine("[GAME STATES]");
            return true;
        }

        static bool logBotWriter()
        {
            logWriter.WriteLine("[END]");
            return true;
        }

        static bool logStateWriter()
        {
            logWriter.WriteLine(GameState.GetState(world));

            return true;
        }

        static bool disconnect()
        {

            return true;
        }

        static bool connectToCore()
        {
            try
            {
                // Connect to GameCore
                cout("connecting to SynchroCore...");
                String host = Dns.GetHostName();
                IPAddress ipAddr = Dns.GetHostByName(host).AddressList[0];
                ipEndPoint = new IPEndPoint(ipAddr, port);
                handler = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                handler.Connect(ipEndPoint);
                byte[] msg = Encoding.ASCII.GetBytes("world");
                handler.Send(msg);
                int c = handler.Receive(msg);
                cout(Encoding.ASCII.GetString(msg, 0, c));
            }
            catch (Exception ex)
            {
                cout("Connection filed");
                return false;
            }
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
            if(!File.Exists(configFileName))
            {
                configWriteDefault();
            }
            StreamReader inf = new StreamReader(configFileName);
            while(!inf.EndOfStream)
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

        static void tDebug()
        {
            world.players[0].currentSpeed.x = 10;
            world.players[0].position.x = 100;
            world.players[0].position.y = 100;
        }

        static bool Init()
        {
            clientConnected = false;
            port = 11000;
            int playersCount = 1;       ///////////NET WORK///////////
            int playerRadius = 10;      ///////////NET WORK///////////
            tickCurrent = 0;            ///////////NET WORK///////////
            tickCount = 3000;           ///////////NET WORK///////////
            worldHeightSize = 768;
            worldWidthSize = 1024;

            world = new GameState(playersCount, playerRadius);
            cout("loading config");
            if (!configLoad())
            {
                return false;
                cout("config load filed");
            }

            logWriter = new StreamWriter(logFileName,false);

            if (!connectToCore())
            {
                return false;
            }

            return true;
        }

        static void shutdown()
        {
            logWriter.Close();
            if (clientConnected)
            {
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }
        
        static bool cout(string msg)
        {
            Console.WriteLine(msg);
            // logwrite(msg);
            return true;
        }

        static void Main(string[] args)
        {
            cout("GameWorld starting...");
            cout("Initing...");
            if (!Init())
            {
                return;
            }


            cout("Starting work");
            Run();

            shutdown();
            cout("Compleated");
            cout("waiting 10 secs and exit..");
            Thread.Sleep(10000);
        }
    }
}
