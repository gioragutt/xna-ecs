using System;
using System.Threading.Tasks;

namespace WpfServer.Windows
{
    /// <summary>
    /// Implementation of <c>CommandBase</c> that allows for asynchronous operation.
    /// </summary>
    public abstract class AsyncCommandBase : CommandBase
    {
        /// <summary>
        /// When overridden in a derived class, performs operations in the UI thread
        /// before beginning the background operation.
        /// </summary>
        /// <param name="parameter">The parameter passed to the <c>Execute</c> method of the command.</param>
        protected virtual void BeforeExecute(object parameter)
        {
        }

        /// <summary>
        /// When overridden in a derived class, performs operations in a background
        /// thread when the <c>Execute</c> method is invoked.
        /// </summary>
        /// <param name="parameter">The paramter passed to the <c>Execute</c> method of the command.</param>
        protected virtual void OnExecute(object parameter)
        {
        }

        /// <summary>
        /// When overridden in a derived class, performs operations when the
        /// background execution has completed.
        /// </summary>
        /// <param name="parameter">The parameter passed to the <c>Execute</c> method of the command.</param>
        /// <param name="error">The error object that was thrown during the background operation, or null if no error was thrown.</param>
        protected virtual void AfterExecute(object parameter, Exception error)
        {
        }

        /// <summary>
        /// When overridden in a derived class, defines the method that determines whether the command can execute in its
        /// current state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed,
        /// this object can be set to null.
        /// </param>
        /// <returns>True if this command can be executed; otherwise, false.</returns>
        public abstract override bool CanExecute(object parameter);

        /// <summary>
        /// Runs the command method in a background thread.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed,
        /// this object can be set to null.
        /// </param>
        public override void Execute(object parameter)
        {
            Task.Factory.StartNew(() => BeforeExecute(parameter))
                .ContinueWith(task => OnExecute(parameter))
                .ContinueWith(task => AfterExecute(parameter, task.Exception));
        }
    }
}