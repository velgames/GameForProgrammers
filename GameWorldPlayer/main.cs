using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameWorld.GameObjects;
using GameWorld.GameTypes;

namespace GameWorldPlayer
{
    class mainclass
    {
        static List<Player> Players;


        static void Main()
        {
            string[] names = { "b1", "b2", "b3", "b4" };

            int playerRadius = 10;
            int playerscount = 4;

            GameState gs = new GameState("0 0 0 1 10 10 2 20 20 3 30 30 ;", playerscount, playerRadius, names);

            int a = 0;
        }
    }
}
