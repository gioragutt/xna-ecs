namespace XnaTry
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var arguments = new ConnectionArguments(args);
            using (var game = new XnaTryGame(arguments))
            {
                game.Run();
            }
        }
    }
#endif
}

