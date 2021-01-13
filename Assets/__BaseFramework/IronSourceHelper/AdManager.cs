using UnityEngine;
using System.Collections;

public class AdManager : SingletonMono<AdManager>
{

	public ShowInterstitialScript interstitialScript;
	public ShowRewardedVideoScript rewardedVideoScript;
	public static string uniqueUserId = "demoUserUnity";

	// Use this for initialization
	void Start ()
	{
		#if UNITY_ANDROID
        string appKey = "e6ec9e39";
		#elif UNITY_IPHONE
        string appKey = "8545d445";
		#else
        string appKey = "unexpected_platform";
		#endif
		Debug.Log ("unity-script: MyAppStart Start called");

		//Dynamic config example
		IronSourceConfig.Instance.setClientSideCallbacks (true);

		string id = IronSource.Agent.getAdvertiserId ();
		Debug.Log ("unity-script: IronSource.Agent.getAdvertiserId : " + id);
		
	
		Debug.Log ("unity-script: unity version" + IronSource.unityVersion ());

		

		// SDK init
		Debug.Log ("unity-script: IronSource.Agent.init");
		IronSource.Agent.init(appKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.OFFERWALL, IronSourceAdUnits.BANNER);

		//IronSource.Agent.init (appKey);
		//IronSource.Agent.init(appKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.OFFERWALL, IronSourceAdUnits.BANNER);
		//IronSource.Agent.initISDemandOnly(appKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL);

		//Set User ID For Server To Server Integration
		//// IronSource.Agent.setUserId ("UserId");

	

		Debug.Log("unity-script: IronSource.Agent.validateIntegration");
		IronSource.Agent.validateIntegration();

	}

	void OnApplicationPause (bool isPaused)
	{
		Debug.Log ("unity-script: OnApplicationPause = " + isPaused);
		IronSource.Agent.onApplicationPause (isPaused);
	}

	public static bool Interstitial_IsReady()
	{
		return Instance.interstitialScript.IsReady();
	}
	public static void Interstitial_Show()
	{
		Instance.interstitialScript.ShowInterstitialButtonClicked();
	}

	public static bool Video_IsReady()
	{
		return Instance.rewardedVideoScript.IsReady();
	}
	public static void Video_Show()
	{
		Instance.rewardedVideoScript.ShowRewardedVideoButtonClicked();
	}
}
