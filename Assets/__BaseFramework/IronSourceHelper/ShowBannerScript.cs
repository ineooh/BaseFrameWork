using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBannerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
		IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
		IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
		IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
		IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
		IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;
		IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
	}

	//Banner Events
	void BannerAdLoadedEvent()
	{
		Debug.Log("unity-script: I got BannerAdLoadedEvent");
	}

	void BannerAdLoadFailedEvent(IronSourceError error)
	{
		Debug.Log("unity-script: I got BannerAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
	}

	void BannerAdClickedEvent()
	{
		Debug.Log("unity-script: I got BannerAdClickedEvent");
	}

	void BannerAdScreenPresentedEvent()
	{
		Debug.Log("unity-script: I got BannerAdScreenPresentedEvent");
	}

	void BannerAdScreenDismissedEvent()
	{
		Debug.Log("unity-script: I got BannerAdScreenDismissedEvent");
	}

	void BannerAdLeftApplicationEvent()
	{
		Debug.Log("unity-script: I got BannerAdLeftApplicationEvent");
	}
}
