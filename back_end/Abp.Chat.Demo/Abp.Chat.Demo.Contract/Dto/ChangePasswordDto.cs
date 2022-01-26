using System.ComponentModel.DataAnnotations;

namespace Abp.Chat.Demo.Contract.Dto
{
    public class ChangePasswordDto
    {
        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        public string NewPassword { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        public string OldPassword { get; set; }
    }
}
