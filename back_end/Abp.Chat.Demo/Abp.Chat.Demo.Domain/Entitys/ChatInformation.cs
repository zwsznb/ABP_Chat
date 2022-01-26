using System;
using Volo.Abp.Domain.Entities;

namespace Abp.Chat.Demo.Domain
{
    /// <summary>
    /// 聊天信息
    /// </summary>
    public class ChatInformation : Entity<Guid>
    {
        public Guid SenderId { get; set; }
        public Guid RecivedId { get; set; }
        public string Message { get; set; }
        public bool IsImgMessage { get; set; } = false;
        public string Path { get; set; }
        public bool IsRead { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public void SetChatInformationId(Guid guid)
        {
            this.Id = guid;
        }
    }
}
