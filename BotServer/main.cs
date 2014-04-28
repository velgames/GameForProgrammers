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
        
        static List<Bot> Bots;
        static List<string> dllBotPaths;

        //----------------------------
        // Net Init vars ===================
        static int BotCount;



        //----------------------------------
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
                sendMsg(Vals.Net_Name_BotServer);
                cout(receiveMsg());
            }
            catch (Exception ex)
            {
                cout("Connection filed");
                return false;
            }
            return true;
        }

        static bool disconnect()
        {

            return true;
        }

        static bool Init()
        {
            dllBotPaths = new List<string>();
            Bots = new List<Bot>();
            BotCount = 1;
            dllBotPaths.Add("MyBot.dll");

            botLoad();

            connectToCore();
            
            return true;
        }

        static void readBotPaths()
        {
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
        }


        /// <summary>
        /// Add bot dll files and create objects of user classes
        /// </summary>
        /// <returns> true or false depend on successful operation</returns>
        static bool botLoad()
        {
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

            return true;
        }


        static void Main(string[] args)
        {
            Init();
            Console.ReadLine();
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
