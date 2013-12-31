using System;
using System.Linq;
using XmppBot.Common;

namespace XmppBot_Commands
{
    public static class ResponseFormatter
    {
        public static string Format(Command command, ParsedLine line)
        {
            var formatString = command.Parameters.First().Eval(line);

            return String.Format(formatString,
                command.Parameters.Skip(1).Select(p => p.Eval(line)).ToArray());
        }
    }
}