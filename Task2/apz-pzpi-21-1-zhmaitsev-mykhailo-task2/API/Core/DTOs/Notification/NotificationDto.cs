namespace API.Core.DTOs.Notification;

public record NotificationDto(string Message, bool IsRead, int DoctorId);
