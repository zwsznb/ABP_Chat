using System;
using System.ComponentModel.DataAnnotations;

namespace Abp.Chat.Demo.Contract.Dto
{
    public class UpdateFriendDto
    {
        [Required]
        public Guid FriendId { get; set; }
        [Required]
        public string NickName { get; set; }
    }
}
