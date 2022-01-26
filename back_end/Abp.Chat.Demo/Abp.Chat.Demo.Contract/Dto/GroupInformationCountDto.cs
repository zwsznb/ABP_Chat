using System;

namespace Abp.Chat.Demo.Contract.Dto
{
    public class GroupInformationCountDto
    {
        public Guid UserId { get; set; }
        public int UnRead { get; set; }
    }
}
