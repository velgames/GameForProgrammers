using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;


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

        // Net Init vars ===================
        static int playerCount;



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
                sendMsg("BotServer");
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


        static void Main(string[] args)
        {

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
