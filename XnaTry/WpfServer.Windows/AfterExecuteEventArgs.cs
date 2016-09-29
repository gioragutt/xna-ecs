using System;
namespace WpfServer.Windows
{
    public class AfterExecuteEventArgs : EventArgs
    {
        public object Parameter { get; set; }
        public Exception Error { get; set; }
    }
}
