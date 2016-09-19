using System.Collections.ObjectModel;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using WpfServer.Commands;
using WpfServer.Windows;
using XnaServerLib;

namespace WpfServer.ViewModels
{
    public class ServerViewModel : ViewModelBase
    {
        public Server Server { get; }

        #region Commands

        public ICommand StartListeningCommand { get; set; }
        public ICommand StopListeningCommand { get; set; }

        #endregion

        #region Properties

        #region ServerStatus

        private string serverStatus;
        public string ServerStatus
        {
            get
            {
                return serverStatus;
            }
            set
            {
                serverStatus = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Ems Messages

        public ObservableCollection<JObject> EmsMessages
        {
            get; set;
        }

        #endregion

        #endregion

        #region Constructor

        public ServerViewModel()
        {
            Server = new Server();
            StartListeningCommand = new StartListeningCommand(this);
            StopListeningCommand = new StopListeningCommand(this);
            ServerStatus = "Not Listening";
            EmsMessages = new ObservableCollection<JObject>();
            Server.SubscribeToAll(Callback_ToAll);
        }

        #endregion

        #region Ems Callbacks

        private void Callback_ToAll(JObject jObject)
        {
            // Insert at the beginning, so that when the list of messages updates, 
            // it will show the newest messages first
            EmsMessages.Insert(0, jObject);
        }

        #endregion
    }
}
