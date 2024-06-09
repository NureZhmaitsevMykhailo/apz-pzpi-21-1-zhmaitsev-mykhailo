using API.Core.Contexts;
using API.Core.DTOs.Notification;
using API.Services.Abstractions;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Implementations;

public class NotificationService(OncoBoundDbContext context) : INotificationService
{
    public async Task<Result<NotificationDto>> ReadNotificationById(int notificationId)
    {
        var notification = await context.Notifications
            .FindAsync(notificationId);

        if (notification == null)
        {
            return Result.Fail($"Notification with id {notificationId} not found.");
        }

        notification.isRead = true;
        await context.SaveChangesAsync();

        return Result.Ok(new NotificationDto(notification.Message, notification.isRead, notification.DoctorId));
    }

    public async Task<Result<NotificationDto>> GetNotificationById(int notificationId)
    {
        var notification = await context.Notifications
            .FindAsync(notificationId);

        if (notification == null)
        {
            return Result.Fail<NotificationDto>($"Notification with id {notificationId} not found.");
        }

        return Result.Ok(new NotificationDto(notification.Message, notification.isRead, notification.DoctorId));
    }

    public async Task<Result<List<NotificationDto>>> GetAllNotifications()
    {
        var notifications = await context.Notifications
            .ToListAsync();

        return Result.Ok(notifications.Select(n => new NotificationDto(n.Message, n.isRead, n.DoctorId)).ToList());
    }

    public async Task<Result<NotificationDto>> UpdateNotification(int notificationId, NotificationDto notificationDto)
    {
        var existingNotification = await context.Notifications
            .FindAsync(notificationId);

        if (existingNotification == null)
        {
            return Result.Fail<NotificationDto>($"Notification with id {notificationId} not found.");
        }

        existingNotification.Message = notificationDto.Message;
        existingNotification.isRead = notificationDto.IsRead;
        await context.SaveChangesAsync();

        return Result.Ok(new NotificationDto(existingNotification.Message, existingNotification.isRead,
            existingNotification.DoctorId));
    }

    public async Task<Result> DeleteNotification(int notificationId)
    {
        var notification = await context.Notifications
            .FindAsync(notificationId);

        if (notification == null)
        {
            return Result.Fail($"Notification with id {notificationId} not found.");
        }

        context.Notifications.Remove(notification);
        await context.SaveChangesAsync();

        return Result.Ok();
    }
}
