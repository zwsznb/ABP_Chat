using System;

namespace Abp.Chat.Demo.Contract.Dto
{
    public class ChatMsgDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime SendTime { get; set; }
        public string Message { get; set; }
        public bool IsMe { get; set; }
    }
}
