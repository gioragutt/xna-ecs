namespace UtilsLib.Exceptions.Server.Commands
{
    public class NoCommandSuppliedException : ServerCommandsException
    {
        private const string ErrorMessage = "No command name was supplied when attempting to execute a command";
        public override string Message => ErrorMessage;
    }
}