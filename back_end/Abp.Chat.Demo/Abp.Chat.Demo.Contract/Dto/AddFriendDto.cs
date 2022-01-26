using System;

namespace Abp.Chat.Demo.Contract.Dto
{
    public class AddFriendDto
    {
        public Guid UserId { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
    }
}
