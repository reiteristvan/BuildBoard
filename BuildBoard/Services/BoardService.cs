using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using BuildBoard.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace BuildBoard.Services
{
    public interface IBoardService
    {
        IEnumerable<Location> GetLocations();
        Task AddMessage(int locationId, string text);
        IEnumerable<MessageTableEntity> GetMessages(int locationId, int top);
    }

    public sealed class BoardService : IBoardService
    {
        private readonly ILocationProvider _locationProvider;
        private readonly CloudTable _messagesTable;

        public BoardService(ILocationProvider locationProvider)
        {
            _locationProvider = locationProvider;

            string connectionString = ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString;
            CloudStorageAccount cloudAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = cloudAccount.CreateCloudTableClient();

            _messagesTable = tableClient.GetTableReference("buildboardmessages");
            _messagesTable.CreateIfNotExists();
        }

        public IEnumerable<Location> GetLocations()
        {
            return _locationProvider.GetLocations();
        }

        public async Task AddMessage(int locationId, string text)
        {
            if (_locationProvider.GetLocations().All(l => l.Id != locationId))
            {
                throw new ArgumentException("Unknown location");
            }

            MessageTableEntity tableEntity = new MessageTableEntity(locationId.ToString())
            {
                Text =  text,
                Date = DateTimeOffset.Now
            };

            await _messagesTable.ExecuteAsync(TableOperation.Insert(tableEntity));

            IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<MessageBroadcastHub>();
            hubContext.Clients.Groups(new List<string> { locationId.ToString() }).broadcastMessage(text);
        }

        public IEnumerable<MessageTableEntity> GetMessages(int locationId, int top)
        {
            TableQuery<MessageTableEntity> query = new TableQuery<MessageTableEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, locationId.ToString()));
            IEnumerable<MessageTableEntity> result = _messagesTable.ExecuteQuery(query).OrderByDescending(m => m.Date).Take(top);

            return result;
        }
    }
}