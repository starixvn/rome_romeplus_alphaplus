namespace AgencyCash.API.Notifications.Models;

public class NotificationParam(
    int agentId,
    string agentName,
    int custId,
    string personalMessageResourceKey,
    string toastTitleResourceKey,
    string toastBodyResourceKey)
{
    public int AgentId { get; } = agentId;

    public string AgentName { get; } = agentName;

    public int CustId { get; } = custId;

    public string PersonalMessageResourceKey { get; } = personalMessageResourceKey;

    public string ToastTitleResourceKey { get; } = toastTitleResourceKey;

    public string ToastBodyResourceKey { get; } = toastBodyResourceKey;
}