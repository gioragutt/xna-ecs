using System.Diagnostics;
using ECS;

namespace XnaTry
{
#if WINDOWS || XBOX
    static class Program
    {
        static void TestECS()
        {
            var manager = new EntityManager();
            var entity1 = manager.CreateEntity();
            Debug.WriteLine("Entity GUID is " + entity1.Id);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            TestECS();
            using (XnaTryGame game = new XnaTryGame())
            {
                game.Run();
            }
        }
    }
#endif
}

