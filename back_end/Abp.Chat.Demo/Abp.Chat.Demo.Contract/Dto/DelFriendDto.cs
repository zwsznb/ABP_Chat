using System;
using System.ComponentModel.DataAnnotations;

namespace Abp.Chat.Demo.Contract.Dto
{
    public class DelFriendDto
    {
        [Required]
        public Guid FriendId { get; set; }
    }
}
