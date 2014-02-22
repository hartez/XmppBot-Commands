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
            var standardArgs = "-NoLogo -OutputFormat Text -NonInteractive -WindowStyle Hidden -NoProfile -EncodedCommand ";

            var cmd = ResponseFormatter.Format(command, line);

            cmd = Convert.ToBase64String(Encoding.Unicode.GetBytes(cmd));

            var args = standardArgs + cmd;

            var info = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Arguments = args
                };

            var proc = new Process {StartInfo = info};
            proc.Start();

            var sb = new StringBuilder();

            while (!proc.StandardOutput.EndOfStream)
            {
                var s = proc.StandardOutput.ReadLine();
                sb.Append(s + "\r\n");
            }

            return sb.ToString();
        }
    }
}