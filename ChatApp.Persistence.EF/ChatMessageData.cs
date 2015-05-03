using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Persistence.EF
{
    [Table("ChatMessages")]
    public class ChatMessageData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [Index]
        public DateTime Timestamp { get; set; }
    }
}
