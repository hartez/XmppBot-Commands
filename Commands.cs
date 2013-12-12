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
                    (s, command) => s + (s.Length > 0 ? "\n" : "") + (String.IsNullOrEmpty(command.Help) ? ("!" + command.Value) : command.Help));
            }

            foreach(Command command in _commands)
            {
                if(command.Value.ToLower() == line.Command.ToLower())
                {
                    return String.Format(command.FormatString,
                        command.Parameters.Select(p => EvalParam(p, line)).ToArray());
                }
            }

            return null;
        }

        public string Name
        {
            get { return "Commands"; }
        }

        private object EvalParam(Parameter parameter, ParsedLine line)
        {
            if(parameter.ParameterType == ParameterType.Argument)
            {
                if(line.Args.Count() >= parameter.Index + 1)
                {
                    return line.Args[parameter.Index];
                }

                return parameter.Default;
            }

            if(parameter.ParameterType == ParameterType.AllArguments)
            {
                if(line.Args.Any())
                {
                    return line.Args.Aggregate(String.Empty, (s, s1) => s + (s.Length > 0 ? " " : "") + s1);
                }

                return parameter.Default;
            }

            switch(parameter.ParameterType)
            {
                case ParameterType.User:
                    return line.User;
                    break;
                case ParameterType.Command:
                    return line.Command;
                    break;
                case ParameterType.Raw:
                    return line.Raw;
                    break;
                case ParameterType.Argument:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
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