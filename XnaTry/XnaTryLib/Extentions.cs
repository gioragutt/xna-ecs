using Microsoft.Xna.Framework;
using System;

namespace XnaTryLib
{
    public static class Extentions
    {
        /// <summary>
        /// Shortcut for adding a service to Game.Services
        /// </summary>
        /// <typeparam name="TService">Type of service to add</typeparam>
        /// <param name="game">Game to add the service to</param>
        /// <param name="service">The service to add</param>
        public static void AddService<TService>(this Game game, TService service)
        {
            if (game == null) throw new ArgumentNullException("game");
            if (service == null) throw new ArgumentNullException("game");
            game.Services.AddService(typeof(TService), service);
        }

        /// <summary>
        /// Shortcut for retrieving a service from Game.Services
        /// </summary>
        /// <typeparam name="TService">Type of service to get</typeparam>
        /// <param name="game">Game to get the service from</param>
        /// <returns>A service of type <typeparamref name="TService"/></returns>
        public static TService GetService<TService>(this Game game)
        {
            if (game == null) throw new ArgumentNullException("game");
            return (TService)game.Services.GetService(typeof(TService));
        }
    }
}
