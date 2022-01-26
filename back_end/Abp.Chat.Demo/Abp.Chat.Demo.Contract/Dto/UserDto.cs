using System.ComponentModel.DataAnnotations;

namespace Abp.Chat.Demo.Contract.Dto
{
    public class UserDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string UserName { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(20)]
        //TODO 后面加上正则表达式，防止密码输入中文
        //[RegularExpression(@"/^[A-Za-z0-9]{8,20}$/")]
        public string Password { get; set; }
    }
}
