using System;
using XnaCommonLib.ECS;

namespace XnaServerLib.ECS
{
    public class ServerGameManager : GameManagerBase
    {
        public void Update(TimeSpan delta)
        {
            Update((long)delta.TotalMilliseconds);
        }
    }
}