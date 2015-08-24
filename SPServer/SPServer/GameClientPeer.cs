
using ExitGames.Logging;
using Lite;
using Lite.Caching;
using Photon.LoadBalancing.GameServer;
using Photon.LoadBalancing.Operations;
using Photon.SocketServer;
using SPServer.Operations;
using SPShared.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPServer
{
    class GameClientPeer : Photon.LoadBalancing.GameServer.GameClientPeer
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public GameClientPeer(InitRequest initRequest, GameApplication application)
            : base(initRequest, application) {}

        protected override void OnOperationRequest(OperationRequest request, SendParameters sendParameters)
        {
            switch (request.OperationCode)
            {
                case (byte)OperationCode.Authenticate:
                    this.HandleAuthenticateOperation(request, sendParameters);
                    return;

                case (byte)OperationCode.CreateGame:
                    this.HandleCreateGameOperation(request, sendParameters);
                    return;

                case (byte)OperationCode.JoinGame:
                    this.HandleJoinGameOperation(request, sendParameters);
                    return;

                case (byte)Lite.Operations.OperationCode.Leave:
                    this.HandleLeaveOperation(request, sendParameters);
                    return;

                case (byte)Lite.Operations.OperationCode.Ping:
                    this.HandlePingOperation(request, sendParameters);
                    return;

                case (byte)OperationCode.DebugGame:
                    this.HandleDebugGameOperation(request, sendParameters);
                    return;

                case (byte)SPOperationCode.UpdateFlightControls:
                this.HandleGameOperation(request, sendParameters);
                    return;  
            }
            
            base.OnOperationRequest(request, sendParameters);
        }

        protected override RoomReference GetOrCreateRoom(string gameId)
        {
            return GameCache.Instance.GetRoomReference(gameId, this);
        }

        protected override bool TryCreateRoom(string gameId, out RoomReference roomReference)
        {
            return GameCache.Instance.TryCreateRoom(gameId, this, out roomReference);
        }

        protected override bool TryGetRoomReference(string gameId, out RoomReference roomReference)
        {
            return GameCache.Instance.TryGetRoomReference(gameId, this, out roomReference);
        }

        protected override bool TryGetRoomWithoutReference(string gameId, out Room room)
        {
            return GameCache.Instance.TryGetRoomWithoutReference(gameId, out room); 
        }

        public override string GetRoomCacheDebugString(string gameId)
        {
            return GameCache.Instance.GetDebugString(gameId);
        }
    }
}
