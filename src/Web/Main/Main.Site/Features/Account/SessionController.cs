using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Authorization;
using AlphaPlus.NetThrottling;

namespace Main.Site.Features.Account;

[ApiController]
[AllowAnonymous]
[DisableNetThrottling]
[Route("api/[controller]")]
public class SessionController : ControllerBase
{
    private readonly Fanex.Logging.ILogger _logger;
    private readonly IUserSession _userSession;

    public SessionController(Fanex.Logging.ILogger logger, IUserSession userSession)
    {
        _logger = logger;
        _userSession = userSession;
    }

    [HttpPost("log")]
    public async Task<IActionResult> LogSessionMismatch([FromBody] SessionMismatchLogModel logData)
    {
        try
        {
            logData.CustId = _userSession.CustId;
            logData.Username = _userSession.UserName;
            logData.SubAccId = _userSession.SubAccId;
            logData.AdminId = _userSession.AdminId;
            logData.AdminName = _userSession.AdminName;
            logData.IpAddress = _userSession.UserLoginIp;
            logData.RoleId = _userSession.RoleId;

            await _logger.ErrorAsync($"Client Side Comparison: Session Cookie Mismatch:{Environment.NewLine}{System.Text.Json.JsonSerializer.Serialize(logData,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = true })}");

            return Ok(new { success = true, message = "Session mismatch logged successfully" });
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync($"Error logging session mismatch: {ex.Message}", ex);
            return StatusCode(500, new { success = false, message = "Failed to log session mismatch" });
        }
    }
}

public class SessionMismatchLogModel
{
    public string Event { get; set; } = string.Empty;
    public string OriginalSessionId { get; set; } = string.Empty;
    public string NewSessionId { get; set; } = string.Empty;
    public string Timestamp { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Referrer { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public int CustId { get; set; }
    public string Username { get; set; } = string.Empty;
    public int SubAccId { get; set; }
    public int AdminId { get; set; }
    public string AdminName { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public string IpAddress { get; set; } = string.Empty;
}