#if UNITY_IOS
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.iOS;
using UnityEngine;

//namespace GFramework.LocalPush
//{
    public class NotificationForIOS : NotificationBase
    {
        /// Initial LocalPush
        public void InitialLocalNotification()
        {
            //Auto request by checking on project seting
            UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert | UnityEngine.iOS.NotificationType.Badge | UnityEngine.iOS.NotificationType.Sound);
        }

        // Register A Notification -> Using for local push is prefer
        public void ScheduleNotification(NotificationData data)
        {
            //Old local notification
            /*
            UnityEngine.iOS.LocalNotification notification = new UnityEngine.iOS.LocalNotification();
            notification.alertTitle = data.title;
            notification.alertBody = data.content;
            //notification.IntentData = data.data;
            notification.fireDate = System.DateTime.Now.AddSeconds(data.timeToSchedule);

            UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notification);
            */

            iOSNotificationTimeIntervalTrigger timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = new TimeSpan(0, 0, data.timeToSchedule),
                Repeats = false
            };

            iOSNotification notification = new iOSNotification()
            {
                // You can optionally specify a custom identifier which can later be 
                // used to cancel the notification, if you don't set one, a unique 
                // string will be generated automatically.
                Identifier = data.notification_id.ToString(),
                Title = data.title,
                Body = data.content,
                Subtitle = "",
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_beatsaber",
                ThreadIdentifier = "thread_beatsaber",
                Trigger = timeTrigger,
            };

            iOSNotificationCenter.ScheduleNotification(notification);
        }

        // Cancel All Notification
        public void CancelAllNotification()
        {
            iOSNotificationCenter.RemoveAllScheduledNotifications();
        }

        public bool GetNotificationContent()
        {
            return iOSNotificationCenter.GetLastRespondedNotification() != null;
        }
    }
//}
#endif