namespace XmppBot_Commands
{
    public class Command
    {
        public string FormatString { get; set; }
        public Parameter[] Parameters { get; set; }
        public string Value { get; set; }
        public string Help { get; set; }
    }
}