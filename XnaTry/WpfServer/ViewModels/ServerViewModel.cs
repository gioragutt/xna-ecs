using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;
using WpfServer.Commands;
using WpfServer.Models;
using WpfServer.Windows;
using XnaServerLib;

namespace WpfServer.ViewModels
{
    public class ServerViewModel : ViewModelBase
    {
        public Server Server { get; }
        public PlayerInformationViewModel PlayerInformationViewModel { get; }
        public Dispatcher Dispatcher { get; }

        public ObservableItemCollection<PlayerInformation> PlayerData => PlayerInformationViewModel.PlayerData;

        #region Commands

        public ICommand StartListeningCommand { get; set; }
        public ICommand StopListeningCommand { get; set; }
        public ICommand KickPlayerCommand { get; set; }
        public ICommand OpenPlayerInformationWindowCommand { get; set; }

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

        public ServerViewModel(Dispatcher dispatcher)
        {
            Server = new Server();
            Dispatcher = dispatcher;
            PlayerInformationViewModel = new PlayerInformationViewModel();

            StartListeningCommand = new StartListeningCommand(this);
            StopListeningCommand = new StopListeningCommand(this);
            KickPlayerCommand = new KickPlayerCommand(this);
            OpenPlayerInformationWindowCommand = new OpenPlayerInformationWindowCommand(Dispatcher);

            ServerStatus = "Not Listening";
            EmsMessages = new ObservableCollection<JObject>();

            Server.SubscribeToAll(Callback_ToAll);
            Server.ClientConnected += ServerOnClientConnected;
            Server.ClientDisconnected += ServerOnClientDisconnected;
            Server.ClientUpdateReceived += ServerOnClientUpdateReceived;
        }

        private void ServerOnClientUpdateReceived(object sender, PlayerObjectEventArgs playerObjectEventArgs)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(() => PlayerInformationViewModel.UpdatePlayer(playerObjectEventArgs.GameObject)));
        }

        private void ServerOnClientDisconnected(object sender, PlayerIdEventArgs playerIdEventArgs)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(() => PlayerInformationViewModel.RemovePlayer(playerIdEventArgs.Id)));
        }

        private void ServerOnClientConnected(object sender, PlayerObjectEventArgs playerObjectEventArgs)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(() => PlayerInformationViewModel.AddPlayer(playerObjectEventArgs.GameObject)));
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
