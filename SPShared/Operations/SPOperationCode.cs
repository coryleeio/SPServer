using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPServer.Operations
{
    /// <summary>
    ///  Update flight controls
    /// </summary>
    public enum SPOperationCode : byte
    {
        Leave = 254,
        Join = 255,
        UpdateFlightControls = 10
    }
}
