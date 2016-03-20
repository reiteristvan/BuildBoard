using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace BuildBoard.Services
{
    public interface IBoardService
    {
        IEnumerable<Location> GetLocations();
        Task AddMessage(int locationId, string text);
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
        }
    }
}