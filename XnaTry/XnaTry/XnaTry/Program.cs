using System.Diagnostics;
using ECS.Managers;

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
            using (var game = new XnaTryGame())
            {
                game.Run();
            }
        }
    }
#endif
}

