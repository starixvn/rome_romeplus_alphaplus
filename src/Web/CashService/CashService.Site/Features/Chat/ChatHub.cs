using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace AlphaPlus.CashService.Site.Features.ChatHub;

public class ChatHub : Hub
{
    private static readonly ConcurrentDictionary<string, string> _userConnections = new();
    private static readonly ConcurrentDictionary<string, HashSet<string>> _userGroups = new();

    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task SendPrivateMessage(string fromUser, string toUser, string message)
    {
        var connectionId = _userConnections.FirstOrDefault(x => x.Value == toUser).Key;
        if (!string.IsNullOrEmpty(connectionId))
        {
            await Clients.Client(connectionId).SendAsync("ReceivePrivateMessage", fromUser, message);
            await Clients.Caller.SendAsync("ReceivePrivateMessage", fromUser, message);
        }
    }

    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        if (!_userGroups.ContainsKey(Context.ConnectionId))
        {
            _userGroups[Context.ConnectionId] = new HashSet<string>();
        }
        _userGroups[Context.ConnectionId].Add(groupName);
        await Clients.Group(groupName).SendAsync("UserJoinedGroup", Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        if (_userGroups.ContainsKey(Context.ConnectionId))
        {
            _userGroups[Context.ConnectionId].Remove(groupName);
        }
        await Clients.Group(groupName).SendAsync("UserLeftGroup", Context.ConnectionId, groupName);
    }

    public async Task SendGroupMessage(string groupName, string user, string message)
    {
        await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", user, message);
    }

    public async Task SendTypingIndicator(string user, bool isTyping)
    {
        await Clients.All.SendAsync("UserTyping", user, isTyping);
    }

    public async Task GetOnlineUsers()
    {
        var onlineUsers = _userConnections.Values.ToList();
        await Clients.Caller.SendAsync("OnlineUsers", onlineUsers);
    }

    public override async Task OnConnectedAsync()
    {
        var user = Context.User?.Identity?.Name ?? "Anonymous";
        _userConnections[Context.ConnectionId] = user;
        await Clients.All.SendAsync("UserConnected", user);
        await Clients.All.SendAsync("OnlineUsers", _userConnections.Values.ToList());
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = _userConnections.GetValueOrDefault(Context.ConnectionId);
        _userConnections.TryRemove(Context.ConnectionId, out _);
        _userGroups.TryRemove(Context.ConnectionId, out _);
        
        if (!string.IsNullOrEmpty(user))
        {
            await Clients.All.SendAsync("UserDisconnected", user);
            await Clients.All.SendAsync("OnlineUsers", _userConnections.Values.ToList());
        }
        
        await base.OnDisconnectedAsync(exception);
    }
} 