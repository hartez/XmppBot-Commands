using System;
using System.Linq;
using XmppBot.Common;

namespace XmppBot_Commands
{
    public class Parameter
    {
        public ParameterType ParameterType { get; set; }
        public int Index { get; set; }
        public string Default { get; set; }
        public string Value { get; set; }

        public string Eval(ParsedLine line)
        {
            if (ParameterType == ParameterType.Argument)
            {
                if (line.Args.Count() >= Index + 1)
                {
                    return line.Args[Index];
                }

                return Default;
            }

            if (ParameterType == ParameterType.AllArguments)
            {
                if (line.Args.Any())
                {
                    return line.Args.Aggregate(String.Empty, (s, s1) => s + (s.Length > 0 ? " " : "") + s1);
                }

                return Default;
            }

            switch (ParameterType)
            {
                case ParameterType.User:
                    return line.User;
                case ParameterType.Command:
                    return line.Command;
                case ParameterType.Raw:
                    return line.Raw;
                case ParameterType.Predefined:
                    return Value;
                case ParameterType.Argument:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
        }
    }
}