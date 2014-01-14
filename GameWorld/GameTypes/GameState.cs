using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameWorld.GameObjects;

namespace GameWorld.GameTypes
{
    public class GameState
    {
        List<Player> Players;

        public GameState(string str, int playersCount, int playerRadius,string[] names)
        {
            Players = new List<Player>();
            string[] strings = str.Split(';');
            string[] players = strings[0].Split(' ');
            int c = 0;
            for (int i = 0; i < playersCount; i++)
            {
                int id = Convert.ToInt32(players[c]);
                int x = Convert.ToInt32(players[c + 1]);
                int y = Convert.ToInt32(players[c + 2]);
                c += 3;
                Players.Add(new Player(new GamePoint(x, y), playerRadius, names[i], id));
            }


        }
    }
}
