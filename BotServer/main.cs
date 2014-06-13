using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Reflection;

using DefBot;
using Definitions;
using GameWorld.GameObjects;
using GameWorld.GameTypes;
using GameWorld;

namespace BotServer
{
    class BotServer
    {

        // Net config vars==================
        static Socket handler;
        static string IP;
        static int port; //config
        static IPEndPoint ipEndPoint;
        static bool clientConnected;
        
        //----------------------------
        // Config and vars
        const string configFileName = "botServerConfig.conf";
        const string BotPathsFilePath = "bots.ini";
        static StreamWriter logWriter;


        static List<Bot> Bots;
        static List<string> dllBotPaths;
        static string botsInitString;

        //----------------------------
        // Net Init vars ===================
        static int BotCount;



        //----------------------------------

        static bool run()
        {
            cout("Runed");
            
            bool stop = false;
            while (!stop)
            {
                sendMsg(Vals.NETWORK_COMMAND_OK);
                cout("Wait for command");
                string command = receiveMsg();
                sendMsg(Vals.NETWORK_COMMAND_OK);
                //cout("command : " + command);
                switch (command)
                {
                    case Vals.NETWORK_COMMAND_END:
                        stop = true;
                        break;

                    case Vals.NETWORK_COMMAND_TICK:
                        cout("Tick");
                        string str = receiveMsg();
                        
                        string answer = tick(str);
                        sendMsg(answer);

                        break;

                    default:
                        cout("Wrong command " + command);
                        break;
                }
            
            }

            return true;
        }

        static string tick(string state)
        {
            string answer = "";
            for (int i = 0; i < Bots.Count; i++)
            {
                Arena arena = createArena(state, Bots[i].VSID);

                string str = Bots[i].Answer(Vals.KEY_TO_RUN_BOT,arena);
                if (str != Vals.BOT_ACTION_COMMAND_DIE && str != Vals.SYSTEM_BOT_ERROR_WHILE_EXECUTE && str != Vals.SECURITY_ERROR_BOT_RUN)
                {
                    answer += Bots[i].VSID.ToString() + " " + str + " ; ";
                }
                else
                {
                    Bots[i].diedWhileExecute = true;
                }
            }
            int a = 0;
            return answer;
        }


        static Arena createArena(string str,int ID)
        {
            Arena cArena = new Arena();
            cArena.arenaSize = new GamePoint(1024, 768);
            string[] space = str.Split(';');
            string[] bots = space[0].Split(' ');
            int t = 0;
            for (int i = 0; i < Bots.Count; i++)
            {
                string name = bots[t];
                int VSID = Convert.ToInt32( bots[t + 1]);
                GamePoint p = new GamePoint(Convert.ToInt32( bots[t + 2]),Convert.ToInt32( bots[t + 3]));
                GameDPoint s = new GameDPoint(Convert.ToDouble(bots[t + 4]),Convert.ToDouble(bots[t + 5]));
                int r = Convert.ToInt32( bots[t + 6]);

                if (VSID == Bots[i].VSID)
                {
                    cArena.Me = new ArenaPlayer(s, p, r);
                }
                else
                {
                    cArena.Players.Add(new ArenaPlayer(s, p, r));
                }
                t += 7;
            }
           
            return cArena; 
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
                sendMsg(Vals.NETWORK_NAME_BOTSERVER);
                cout(receiveMsg());
            }
            catch (Exception ex)
            {
                cout("Connection filed");
                return false;
            }
            cout("Connected");
            return true;
        }

        static bool disconnect()
        {
            return true;
        }

        static bool Init()
        {
            port = 11000;
            dllBotPaths = new List<string>();
            Bots = new List<Bot>();
            BotCount = 1;
            botsInitString = "";
            dllBotPaths.Add("MyBot.dll"); // test debug
            logWriter = new StreamWriter("botserver.log");


            botsLoad();
            botsInit();


            if (!connectToCore())
            {
                return false;
            }

            if (!netWorkInit())
            {
                cout("network init failed");
                return false;
            }
            
            return true;
        }

