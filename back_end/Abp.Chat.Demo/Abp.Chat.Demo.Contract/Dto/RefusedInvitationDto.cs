using System;
using System.ComponentModel.DataAnnotations;

namespace Abp.Chat.Demo.Contract.Dto
{
    public class RefusedInvitationDto
    {
        [Required]
        public Guid InvitationId { get; set; }
    }
}
