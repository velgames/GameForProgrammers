
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Definitions;
namespace DefBot
{
    public class Bot
    {
        protected string command = Vals.BOT_ACTION_COMMAND_SLEEP;

        public virtual void move()
        {
        }

        public void goTo(int coordinate_x, int coordinate_y)
        {
            command = Vals.BOT_ACTION_COMMAND_MOVETO + " " + coordinate_x.ToString() + " " + coordinate_y.ToString();
            return;
        }

        public string Answer(string key)
        {
            if (key == Vals.KEY_TO_RUN_BOT)
            {
                command = Vals.BOT_ACTION_COMMAND_SLEEP;

                try
                {
                    move();
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
}
