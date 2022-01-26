using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Chat.Demo.Contract.Dto
{
    public class ReadSingleMessageDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}
