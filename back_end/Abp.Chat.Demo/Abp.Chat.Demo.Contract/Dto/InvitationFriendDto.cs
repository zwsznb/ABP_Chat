using System;
using System.ComponentModel.DataAnnotations;

namespace Abp.Chat.Demo.Contract.Dto
{
    public class InvitationFriendDto
    {
        [Required]
        public Guid FriendId { get; set; }
        [Required]
        public Guid InvitationId { get; set; }
    }
}
