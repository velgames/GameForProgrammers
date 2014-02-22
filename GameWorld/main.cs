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
        //-------CONFIG VARS------
        static string logFileName;
        const string configFileName = "config.ini";
        static int tickCount;
        static int tickCurrent;
        static int worldWidthSize;
        static int worldHeightSize;
        static int playerRadius;
        static int playersCount;

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

        static bool Run()
        {
            logTopWriter();
            tDebug();
            if (receiveMsg() != "START")
            {
                sendMsg("EXIT");
                cout("'START' is not received");
                return false;
            }
            bool needWork = true;
            while (needWork)
            {
                string msg = receiveMsg();
                if (msg == "END")
                {
                    break;
                }
                if (!applyAction(msg))
                {
                    //somthing happends, need write to error log
                    sendMsg("EXIT");
                    cout("applyAction fail '"+ msg +"'");
                    return false;
                }
                
                physics();
                logStateWriter();
                sendMsg("NEXT");
            }

            logBotWriter();
            return true;
        }

        static bool applyAction(string command)
        {
            string[] commands = command.Split(';');
            for (int i = 0; i < commands.Length; i++)
            {
                string[] str = commands[0].Split(' ');
                
                switch (str[1]) // analyse command
                {
                    case "MOVETO":
                        int id = Convert.ToInt32(str[0]); // get id of player who is do action
                        double x = Convert.ToInt32(str[2]);
                        double y = Convert.ToInt32(str[3]);
                        int k = 0;
                        // search in array for player with received id
                        for (int j = 0; j < playersCount; j++)
                        {
                            if (world.players[j].ID == id)
                            {
                                k = j;
                                break;
                            }
                        }

                        if (world.players[k].position.x <= x)
                        {
                            world.players[k].acceleration.x = Math.Cos(Math.Atan(Math.Abs(world.players[k].position.y - y) / Math.Abs(world.players[k].position.x - x))) * Physics.playerAccelerate;
                        }
                        else
                        {
                            world.players[k].acceleration.x = Math.Cos(Math.Atan(Math.Abs(world.players[k].position.y - y) / Math.Abs(world.players[k].position.x - x)) + Math.PI) * Physics.playerAccelerate;
                        }
                        if (world.players[k].position.y <= y)
                        {
                            world.players[k].acceleration.y = Math.Sin(Math.Atan(Math.Abs(world.players[k].position.y - y) / Math.Abs(world.players[k].position.x - x))) * Physics.playerAccelerate;
                        }
                        else
                        {
                            world.players[k].acceleration.y = Math.Sin(Math.Atan(Math.Abs(world.players[k].position.y - y) / Math.Abs(world.players[k].position.x - x)) + Math.PI) * Physics.playerAccelerate;
                        }

                        cout("player" + world.players[k].name + "moving to ("+x.ToString() + ";" + y.ToString()+")");
                        break;

                    default:
                        
                        return false;
                        
                }
            }

            return true;
        }

        static void physics()
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
                sendMsg ("world");
                cout(receiveMsg());
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
            playersCount = 1;       ///////////NET WORK///////////
            playerRadius = 10;      
            tickCurrent = 0;            
            tickCount = 3000;           ///////////NET WORK///////////
            worldHeightSize = 768;
            worldWidthSize = 1024;

            world = new GameState(0, playerRadius); //debug
            cout("loading config");
            if (!configLoad())
            {
                return false;
                cout("config load filed");
            }

            if (!connectToCore())
            {
                return false;
            }

            if (!initNetwork())
            {
                return false;
            }
            logWriter = new StreamWriter(logFileName,false);
            return true;
        }


        /// <summary>
        /// Initing network config and init objects
        /// </summary>
        static bool initNetwork()
        {
            try
            {
                //Init network vars
                playersCount = Convert.ToInt32(receiveMsg());
                cout("playersCount = " + playersCount.ToString());
                tickCount = Convert.ToInt32(receiveMsg());
                cout("tickCount = " + tickCount.ToString());

                //Init players
                for (int i = 0; i < playersCount; i++)
                {
                    string name = receiveMsg();
                    int id = Convert.ToInt32(receiveMsg());
                    int x = Convert.ToInt32(receiveMsg());
                    int y = Convert.ToInt32(receiveMsg());
                    world.players.Add(new Player(new GamePoint(x, y), playerRadius, name, id));
                    cout("player '" + name + "' with id='" + id.ToString() + "' added ");
                }
                sendMsg("OK");
            }
            catch (Exception ex)
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
            if (!Run())
            {
                cout("run fail");
            }
            else
            {
                cout("Compleated");
            }

            shutdown();

            cout("press enter to exit..");
            Console.ReadLine();
        }


        /// <summary>
        /// Send to network data
        /// </summary>
        /// <returns></returns>
        static bool sendMsg(string msg)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(msg);
            handler.Send(bytes);
            Thread.Sleep(100);
            return true;
        }

        static string receiveMsg()
        {
            byte[] bytes = new byte[1024];
            int c = handler.Receive(bytes);
            return Encoding.ASCII.GetString(bytes, 0, c);
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
    }
}
