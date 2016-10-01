using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using WpfServer.Commands;
using WpfServer.Windows;
using XnaServerLib.Commands.BaseClasses;

namespace WpfServer.ViewModels
{
    public class ServerCommandLineViewModel : ViewModelBase
    {
        public AsyncCommandBase ExecuteServerCommandCommand { get; set; }
        public Dispatcher Dispatcher { get; }

        #region CommandHistory

        private ObservableCollection<string> commandHistory;

        public ObservableCollection<string> CommandHistory
        {
            get
            {
                return commandHistory;
            }
            set
            {
                commandHistory = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region ErrorText

        private string errorText;

        public string ErrorText
        {
            get
            {
                return errorText;
            }
            set
            {
                errorText = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region ErrorToolTip

        private string errorTooltip;

        public string ErrorToolTip
        {
            get
            {
                return errorTooltip;
            }
            set
            {
                errorTooltip = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private const string ErrorFormat = "({0}) {1}";

        public ServerCommandLineViewModel(ServerCommandsService commandsService, Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;
            CommandHistory = new ObservableCollection<string>();
            ExecuteServerCommandCommand = new ExecuteServerCommandCommand(commandsService);
            ExecuteServerCommandCommand.BeforeCommandExecute += AddCommandToHistory;
            ExecuteServerCommandCommand.AfterCommandExecute += GetCommandErrors;
        }

        private void AddCommandToHistory(object sender, object parameters)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var commandString = string.Join(" ", (IList<string>) parameters);
                if (!CommandHistory.Contains(commandString))
                    CommandHistory.Add(commandString);
            }));
        }

        private void GetCommandErrors(object sender, AfterExecuteEventArgs afterExecuteEventArgs)
        {
            ErrorText = ErrorToolTip = string.Empty;
            if (afterExecuteEventArgs.Error == null)
                return;

            var errors = GetAllInnerExceptions(afterExecuteEventArgs.Error.InnerExceptions);
            if (errors.Count == 0)
                return;

            MakeText(errors);
            MakeTooltip(errors);
        }

        private static IList<Exception> GetAllInnerExceptions(IEnumerable<Exception> errors)
        {
            var allExceptions = new List<Exception>();
            foreach (var error in errors)
            {
                var exception = error as AggregateException;
                if (exception != null)
                    allExceptions.AddRange(GetAllInnerExceptions(exception.InnerExceptions));
                else
                    allExceptions.Add(error);
            }
            return allExceptions;
        }

        private void MakeText(IList<Exception> errors)
        {
            ErrorText = string.Format(ErrorFormat, errors.Count, errors[0].Message);
        }

        private void MakeTooltip(IList<Exception> errors)
        {
            ErrorToolTip = string.Format(ErrorFormat, 1, errors[0].Message);
            for (var i = 1; i < errors.Count; ++i)
                ErrorToolTip += "\n" + string.Format(ErrorFormat, i + 1, errors[i].Message);
        }
    }
}
