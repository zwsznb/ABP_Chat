using System;
using System.ComponentModel.DataAnnotations;

namespace Abp.Chat.Demo.Contract.Dto
{
    public class ChatMessageDto
    {
        [Required]
        public Guid ReciveUserId { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
