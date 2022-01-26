using Abp.Chat.Demo.Contract.Dto;
using Abp.Chat.Demo.Domain;
using Custom.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Abp.Chat.Demo.Contract.Interfaces
{
    public interface IChatManagerService : IApplicationService
    {
        Task<ApiResult<ChatInformation>> SendMsg(ChatMessageDto chatMessageDto);
        Task<ApiResult<List<ChatMsgDto>>> FindMsgsAndReadByFriend(ChatMsgReciveDto chatMsgReciveDto);
        Task<ApiResult<List<GroupInformationCountDto>>> FindAllNewMessage();
        Task<ApiResult<object>> ReadSingleMessage(ReadSingleMessageDto readSingleMessageDto);
    }
}
