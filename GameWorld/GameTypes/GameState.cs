using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameWorld.GameObjects;

namespace GameWorld.GameTypes
{
    public class GameState
    {
        public List<Player> players;

        public GameState(int playersCount,int playerRadius)
        {
            players = new List<Player>();
            for(int i=0;i<playersCount;i++)
            {
                players.Add(new Player(new GamePoint(0,0),playerRadius,"Player"+i.ToString(),i));
            }
        }

        public GameState(string str, int playersCount)
        {
            players = new List<Player>();
            string[] strings = str.Split(';');
            string[] strplayers = strings[0].Split(' ');
            

            // Loading players
            int c = 0;
            
            for (int i = 0; i < playersCount; i++)
            {
                string name = strplayers[c];
                int id = Convert.ToInt32(strplayers[c+1]);
                int x = Convert.ToInt32(strplayers[c + 2]);
                int y = Convert.ToInt32(strplayers[c + 3]);
                int playerRadius = Convert.ToInt32(strplayers[c + 4]);
                c += 5;
                players.Add(new Player(new GamePoint(x, y), playerRadius, name, id));
            }
        }

        public static string GetState(GameState state)
        {
            string str = "";
            string sc = " ";
            int playersCount = state.players.Count;

            for (int i = 0; i < playersCount; i++)
            {
                str += state.players[i].name + sc;
                str += state.players[i].ID + sc;
                str += state.players[i].position.x.ToString() + sc;
                str += state.players[i].position.y.ToString() + sc;
                str += state.players[i].radius + sc;
            }
            str += ";";


            return str;
        }
    }
}
