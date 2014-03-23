
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Definitions;
namespace DefBot
{
    public class Bot
    {
        protected string status;

        public virtual void move()
        {
        }

        public void goTo(int coordinate_x, int coordinate_y)
        {
            status = Vals.BOT_ACTION_COMMAND_MOVETO + " " + coordinate_x.ToString() + " " + coordinate_y.ToString();
            return;
        }

        protected string Answer(string key)
        {
            if (key == Vals.KEY_TO_RUN_BOT)
            {
                try
                {
                    move();
                }
                catch (Exception ex)
                {
                    return Vals.SYSTEM_BOT_ERROR_WHILE_EXECUTE;
                }
                return status;
            }
            return Vals.SECURITY_ERROR_BOT_RUN;
        }
        
    }
}
