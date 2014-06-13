using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Definitions
{
    public static class Vals
    {
        public const int VALUE_NETWORK_SYNC_PAUSETIME = 100;

        public const string NETWORK_NAME_BOTSERVER = "BotServer";
        public const string NETWORK_NAME_GAMEWORLD = "world";

        public const string NETWORK_COMMAND_OK = "OK";
        public const string NETWORK_COMMAND_INIT = "initialize_command";
        public const string NETWORK_COMMAND_START = "START";
        public const string NETWORK_COMMAND_STOP = "STOP";
        public const string NETWORK_COMMAND_END = "END";
        public const string NETWORK_COMMAND_TICK = "TICK";
        public const string NETWORK_COMMAND_TERMINATE = "TERMINATE";
        public const string NETWORK_COMMAND_EXIT = "EXIT";

        // Security
        public const string KEY_TO_RUN_BOT = "1weq83rr5w24jgfh489102fda";
        public const string SECURITY_ERROR_BOT_RUN = "botRunSecurityErrorzxc";

        // Bot statuses
        public const string BOT_DEFAULT_NAME = "IAmBot";

        public const string BOT_DIED = "botDied...Pechalka:(";

        // System Messages
        public const string SYSTEM_BOT_ERROR_WHILE_EXECUTE = "botDiedWhileExecuting";


        // bot Actions Commands
        public const string BOT_ACTION_COMMAND_MOVETO = "MOVETO";
        public const string BOT_ACTION_COMMAND_SLEEP = "ZALIP"; // BOT deside to do nothing and wait next tick

        public const string BOT_ACTION_COMMAND_DIE = "DesideToDie";
    }


}
