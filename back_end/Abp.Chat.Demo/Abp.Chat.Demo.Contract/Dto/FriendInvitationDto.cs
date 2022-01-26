using Abp.Chat.Demo.Domain;
using System;

namespace Abp.Chat.Demo.Contract.Dto
{
    public class FriendInvitationDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid ChumId { get; set; }
        public InvitationAcceptState IsAccept { get; set; }
        public bool IsReaded { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
