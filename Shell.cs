using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using XmppBot.Common;

namespace XmppBot_Commands
{
    public static class Shell
    {
        public static string Execute(Command command, ParsedLine line)
        {
            var cmd = command.Parameters.First().Eval(line);

            var standardArgs = "-NoLogo -OutputFormat Text -NonInteractive -WindowStyle Hidden -NoProfile ";

            var args = standardArgs +
                       command.Parameters.Aggregate(String.Empty, (s, p) => s + (s.Length > 0 ? " " : "") + p.Eval(line));

            var info = new ProcessStartInfo()
                {
                    FileName = "powershell.exe",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Arguments = args
                };

            var proc = new Process() {StartInfo = info};
            proc.Start();

            StringBuilder sb = new StringBuilder();

            while (!proc.StandardOutput.EndOfStream)
            {
                var s = proc.StandardOutput.ReadLine();
                sb.Append(s);
            }

            return sb.ToString();
        }
    }
}