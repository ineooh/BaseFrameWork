using System.Collections.Generic;
using UnityEngine;
//using Amanotes.Utils;
using Firebase;
using Firebase.Analytics;
using Firebase.RemoteConfig;
using System;
using System.Threading.Tasks;


public class FirebaseManager : SingletonMono<FirebaseManager> {
    public bool enableFirebaseCloudMessage;
    public bool enableFirebaseRemoteConfig;
    public bool enableFirebaseAnalytic;

    public float remoteConfigFetchTimeExpire=0;


    private bool isFirebaseAnalyticInitSuccess = false;

    private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

    public static Action<bool> onCheckFirebaseDependencyFinish;
    public bool IsFirebaseAnalyticInitSuccess
    {
        get { return isFirebaseAnalyticInitSuccess; }
    }
    public void Init()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }
	//public void OnEnable()
	//{
 //       Firebase.Messaging.FirebaseMessaging.TokenRegistrationOnInitEnabled = true;
 //       Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
 //       Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
	//}
	//public void OnDisable()
	//{
 //       Firebase.Messaging.FirebaseMessaging.MessageReceived -= OnMessageReceived;
 //       Firebase.Messaging.FirebaseMessaging.TokenReceived -= OnTokenReceived;
	//}


	private void InitializeFirebase()
    {
        Debug.Log("FrameWork InitializeFirebase");
        EnableAnalyticModule();
        //EnableCloudMessageModule();
        EnableRemoteConfigModule();
        DoOnMainThread.Instance.QueueOnMainThread(() =>
        {
            if(onCheckFirebaseDependencyFinish!=null){
                onCheckFirebaseDependencyFinish(true);
            }
        });
    }

    #region Analytic Module
    private void EnableAnalyticModule()
    {
        if (this.enableFirebaseAnalytic == false) return;
        //Debug.Log("Enabling FIREBASE ANALYTIC.");

        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        this.isFirebaseAnalyticInitSuccess = true;
        //AnalyticService.Instance.ReSendEventFailedFirebaseEventQueue();
    }

    public void InitFirebaseProperties()
    {
        if (this.isFirebaseAnalyticInitSuccess)
        {

        }
    }

    public void LogEvent(string eventName)
    {
        try
        {
            FirebaseAnalytics.LogEvent(eventName);
        }
        catch (Exception ex)
        {
            Debug.LogError("Firebase LogEvent Exception:" + ex.Message);
        }
    }

    public void LogEvent(string eventName, params Parameter[] para)
	{
        try
        {
            FirebaseAnalytics.LogEvent(eventName, para);
        }
        catch(Exception ex){
            Debug.LogError("Firebase LogEvent Exception:" + ex.Message);
        }
	}

    string ValidateFirebaseName(string inputStr)
    {
        string outputStr = inputStr.Replace(' ', '_');
        outputStr = outputStr.Replace('-', '_');
        return outputStr;
    }

    public void LogEvent(string eventName, Dictionary<string,object> para=null)
    {
        eventName = ValidateFirebaseName(eventName);
        if (para == null || para.Count < 1)
        {
            LogEvent(eventName);
        }
        else
        {
            List<Parameter> list = new List<Parameter>();
            foreach (KeyValuePair<string, object> pair in para)
            {
                try
                {
                    if (pair.Value is long)
                    {
                        list.Add(new Parameter(ValidateFirebaseName(pair.Key), (long)pair.Value));
                    }
                    else if (pair.Value is float)
                    {
                         list.Add(new Parameter(ValidateFirebaseName(pair.Key), (float)pair.Value));
                    }
                    else if (pair.Value is double)
                    {
                        list.Add(new Parameter(ValidateFirebaseName(pair.Key), (double)pair.Value));
                    }
                    else
                    {
                        if (pair.Value != null)
                        {
                            list.Add(new Parameter(ValidateFirebaseName(pair.Key), pair.Value.ToString()));
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Wrong cast must fix! Error : " + e);
                }
            }
            LogEvent(eventName, list.ToArray());
        }
    }

    public void SetUserProperty(KeyValuePair<string,string> pair)
    {
        if (pair.Key != null && pair.Value != null)
        {
            FirebaseAnalytics.SetUserProperty(pair.Key, pair.Value);
        }
        else
        {
            Debug.LogError("Firebase SetUserProperty Key or Value null!");
        }
    }
    #endregion

    #region RemoteConfig Module

    public static Action<bool> onRemoteConfigFetchComplete;
    private void EnableRemoteConfigModule()
    {
        if (this.enableFirebaseRemoteConfig == false) return;

        var settings = Firebase.RemoteConfig.FirebaseRemoteConfig.Settings;
        settings.IsDeveloperMode = true;
        Firebase.RemoteConfig.FirebaseRemoteConfig.Settings = settings;
        FetchRemoteDataData();
        Development.Log("RemoteConfig configured and ready!");
        
    }

    public int GetIntValueRemoteConfig(string name, int fallbackValue = 0)
    {
        try
        {
            return (int)Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(name).LongValue;
        }
        catch
        {
            return fallbackValue;
        }
    }

    public float GetFloatValueRemoteConfig(string name, float fallbackValue = 0.0f)
    {
        try
        {
            return (float)Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(name).DoubleValue;
        }
        catch
        {
            return fallbackValue;
        }
        
    }

    public string GetStringValueRemoteConfig(string name, string fallbackValue = "")
    {
        try
        {
            return Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(name).StringValue;
        }
        catch
        {
            return fallbackValue;
        }
        
    }

    public bool GetBoolValueRemoteConfig(string name, bool fallbackValue)
    {
        try
        {
            return Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(name).BooleanValue;
        }
        catch
        {
            return fallbackValue;
        }
    }

    public void FetchRemoteDataData(bool isTest = false)
    {

        Development.Log("RemoteConfig Fetching data...");
        // FetchAsync only fetches new data if the current data is older than the provided
        // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
        // By default the timespan is 12 hours, and for production apps, this is a good
        // number.  For this example though, it's set to a timespan of zero, so that
        // changes in the console will always show up immediately.
        TimeSpan timeExpire = TimeSpan.FromHours(remoteConfigFetchTimeExpire);
        System.Threading.Tasks.Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.FetchAsync(
            timeExpire);
        fetchTask.ContinueWith(FetchRemoteComplete);
    }

    private void FetchRemoteComplete(Task fetchTask)
    {
        bool isError = true;
        if (fetchTask.IsCanceled)
        {
            Debug.Log("RemoteConfig Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            Debug.Log("RemoteConfig Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            isError = false;
        }
        DoOnMainThread.Instance.QueueOnMainThread(() =>
        {
            if (isError)
            {
                if (GameInitialManager.Instance.IsCanShowLog)
                {
                    Development.Log("Firebase RemoteConfig Fetch Error");
                }
                if(onRemoteConfigFetchComplete!=null){
                    onRemoteConfigFetchComplete(!isError);
                }
                
            }
            else
            {
                Development.Log("Firebase RemoteConfig Fetch completed successfully!, Developer to do with Firebase Remote Config Here");
                var info = Firebase.RemoteConfig.FirebaseRemoteConfig.Info;
                switch (info.LastFetchStatus)
                {
                    case Firebase.RemoteConfig.LastFetchStatus.Success:
                        Firebase.RemoteConfig.FirebaseRemoteConfig.ActivateFetched();
                        Development.Log(string.Format("RemoteConfig Remote data loaded and ready (last fetch time {0}).",
                                               info.FetchTime));
                        break;
                    case Firebase.RemoteConfig.LastFetchStatus.Failure:
                        switch (info.LastFetchFailureReason)
                        {
                            case Firebase.RemoteConfig.FetchFailureReason.Error:
                                Development.Log("RemoteConfig Fetch failed for unknown reason");
                                break;
                            case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                                Development.Log("RemoteConfig Fetch throttled until " + info.ThrottledEndTime);
                                break;
                        }
                        break;
                    case Firebase.RemoteConfig.LastFetchStatus.Pending:
                        Development.Log("RemoteConfig Latest Fetch call still pending.");
                        break;
                }
                if(onRemoteConfigFetchComplete!=null){
                    onRemoteConfigFetchComplete(!isError);
                }
                //test show data
                //DisplayAllKeys();

            }

        });
    }
  
    public void DisplayAllKeys()
    {
        Development.Log("RemoteConfig Current Keys:");
        
        System.Collections.Generic.IEnumerable<string> keys =
            Firebase.RemoteConfig.FirebaseRemoteConfig.Keys;
        foreach (string key in keys)
        {
           Development.Log("Firebase Remotes Key:" + key);
        }
    }
    #endregion

    #region Cloud Message
    /*
    private string topic = "TestTopic";
    private void EnableCloudMessageModule()
    {
        if (this.enableFirebaseCloudMessage == false) return;
        
        Firebase.Messaging.FirebaseMessaging.SubscribeAsync(topic);
        //Development.Log("Firebase Messaging Initialized");
    }

    public virtual void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        //Debug.Log("Received a new message");
        var notification = e.Message.Notification;
        if (notification != null)
        {
            Debug.Log("title: " + notification.Title);
            Debug.Log("body: " + notification.Body);
        }
        if (e.Message.From.Length > 0)
            Debug.Log("from: " + e.Message.From);
        if (e.Message.Link != null)
        {
            Debug.Log("link: " + e.Message.Link.ToString());
        }
        if (e.Message.Data.Count > 0)
        {
            Debug.Log("data:");
            foreach (System.Collections.Generic.KeyValuePair<string, string> iter in
                     e.Message.Data)
            {
                Debug.Log("  " + iter.Key + ": " + iter.Value);
            }
        }
    }

    public virtual void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        Debug.Log("Firebase Received Registration Token: " + token.Token);
    }

    private void OnDestroy()
    {
        if (this.enableFirebaseCloudMessage)
        {
            Firebase.Messaging.FirebaseMessaging.MessageReceived    -= OnMessageReceived;
            Firebase.Messaging.FirebaseMessaging.TokenReceived      -= OnTokenReceived;
        }
    }
    */
    #endregion
}