        static bool netWorkInit()
        {
            cout("Network init started");
            if (receiveMsg() != Vals.NETWORK_COMMAND_INIT)
            {
                cout("Init command not received or received error data");
            }
            else
            {
                cout("network init accepted");
            }

            sendMsg(botsInitString);
            //cout("bots sent");
            cout(receiveMsg());
            cout("network init [ok]");
            return true;
        }

        static bool botsInit()
        {
            cout("bots init");
            string str = "";
            /*
            str = [name] [id]
            */
            for (int i = 0; i < Bots.Count; i++)
            {
                if(Bots[i].name == Vals.BOT_DEFAULT_NAME)
                {
                    Bots[i].name = Vals.BOT_DEFAULT_NAME  + (i+1).ToString();
                }
                str += Bots[i].name + " ";
                Bots[i].VSID = (i + 1) * 10 / 3 * 1234; // generate id
                str += Bots[i].VSID.ToString(); 
                str += " ;";
            }
            botsInitString = str;
            cout("bots init [ok]");
            return true;
        }

        static void readBotPaths()
        {
            cout("read paths");
            if (!File.Exists(BotPathsFilePath))
            {
                StreamWriter off = new StreamWriter(BotPathsFilePath);
                off.WriteLine("#there bot.dll file names you want to play");
            }
            StreamReader str = new StreamReader(BotPathsFilePath);
            while (!str.EndOfStream)
            {
                string path = str.ReadLine();
                if (path.Length > 4)
                {
                    if (path[0] != '#')
                    {
                        dllBotPaths.Add(path);
                    }
                }
            }
            str.Close();
            cout("read paths [ok]");
        }


        /// <summary>
        /// Add bot dll files and create objects of user classes
        /// </summary>
        /// <returns> true or false depend on successful operation</returns>
        static bool botsLoad()
        {
            cout("load bots");
            for (int i = 0; i < dllBotPaths.Count; i++)
            {
                try
                {
                    Assembly asemb = Assembly.LoadFrom(dllBotPaths[i]);
                    var ClassTypes = from t in asemb.GetTypes() where t.IsClass && (t.GetMember("Bot") != null) select t;
                    foreach (Type t in ClassTypes)
                    {
                        Bot tBot = (Bot)asemb.CreateInstance(t.FullName, true);
                        //tBot.move();
                        Bots.Add(tBot);
                    }

                }
                catch (Exception ex)
                {
                    if (i < dllBotPaths.Count)
                    {
                        cout("filed load dll : " + dllBotPaths[i]);
                    }
                    else
                    {
                        cout("filed load dll : out of range");
                    }
                    return false;
                }
            }
            cout("bots load [ok]");
            return true;
        }

        static void Main(string[] args)
        {
            if (!Init())
            {
                cout("Initialiazing failed");

            }
            else
            {
                cout("initialized successful");
            }
            run();
            Console.WriteLine("Press enter to exit");
           // Console.ReadLine();
        }

        static bool configWriteDefault()
        {
            StreamWriter off = new StreamWriter(configFileName);
            off.Write("#Its automaticaly generated config file, edit it if you need\r\nBotDllPath MyBot.dll\r\n");
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

                    if (str[0] == "BotDllPath")
                    {
                        dllBotPaths.Add(str[1]);
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

        static bool cout(string msg)
        {
            Console.WriteLine(msg);
            logWriter.WriteLine(msg);
            logWriter.Flush();
            return true;
        }
    }
}




//-------------------------------------------------------------------\\\\\---------------------/////--
//--------------------------------------------------------------------\\\\\-------------------/////---
//---------------------------------------------------------------------\\\\\-----------------/////----
//----------------------------------------------------------------------\\\\\---------------/////-----
//-----------------------------------------------------------------------\\\\\-------------/////------
//------------------------------------------------------------------------\\\\\-----------/////-------
//-------------------------------------------------------------------------\\\\\---------/////--------
//--------------------------------------------------------------------------\\\\\-------/////---------
//---------------------------------------------------------------------------\\\\\-----/////----------
//----------------------------------------------------------------------------\\\\\___/////-----------
//-----------------------------------------------------------------------------\_________/------------
//------------------------------------------------------------------------------\_______/-------------
