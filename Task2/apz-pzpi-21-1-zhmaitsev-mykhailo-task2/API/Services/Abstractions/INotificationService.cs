using API.Core.DTOs.Notification;
using FluentResults;

namespace API.Services.Abstractions;

public interface INotificationService
{
    Task<Result<NotificationDto>> ReadNotificationById(int notificationId);
    
    Task<Result<NotificationDto>> GetNotificationById(int notificationId);
    
    Task<Result<List<NotificationDto>>> GetAllNotifications();
    
    Task<Result<NotificationDto>> UpdateNotification(int notificationId, NotificationDto notificationDto);
    
    Task<Result> DeleteNotification(int notificationId);
}
