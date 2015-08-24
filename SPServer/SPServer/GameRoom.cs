using Lite;
using Lite.Diagnostics.OperationLogging;
using Lite.Operations;
using Photon.LoadBalancing.GameServer;
using Photon.LoadBalancing.Operations;
using Photon.SocketServer;
using SPServer.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPServer
{
    class GameRoom : Photon.LoadBalancing.GameServer.Game
    {
        public GameRoom(string gameId, GameCache roomCache = null, int emptyRoomLiveTime = 0) : base(gameId, roomCache, emptyRoomLiveTime)
        {

        }

        protected override void ExecuteOperation(LitePeer peer, OperationRequest operationRequest, SendParameters sendParameters)
        {
            try
            {
                if (Log.IsDebugEnabled)
                {
                    Log.DebugFormat("Executing operation {0}", operationRequest.OperationCode);
                }

                switch ((byte)operationRequest.OperationCode)
                {
                    case (byte)SPServer.Operations.SPOperationCode.Leave:
                        HandleLeaveOperation(peer, operationRequest, sendParameters);
                        break;

                    //case (byte)SPServer.Operations.SPOperationCode.UpdateFlightControls:
                        //HandleUpdateFlightControls(peer, operationRequest, sendParameters);
                        //break;

                    // Block non-authoritative Lite op codes. 
                    case (byte)Lite.Operations.OperationCode.RaiseEvent:
                    case (byte)Lite.Operations.OperationCode.GetProperties:
                    case (byte)Lite.Operations.OperationCode.SetProperties:
                    case (byte)Lite.Operations.OperationCode.ChangeGroups:
                        HandleLiteOperations(peer, operationRequest, sendParameters);
                        break;
                    // all other operation codes will be handled by the LoadBalancing Game implementation
                    default:
                        base.ExecuteOperation(peer, operationRequest, sendParameters);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void HandleUpdateFlightControls(LitePeer peer, OperationRequest operationRequest, SendParameters sendParameters)
        {
            var op = new UpdateControlsRequest(peer.Protocol, operationRequest);
            if (peer.ValidateOperation(op, sendParameters) == false)
            {
                return;
            }
            DebugLog(peer, operationRequest);

            op.OnStart();
            peer.SendOperationResponse(new OperationResponse { OperationCode = op.OperationRequest.OperationCode }, sendParameters);
            op.OnComplete();
        }

        private void DebugLog(LitePeer peer, OperationRequest operationRequest)
        {
            if (this.LogQueue.Log.IsDebugEnabled)
            {
                this.LogQueue.Add(
                    new LogEntry(
                        "ExecuteOperation: " + (Lite.Operations.OperationCode)operationRequest.OperationCode,
                        "Peer=" + peer.ConnectionId));
            }
        }

        private void HandleLiteOperations(LitePeer peer, OperationRequest operationRequest, SendParameters sendParameters)
        {
            string message = string.Format("Unknown operation code {0}", (Lite.Operations.OperationCode)operationRequest.OperationCode);
            peer.SendOperationResponse(
                new OperationResponse { OperationCode = operationRequest.OperationCode, ReturnCode = -1, DebugMessage = message }, sendParameters);

            if (Log.IsWarnEnabled)
            {
                Log.Warn(message);
            }
        }

        private void HandleLeaveOperation(LitePeer peer, OperationRequest operationRequest, SendParameters sendParameters)
        {
            var leaveOperation = new LeaveRequest(peer.Protocol, operationRequest);
            if (peer.ValidateOperation(leaveOperation, sendParameters) == false)
            {
                return;
            }

            DebugLog(peer, operationRequest);

            leaveOperation.OnStart();
            this.HandleLeaveOperation(peer, leaveOperation, sendParameters);
            leaveOperation.OnComplete();
        }
    }
}
