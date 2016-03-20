using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace BuildBoard.Hubs
{
    public class MessageBroadcastHub : Hub
    {
        public Task JoinLocation(int locationId)
        {
            return Groups.Add(Context.ConnectionId, locationId.ToString());
        }
    }
}