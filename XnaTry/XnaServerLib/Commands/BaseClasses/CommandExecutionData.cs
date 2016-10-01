using System.Collections.Generic;

namespace XnaServerLib.Commands.BaseClasses
{
    public class CommandExecutionData
    {
        public IServerCommand Command { get; set; }
        public IList<string> Parameters { get; set; }
    }
}