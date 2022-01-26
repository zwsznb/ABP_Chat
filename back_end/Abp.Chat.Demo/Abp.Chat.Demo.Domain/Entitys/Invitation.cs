using System;
using Volo.Abp.Domain.Entities;

namespace Abp.Chat.Demo.Domain
{
    /// <summary>
    /// 好友邀请，这里可能要采用后台作业
    /// </summary>
    public class Invitation : Entity<Guid>
    {
        public Guid UserId { get; set; }
        public Guid ChumId { get; set; }
        public InvitationAcceptState IsAccept { get; set; } = InvitationAcceptState.NONE;
        public bool IsReaded { get; set; } = false;
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public void SetInvitationId(Guid guid)
        {
            this.Id = guid;
        }
    }
}
