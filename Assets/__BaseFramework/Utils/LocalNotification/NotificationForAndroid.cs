//#define ENABLE_LOCAL_NOTIFICATION

#if UNITY_ANDROID && ENABLE_LOCAL_NOTIFICATION
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

//namespace GFramework.LocalPush
//{
    public class NotificationForAndroid : NotificationBase
    {
        public const string default_chanel_id = "noti_beatsaber";

        /// Initial LocalPush
        public void InitialLocalNotification()
        {
            var chanelNoti = new AndroidNotificationChannel()
            {
                Id = default_chanel_id,
                Name = "Beatsaber Channel",
                Importance = Importance.High,
                Description = "Beatsaber notifications",
            };
            AndroidNotificationCenter.RegisterNotificationChannel(chanelNoti);
        }

        // Register A Notification
        public void ScheduleNotification(NotificationData data)
        {
            var notification = new AndroidNotification();
            notification.Title = data.title;
            notification.Text = data.content;
            notification.IntentData = data.data;
            notification.FireTime = System.DateTime.Now.AddSeconds(data.timeToSchedule);

            AndroidNotificationCenter.SendNotification(notification, default_chanel_id);
        }

        // Cancel All Notification
        public void CancelAllNotification()
        {
            AndroidNotificationCenter.CancelAllNotifications();
        }

        //Get Notification intent to see if the game is launched from notification
        public bool GetNotificationContent()
        {
            return AndroidNotificationCenter.GetLastNotificationIntent() != null;
        }
    }
//}
#endif