using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp_Chess.Services
{
    public class UserService
    {
        public event Action Notify;
        public string User { get; set; }
        public string Message { get; set; }

        private HubConnection _hubConnection;

        public UserService(NavigationManager navigationManager)
        {
            _hubConnection = new HubConnectionBuilder()
            .WithUrl(navigationManager.ToAbsoluteUri("/chathub"))
            .Build();

            _hubConnection.On<string, string>("ReceiveMessage", (user,
                                                              message) =>
            {
                User = user;
                Message = message;

                if (Notify != null)
                {
                    Notify?.Invoke();
                }
            });

            _hubConnection.StartAsync();
            _hubConnection.SendAsync("SendMessage", null, null);
        }

        public void Send(string userInput, string messageInput) =>
          _hubConnection.SendAsync("SendMessage", userInput, messageInput);

        public bool IsConnected => _hubConnection.State ==
                                               HubConnectionState.Connected;

    }
}
