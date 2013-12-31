using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SimpleConfig;
using XmppBot.Common;

namespace XmppBot_Commands
{
    [Export(typeof(IXmppBotPlugin))]
    public class CommandPlugin : IXmppBotPlugin
    {
        private IList<Command> _commands;

        public CommandPlugin()
        {
            var config = Configuration.Load<CommandsConfig>();
            Initialize(config);
        }

        public CommandPlugin(string configPath)
        {
            var config = Configuration.Load<CommandsConfig>(configPath: configPath);
            Initialize(config);
        }

        public string Evaluate(ParsedLine line)
        {
            if(!line.IsCommand)
            {
                return string.Empty;
            }

            if(line.Command.ToLower() == "help")
            {
                return "Here are the commands I know: \n" + _commands.OrderBy(c => c.Value).Aggregate(String.Empty,
                    (s, command) => s + (s.Length > 0 ? "\n" : "") + (String.IsNullOrEmpty(command.Help) ? ("!" + command.ChatCommand) : command.Help));
            }

            foreach(Command command in _commands)
            {
                if(command.ChatCommand.ToLower() == line.Command.ToLower())
                {
                    if(command.Value == "FormatResponse")
                    {
                        return ResponseFormatter.Format(command, line);
                    }

                    if(command.Value == "Shell")
                    {
                        return Shell.Execute(command, line);
                    }
                }
            }

            return null;
        }

        public string Name
        {
            get { return "Commands"; }
        }

        private void Initialize(CommandsConfig config)
        {
            if(!String.IsNullOrEmpty(config.CommandFilePath))
            {
                ReadCommands(config.CommandFilePath);
            }
            else
            {
                _commands = new List<Command>();
            }
        }

        private void ReadCommands(string commandFilePath)
        {
            string commandData = File.ReadAllText(commandFilePath);

            var potentialResponses = JsonConvert.DeserializeObject<List<Command>>(commandData);

            _commands = potentialResponses;
        }
    }
}