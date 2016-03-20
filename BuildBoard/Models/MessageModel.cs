using System;

namespace BuildBoard.Models
{
    public sealed class MessageModel
    {
        public string Message { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}