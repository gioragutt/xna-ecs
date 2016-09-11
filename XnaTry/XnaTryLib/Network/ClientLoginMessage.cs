namespace XnaCommonLib.Network
{
    public class ClientLoginMessage
    {
        public uint MessageHeader { get; set; }
        public string PlayerName { get; set; }
        public string PlayerTeam { get; set; }
        public uint MessageFooter { get; set; }
    }
}
