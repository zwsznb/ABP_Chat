using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Abp.Chat.Demo.Domain
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : Entity<Guid>
    {
        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string UserName { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        public string Password { get; set; }

        public void SetUserId(Guid guid)
        {
            this.Id = guid;
        }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string HeaderImg { get; set; }
        /// <summary>
        /// 用来保存当前登录的sessionId
        /// </summary>
        public string SessionId { get; set; }
        public OnlineState IsOnline { get; set; } = OnlineState.Offline;
        public string ConnectId { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;

    }

    /// <summary>
    /// 线上状态
    /// </summary>
    public enum OnlineState
    {
        Online, //在线
        Offline   //下线
    }
}
