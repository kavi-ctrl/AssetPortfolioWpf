using Notifications.Wpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetPortfolioManager.Services
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationManager _notificationManager;

        public NotificationService()
        {
            _notificationManager = new NotificationManager();
        }
        public void ShowError(string message)
        {
            _notificationManager.ShowAsync(new NotificationContent
            {
                Title = "Error",Message = message,Type = NotificationType.Error
            }, areaName: "MainArea");
        }

        public void ShowSuccess(string message)
        {
            _notificationManager.ShowAsync(new NotificationContent
            {
                Title = "Success",Message = message,Type = NotificationType.Success
            }, areaName:"MainArea");
        }

        public void ShowWarning(string message)
        {
            _notificationManager.ShowAsync(new NotificationContent
            {
                Title = "Warning",Message = message,Type = NotificationType.Warning
            }, areaName: "MainArea");
        }
    }
}
