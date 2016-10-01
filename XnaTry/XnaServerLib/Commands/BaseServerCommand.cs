using System;
using System.Collections.Generic;
using EMS;
using UtilsLib.Exceptions.Server.Commands;
using XnaCommonLib.ECS;

namespace XnaServerLib.Commands
{
    public abstract class BaseServerCommand : EmsClient, IServerCommand
    {
        private readonly IList<Exception> executionExceptions;

        protected BaseServerCommand()
        {
            executionExceptions = new List<Exception>();
        }

        protected void AddExecutionException(CommandExecutionException executionException)
        {
            executionExceptions.Add(executionException);
        }

        protected void AddExecutionException(string message)
        {
            AddExecutionException(new CommandExecutionException(message));
        }

        public abstract bool CanExecute(IList<string> parameters);
        public virtual void Execute(IList<GameObject> gameObjects, IList<string> parameters)
        {
            if (executionExceptions.Count > 0)
                throw new AggregateException(executionExceptions);
        }
    }
}