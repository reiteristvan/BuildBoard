using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace BuildBoard.Services
{
    public class MessageTableEntity : TableEntity
    {
        public MessageTableEntity(string partitionKey)
        {
            PartitionKey = partitionKey;
            RowKey = Guid.NewGuid().ToString();
        }

        public string Text { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}