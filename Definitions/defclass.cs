using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Definitions
{
    public static class Vals
    {
        public const int NetSyncPuase = 100;

        public const string NetNameBotServer = "BotServer";
        public const string NetNameGameWorld = "world";

        public const string CommandStart = "START";
        public const string CommandStop = "STOP";
        public const string CommandEnd = "END";
        public const string CommandTerminate = "TERMINATE";
        public const string CommandExit = "EXIT";

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
