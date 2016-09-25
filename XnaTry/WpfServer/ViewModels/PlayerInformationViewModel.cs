using WpfServer.Models;

namespace WpfServer.ViewModels
{
    public class PlayerInformationViewModel
    {
        public PlayerInformation Player { get; }
        public ServerViewModel Server { get; }

        public PlayerInformationViewModel(PlayerInformation player, ServerViewModel server)
        {
            Player = player;
            Server = server;
        }
    }
}
