using System;
using System.ComponentModel.DataAnnotations;

namespace Abp.Chat.Demo.Contract.Dto
{
    public class ChatMsgReciveDto
    {
        [Required]
        public Guid CurrentChatUserId { get; set; }
    }
}
