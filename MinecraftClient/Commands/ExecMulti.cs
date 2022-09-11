﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MinecraftClient.Commands
{
    class ExecMulti : Command
    {
        public override string CmdName { get { return "execmulti"; } }
        public override string CmdUsage { get { return "execmulti <command 1> -> <command2> -> <command 3> -> ..."; } }
        public override string CmdDesc { get { return "cmd.execmulti.desc"; } }

        public override string Run(McClient handler, string command, Dictionary<string, object> localVars)
        {
            if (hasArg(command))
            {
                string commandsString = getArg(command);
                IEnumerable<string> commands = commandsString.Split("->")
                    .ToList()
                    .ConvertAll(command => command.Trim())
                    .FindAll(command => !string.IsNullOrEmpty(command) || command.StartsWith("execmulti", StringComparison.OrdinalIgnoreCase));

                foreach (string cmd in commands)
                {
                    string output = "";
                    handler.PerformInternalCommand(cmd, ref output);

                    string log = Translations.TryGet(
                        "cmd.execmulti.executed", cmd,
                        string.IsNullOrEmpty(output) ? Translations.TryGet("cmd.execmulti.no_result") : Translations.TryGet("cmd.execmulti.result", output));

                    if (output.Contains("unknown command", StringComparison.OrdinalIgnoreCase))
                        handler.Log.Error(log);
                    else handler.Log.Info(log);
                }

                return "";
            }

            return GetCmdDescTranslated();
        }
    }
}
