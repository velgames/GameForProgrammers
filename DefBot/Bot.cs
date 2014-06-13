using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Definitions;
using GameWorld;
using GameWorld.GameTypes;
using GameWorld.GameObjects;


namespace DefBot
{
    public class Bot
    {
        public int VSID; // Bot ID

        protected string command = Vals.BOT_ACTION_COMMAND_SLEEP;

        public string name = Vals.BOT_DEFAULT_NAME;

        public bool diedWhileExecute = false;

        public virtual void move(Arena arena)
        {
        }

        public void goTo(int coordinate_x, int coordinate_y)
        {
            command = Vals.BOT_ACTION_COMMAND_MOVETO + " " + coordinate_x.ToString() + " " + coordinate_y.ToString();
            return;
        }

        public string Answer(string key,Arena ar)
        {
            if (key == Vals.KEY_TO_RUN_BOT)
            {
                command = Vals.BOT_ACTION_COMMAND_SLEEP;

                try
                {
                    move(ar);
                }
                catch (Exception ex)
                {
                    command = Vals.BOT_ACTION_COMMAND_DIE;
                    return Vals.SYSTEM_BOT_ERROR_WHILE_EXECUTE;
                }
                return command;
            }
            return Vals.SECURITY_ERROR_BOT_RUN;
        }
        
    }

    public class Arena
    {
        public GamePoint arenaSize;

        public List<ArenaPlayer> Players;

        public ArenaPlayer Me;

        public Arena()
        {
            Players = new List<ArenaPlayer>();

            arenaSize = new GamePoint(1, 1);
        }
    }


    public class ArenaPlayer
    {
        GamePoint position;
        GameDPoint speed;
        int radius;

        public ArenaPlayer(GameDPoint Speed, GamePoint Position, int Radius)
        {
            speed = Speed;
            position = Position;
            radius = Radius;
        }
    }
}
