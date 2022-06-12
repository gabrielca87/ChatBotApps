using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Server.Models
{
    [Table("ChatMessage")]
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string User { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
    }
}
