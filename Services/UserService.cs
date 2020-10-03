using BlazorServerApp_Chess.Hubs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.SignalR;
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
        public string Time { get; set; }

        private HubConnection _hubConnection;

        public UserService(NavigationManager navigationManager)
        {
            _hubConnection = new HubConnectionBuilder()
                    .WithUrl(navigationManager.ToAbsoluteUri("/chathub"))
                    .Build();

            _hubConnection.On<string, string, string>("ReceiveMessage", (user, message, time) =>
            {
                User = user;
                Message = message;
                Time = time;

                if (Notify != null)
                {
                    Notify?.Invoke();
                }
            });

            _hubConnection.StartAsync();
        }

        public void Send(string userInput, string messageInput) => 
            _hubConnection.SendAsync("SendMessage", userInput, messageInput);

        public void SendGreetings(string userInput) =>
            _hubConnection.SendAsync("SendMessageToOthers", userInput, $"{userInput} joined the channel!");

        public bool IsConnected => 
            _hubConnection.State == HubConnectionState.Connected;

    }
}
