//#define ENABLE_LOCAL_NOTIFICATION

#if ENABLE_LOCAL_NOTIFICATION
using GFramework.Internal;

//namespace GFramework.LocalPush
//{
    public class NotificationManagerBase : SingletonMono<NotificationManagerBase>
    {
        private NotificationBase notificationBase = null;
        public NotificationBase GetNotification()
        {
            if (notificationBase == null)
            {
#if UNITY_ANDROID
                notificationBase = new NotificationForAndroid();
#else
                notificationBase = new NotificationForIOS();
#endif
            }
            return notificationBase;
        }

        public virtual void InitNotification()
        {
            GetNotification().InitialLocalNotification();
        }

        //  Register Notification 
        public virtual void ScheduleNotification(NotificationData data)
        {
            GetNotification().ScheduleNotification(data);
        }

        // Cancel All Notification
        public virtual void CancelAllNotification()
        {
            GetNotification().CancelAllNotification();
        }

        public virtual bool GetNotificationContent()
        {
            return GetNotification().GetNotificationContent();
        }
    }
//}
#endif