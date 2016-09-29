namespace UtilsLib.Exceptions.Server.Commands
{
    public class NoSuchCommandException : ServerCommandsException
    {
        private readonly string suppliedCommandName;
        private const string ErrorMessageFormat = "Command {0} does not exist";

        public NoSuchCommandException(string suppliedCommandName)
        {
            this.suppliedCommandName = suppliedCommandName;
        }

        public override string Message => string.Format(ErrorMessageFormat, suppliedCommandName);
    }
}