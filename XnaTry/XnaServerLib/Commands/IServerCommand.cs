using System.Collections.Generic;
using EMS;
using XnaCommonLib.ECS;

namespace XnaServerLib.Commands
{
    public interface IServerCommand
    {
        bool CanExecute(IList<string> parameters);
        void Execute(IList<GameObject> gameObjects, IList<string> parameters);
    }

    public abstract class BroadcastingServerCommand : EmsClient, IServerCommand
    {
        public abstract bool CanExecute(IList<string> parameters);
        public abstract void Execute(IList<GameObject> gameObjects, IList<string> parameters);
    }
}