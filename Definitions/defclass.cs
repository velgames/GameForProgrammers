using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Definitions
{
    public static class Vals
    {
        public const int NetSyncPuase = 100;

        public const string Net_Name_BotServer = "BotServer";
        public const string Net_Name_GameWorld = "world";

        public const string Command_Start = "START";
        public const string Command_Stop = "STOP";
        public const string Command_End = "END";
        public const string Command_Terminate = "TERMINATE";
        public const string Command_Exit = "EXIT";

        // Security
        public const string KEY_TO_RUN_BOT = "1weq83rr5w24jgfh489102fda";
        public const string SECURITY_ERROR_BOT_RUN = "botRunSecurityErrorzxc";

        // Bot statuses
        public const string BOT_DIED = "botDied...Pechalka:(";

        // System Messages
        public const string SYSTEM_BOT_ERROR_WHILE_EXECUTE = "botDiedWhileExecuting";


        // bot Actions Commands
        public const string BOT_ACTION_COMMAND_MOVETO = "MOVETO";
        public const string BOT_ACTION_COMMAND_SLEEP = "ZALIP"; // BOT deside to do nothing and wait next tick

        public const string BOT_ACTION_COMMAND_DIE = "DesideToDie";
    }
}
