using System;
using System.ComponentModel.DataAnnotations;

namespace Abp.Chat.Demo.Contract.Dto
{
    public class InvitationDto
    {
        [Required]
        public Guid FriendId { get; set; }
    }
}
