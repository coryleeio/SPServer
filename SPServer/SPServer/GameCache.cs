using Lite;
using Lite.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPServer
{
    class GameCache : Photon.LoadBalancing.GameServer.GameCache
    {
        // Hide instance of Photon.LoadBalancing.GameServer.GameCache
        public new static readonly GameCache Instance = new GameCache();

        protected override Room CreateRoom(string roomId, params object[] args)
        {
            return new GameRoom(roomId, this, GameServerSettings.Default.EmptyRoomLiveTime);
        }
    }
}
