using System;

namespace Abp.Chat.Demo.Contract.Dto
{
    public class RedisUserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }
}
