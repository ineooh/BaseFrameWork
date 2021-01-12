using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace GFramework.LocalPush
//{
    public interface NotificationBase
    {
        /// Initial LocalPush
        void InitialLocalNotification();

        // Register A Notification -> Using for local push is prefer
        void ScheduleNotification(NotificationData data);

        // Cancel All Notification
        void CancelAllNotification();

        //check if the game is launched from notification
        bool GetNotificationContent();
    }

    public class NotificationData
    {
        // id of notification -> unique for each spec push: using for create or remove push
        public int notification_id;

        // title of push
        public string title;

        // push content
        public string content;

        // push content
        public string data;

        // time to schedule in second
        public int timeToSchedule;
    }
//}