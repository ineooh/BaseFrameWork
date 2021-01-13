using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using Facebook.MiniJSON;
//using Newtonsoft.Json;
[Serializable]
    public class FBFriend
    {
        public string id;
        public string name;
        public FBFriend()
        {
        }
        public FBFriend(string _id, string _name)
        {
            id = _id;
            name = _name;
        }
    }

    [Serializable]
    public class FBFriendQuery
    {
        public List<FBFriend> data;
    }

    public class CallbackFromFacebook
    {
        public int result_code;
        public IResult result;
        public CallbackFromFacebook()
        {

        }
        public CallbackFromFacebook(int _result_code, IResult _result)
        {
            result_code = _result_code;
            result = _result;
        }
    }

public class FacebookManager : SingletonMono<FacebookManager>
{
    public Action onSuccedInitFB;

    public void Init()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(OnInitComplete, OnHideUnity);
        }
        else
        {

            Debug.LogError("oplab Facebook Activate App1");
            FB.ActivateApp();
        }
    }

    public bool IsLogin()
    {
        if (!FB.IsInitialized)
        {
            Init();
        }
        return FB.IsLoggedIn;
    }

    public Action<CallbackFromFacebook> callbackLogin = null;

    public void FBLogin(Action<CallbackFromFacebook> callback = null)
    {
        callbackLogin = callback;
        if (FB.IsInitialized)
        {
            // FB.LogInWithReadPermissions(new List<string>() {"email"}, CallbackFBLogin);
            FB.LogInWithReadPermissions(new List<string>() { "email", "user_friends" }, CallbackFBLogin);
            //FB.LogInWithReadPermissions(new List<string>() { "email", "public_profile", "user_friends" }, CallbackFBLogin);
        }
    }

    private void CallbackFBLogin(IResult result)
    {
        if (result == null)
        {
            Debug.Log("#101 Null Response");
            if (callbackLogin != null)
            {
                callbackLogin(new CallbackFromFacebook(-101, result));
            }
            return;
        }
        // Some platforms return the empty string instead of null.
        if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("#102 Error Response: " + result.Error);
            if (callbackLogin != null)
            {
                callbackLogin(new CallbackFromFacebook(-101, result));
            }
        }
        else if (result.Cancelled)
        {
            Debug.Log("#100 Cancelled Response: " + result.RawResult);
            //Xu ly khi user bam cancel
            if (callbackLogin != null)
            {
                callbackLogin(new CallbackFromFacebook(-100, result));
            }

        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            //request additional data
            //InitProfileInfo();
            if (callbackLogin != null)
            {
                InitProfileInfo(callbackLogin, result, 1);
            }
        }
        else
        {
            if (callbackLogin != null)
            {
                callbackLogin(new CallbackFromFacebook(-1, result));
            }
        }
    }
    public void InitProfileInfo(Action<CallbackFromFacebook> loginCallback = null, IResult result = null, int code = -1, Action<IGraphResult> callback = null)
    {
        // Get Information User
        FB.API("/me?fields=name,picture.type(large),email,cover", HttpMethod.GET, resGetInfo =>
        {
            var res = Json.Deserialize(resGetInfo.RawResult) as Dictionary<string, object>;
            if (res.ContainsKey("name"))
            {
                UserDisplayName = res["name"] as string;
                IsUserInfoAvailable = true;
            }

            if (res.ContainsKey("email"))
            {
                UserEmail = res["email"] as string;
                IsUserInfoAvailable = true;
            }

            if (res.ContainsKey("picture"))
            {
                Dictionary<string, object> pictureData = res["picture"] as Dictionary<string, object>;
                Dictionary<string, object> actualPictureData = pictureData["data"] as Dictionary<string, object>;
                AvatarURL = actualPictureData["url"] as string;
                IsUserInfoAvailable = true;
            }

            if (res.ContainsKey("cover"))
            {
                Dictionary<string, object> pictureData = res["cover"] as Dictionary<string, object>;
                CoverURL = pictureData["source"] as string;
                IsUserInfoAvailable = true;
            }

                //Debug.Log("Init profile " + UserDisplayName + " " + UserID);
                if (IsUserInfoAvailable)
            {
                    //MT to do update user data
                }

            Debug.Log("Init profile info " + UserDisplayName);
            if (callback != null)
            {
                callback(resGetInfo);
            }

            if (loginCallback != null)
            {
                loginCallback(new CallbackFromFacebook(code, result));
            }

        });
    }

    public bool CheckHavePublishActionPermision()
    {
        string json = AccessToken.CurrentAccessToken.ToJson();
        if (json.Contains("publish_actions,") || json.Contains(",publish_actions"))
        {
            return true;
        }
        return false;
    }

    public void RequestPublishAction(Action<CallbackFromFacebook> publishActionCallback = null)
    {
        if (publishActionCallback != null)
        {
            callbackLogin = publishActionCallback;
        }
        FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, CallbackPublishAction);
    }

    public void CallbackPublishAction(IResult result)
    {
        if (result == null)
        {
            Debug.Log("#103 Null Response");
            if (callbackLogin != null)
            {
                callbackLogin(new CallbackFromFacebook(-103, result));
            }
            return;
        }
        // Some platforms return the empty string instead of null.
        if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.LogError("#104 Error Response: " + result.Error);
            if (callbackLogin != null)
            {
                callbackLogin(new CallbackFromFacebook(-104, result));
            }
        }
        else if (result.Cancelled)
        {
            Debug.Log("Cancelled Response: " + result.RawResult);
            //Xu ly khi user bam cancel
            if (callbackLogin != null)
            {
                callbackLogin(new CallbackFromFacebook(-100, result));
            }
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            //finish
            if (callbackLogin != null)
            {
                callbackLogin(new CallbackFromFacebook(1, result));
            }
        }
        else
        {
            Debug.Log("Empty Response");
            if (callbackLogin != null)
            {
                callbackLogin(new CallbackFromFacebook(-1, result));
            }
        }
    }

    public void ShowFBInviteDialog(string url, string imageurl = null)
    {
        //FB.Mobile.AppInvite(new Uri(url), new Uri(imageurl));
    }

    public void FBLogout()
    {
        IsUserInfoAvailable = false;
        FB.LogOut();
    }

    public void OnInitComplete()
    {
        FB.ActivateApp();
        if (onSuccedInitFB != null)
            onSuccedInitFB();

        FB.Mobile.RefreshCurrentAccessToken(RefreshCallback);

    }

    void RefreshCallback(IAccessTokenRefreshResult result)
    {
        //Debug.Log("init profile access token refresh " + result.AccessToken.ToString());
        if (FB.IsLoggedIn)
        {
            InitProfileInfo();
            //Debug.Log("auto init profile");
        }
    }

    public void OnHideUnity(bool isGameShown)
    {

    }

    #region Share ScreenShot
    public IEnumerator ShareScreenRoutine(Texture2D texture, string message, Action<bool> callback)
    {
        yield return new WaitForSeconds(0.05f);
        byte[] screenshot = texture.EncodeToPNG();

        var wwwForm = new WWWForm();
        wwwForm.AddBinaryData("image", screenshot, "amanotes.png");
        wwwForm.AddField("message", message);

        FB.API("me/photos", HttpMethod.POST, obj =>
        {
            if (callback != null)
                callback(obj.Error == null);
            Debug.LogError("Upload Photo Result:" + obj.RawResult);
        }, wwwForm);
    }

    IEnumerator ShareFeedWithScreenShotWait(string message, Action<bool> callback, float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(TakeScreenshotToFeed(message, callback));
    }

    private IEnumerator TakeScreenshotToFeed(string message, Action<bool> callback)
    {
        yield return new WaitForEndOfFrame();

        var width = Screen.width;
        var height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        byte[] screenshot = tex.EncodeToPNG();

        var wwwForm = new WWWForm();
        wwwForm.AddBinaryData("image", screenshot, "amanotes.png");
        wwwForm.AddField("message", message);

        //chup hinh post
        FB.API("me/photos", HttpMethod.POST, obj =>
        {
            if (callback != null)
                callback(obj.Error == null);
            Debug.Log("Upload Photo Result:" + obj.RawResult);


        }, wwwForm);
    }

    public void ShareFeedWithScreenShot(string message, byte[] screenshot, Action<bool> callback, float time = 1)
    {
        StartCoroutine(ShareFeedWithScreenShotWait(message, screenshot, callback, 1));
    }

    private IEnumerator ShareFeedWithScreenShotWait(string message, byte[] screenshot, Action<bool> callback, float time)
    {
        yield return new WaitForSeconds(time);

        var wwwForm = new WWWForm();
        wwwForm.AddBinaryData("image", screenshot, "amanotes.png");
        wwwForm.AddField("message", message);
        //chup hinh post
        FB.API("me/photos", HttpMethod.POST, obj =>
        {
            if (callback != null)
            {
                callback(obj.Error == null);
            }
        }, wwwForm);
    }


    #endregion

    public string UserID
    {
        get
        {
            if (IsUserInfoAvailable)
            {
                return AccessToken.CurrentAccessToken.UserId;
            }
            else
            {
                return "";
            }
        }
    }
    public bool IsUserInfoAvailable
    {
        get; set;
    }

    public string AvatarURL
    {
        get; set;
    }

    public string UserDisplayName
    {
        get; set;
    }

    public string UserEmail
    {
        get; set;
    }

    public string CoverURL
    {
        get; set;
    }

    private List<string> m_friends;
    public List<string> Friends { get { return m_friends; } }
    public bool HasFriends { get { return m_friends != null && m_friends.Count > 0; } }

    //public void LoadFriends()
    //{
    //    if (m_friends != null)
    //    {
    //        m_friends = null;
    //    }
    //    FB.API("me/friends?fields=id,name", HttpMethod.GET, resGetInfo =>
    //    {
    //        if (resGetInfo.Cancelled || !string.IsNullOrEmpty(resGetInfo.Error))
    //        {
    //            Debug.LogError("Error:" + resGetInfo.Error.ToString());
    //            //MessageBus.Annouce(new Message(MessageBusType.FriendsLoaded));
    //            return;
    //        }

    //        try
    //        {
    //            m_friends = new List<string>();
    //            FBFriendQuery query = new FBFriendQuery();
    //            var stringData = resGetInfo.RawResult;
    //            // Debug.LogError(stringData);
    //            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(stringData);
    //            var extractData = dict["data"];
    //            string newString = JsonConvert.SerializeObject(extractData);
    //            query.data = JsonConvert.DeserializeObject<List<FBFriend>>(newString);
    //            if (query.data.Count > 0)
    //            {
    //                foreach (var item in query.data)
    //                {
    //                    m_friends.Add(item.id);
    //                }
    //            }
    //            // if (r != null && r.Count > 0)
    //            // {
    //            // 	for (int i = 0; i < query.data.Count; i++)
    //            // 	{
    //            // 		m_friends.Add(query.data[i].id);
    //            // 	}
    //            // }
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.LogError("Exception query friend facebook:" + ex.Message);
    //        }
    //    });
    //}
}
