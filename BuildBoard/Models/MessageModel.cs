using System.ComponentModel.DataAnnotations;

namespace BuildBoard.Models
{
    public sealed class MessageModel
    {
        [Required]
        public int LocationId { get; set; }

        [Required]
        public string Text { get; set; }
    }
}