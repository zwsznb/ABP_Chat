using System;
using Volo.Abp.Domain.Entities;

namespace Abp.Chat.Demo.Domain
{
    /// <summary>
    /// 好友(单对单)
    /// </summary>
    public class Friend : Entity<Guid>
    {
        public Guid UserId { get; set; }
        public Guid ChumId { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public void SetFriendId(Guid guid)
        {
            this.Id = guid;
        }
    }
}
