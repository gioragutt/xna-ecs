﻿using System.Collections.Generic;
using XnaCommonLib.ECS;

namespace XnaServerLib.Commands
{
    public interface IServerCommand
    {
        bool CanExecute(IList<string> parameters);
        void Execute(IList<GameObject> gameObjects, IList<string> parameters);
    }
}