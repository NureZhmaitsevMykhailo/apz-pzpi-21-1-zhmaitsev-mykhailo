using API.Core.Attributes;
using API.Core.DTOs.Notification;
using API.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/notification")]
[DoctorRoleInterceptor]
[Authorize]
public class NotificationController (INotificationService notificationService) : ControllerBase
{
    [HttpPost("readNotification/{notificationId}")]
    public async Task<IActionResult> ReadNotificationById(int notificationId)
    {
        var result = await notificationService.ReadNotificationById(notificationId);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return NotFound(result.Errors);
    }
    
    [HttpGet("{notificationId}")]
    public async Task<IActionResult> GetNotificationById(int notificationId)
    {
        var result = await notificationService.GetNotificationById(notificationId);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return NotFound(result.Errors);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllNotifications()
    {
        var result = await notificationService.GetAllNotifications();

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Errors);
    }

    [HttpPut("{notificationId}")]
    public async Task<IActionResult> UpdateNotification(int notificationId, [FromBody] NotificationDto notificationDto)
    {
        var result = await notificationService.UpdateNotification(notificationId, notificationDto);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Errors);
    }

    [HttpDelete("{notificationId}")]
    public async Task<IActionResult> DeleteNotification(int notificationId)
    {
        var result = await notificationService.DeleteNotification(notificationId);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(result.Errors);
    }
}
