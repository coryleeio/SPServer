namespace SPServer
{
    using ExitGames.Logging;
    using Photon.SocketServer;
    using LogManager = ExitGames.Logging.LogManager;
    public class SPServerApplication : Photon.LoadBalancing.GameServer.GameApplication
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        public SPServerApplication() : base()
        {
        }

        protected override PeerBase CreateGamePeer(InitRequest initRequest)
        {
            return new SPServer.GameClientPeer(initRequest, this);
        }
    }
}
