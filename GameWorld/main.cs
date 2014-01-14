using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameWorld.GameObjects;
using GameWorld.GameTypes;
using System.IO;
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

        //-------NetWork Config----
        static int playersCount;


        //-------------------------

        static List<Player> players;
        static Socket netCore;
        static StreamWriter logWriter;

        static void Run()
        {
            logWriteTop();


            for (tickCurrent = 0; tickCurrent < tickCount; tickCurrent++)
            {
                // talk with Core
                CalculatePhysics();
                LogWriter();
            }
        }

        /// <summary>
        /// Send to network data
        /// </summary>
        /// <returns></returns>
        static bool SendToNetWork(string msg)
        {
            return true;
        }

        static string GetFromNetWork()
        {

            return "NULL";
        }

        static void CalculatePhysics()
        {

            //move all players
            for (int i = 0; i < players.Count; i++)
            {
                players[i].move();
            }

        }

        static bool logWriteTop()
        {
            logWriter.WriteLine("tickCount " + tickCount);
            
            logWriter.WriteLine("playersCount " + playersCount);
            for (int i = 0; i < playersCount; i++)
            {
                logWriter.WriteLine(players[i].name); //name
                logWriter.WriteLine(players[i].position.x + " " + players[i].position.y); // position
                logWriter.WriteLine(players[i].radius); // radius
                logWriter.WriteLine(players[i].ID); // ID
            }
            return true;
        }

        static bool LogWriter()
        {
            Console.WriteLine("tick = " + (tickCurrent+1));
            logWriter.WriteLine("tick = " + (tickCurrent + 1));
            logWriter.Flush();
            //Thread.Sleep(1);

            return true;
        }

        static bool Disconnect()
        {

            return true;
        }

        static bool ConnectToCore()
        {
            // Connect to GameCore

            return true;
        }

        static bool ConfigWriteDefault()
        {
            StreamWriter off = new StreamWriter(configFileName);
            off.Write("#Its automaticaly generated config file, edit it if you need\r\nlogFileName gameLog.txt\r\n");
            off.Close();
            return true;
        }

        static bool ConfigLoad()
        {
            if(!File.Exists(configFileName))
            {
                ConfigWriteDefault();
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

        static bool Init()
        {
            playersCount = 1;       ///////////NET WORK///////////

            players = new List<Player>();
            for (int i = 0; i < playersCount; i++)
            {
                players.Add(new Player(new GamePoint(50, 50), 10, "Man", i));
            }
            tickCurrent = 0;
            tickCount = 3000;



            if (!ConfigLoad())
            {
                return false;
            }

            logWriter = new StreamWriter(logFileName,false);

            return true;
        }

        static void closer()
        {
            logWriter.Close();
        }

        static void Main(string[] args)
        {
            //Start up point
            //Init

            if (!Init())
            {
                return;
            }

            Run();

            Console.WriteLine("Compleated");
            Thread.Sleep(1000);
        }
    }
}
