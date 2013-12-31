namespace XmppBot_Commands
{
    public class Command
    {
        public string Value { get; set; }
        public Parameter[] Parameters { get; set; }
        public string ChatCommand { get; set; }
        public string Help { get; set; }
    }
}