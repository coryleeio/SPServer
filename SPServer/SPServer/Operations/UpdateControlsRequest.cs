using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPServer.Operations
{
    #region using directives

    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    #endregion


    public class UpdateControlsRequest : Operation
    {

        #region Constructors and Destructors

        public UpdateControlsRequest(IRpcProtocol protocol, OperationRequest operationRequest)
            : base(protocol, operationRequest) {}

        public UpdateControlsRequest() { }


        #endregion

        #region Properties

            [DataMember(Code = (byte)SPParameterCode.MoveForward, IsOptional = false)]
            public Boolean moveForward { get; set; }

            [DataMember(Code = (byte)SPParameterCode.MoveBackward, IsOptional = false)]
            public Boolean moveBackward { get; set; }

            [DataMember(Code = (byte)SPParameterCode.MoveLeft, IsOptional = false)]
            public Boolean turnLeft { get; set; }

            [DataMember(Code = (byte)SPParameterCode.MoveRight, IsOptional = false)]
            public Boolean turnRight { get; set; }

        #endregion
    }
}
