using Microsoft.AspNetCore.SignalR;
using Signalr.Log.Brower.Services;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.SignalR;

namespace Signalr.Log.Brower
{
    /// <summary>
    /// TODO 之后做个权限只能Admin使用这个hub
    /// </summary>
    [HubRoute("BrowerLog")]
    public class BrowerLogHub : Hub
    {
        private readonly IBrowerLogServices _services;
        public BrowerLogHub(IBrowerLogServices services)
        {
            _services = services;
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task Hearbeat(string msg)
        {
            while (_services.GetCount() > 0)
            {
                var isSuccess = _services.Dequeue(out var result);
                if (isSuccess)
                {
                    await Clients.All.SendAsync("ReceiveLog", result);
                }
                else
                {
                    await Clients.All.SendAsync("ReceiveLog", "");
                }
            }
        }
        public override Task OnConnectedAsync()
        {
            _services.Online();
            return Task.CompletedTask;
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            _services.Offline();
            return Task.CompletedTask;
        }
    }
}
