using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using Amanotes.Utils;
//using GFramework.ABTest;
//using GFramework.Config;

[RequireComponent(typeof(GlobalData))]
public class GameInitialManager : SingletonMono<GameInitialManager>
{
    [SerializeField]
    private TextAsset settingJson;

    // [SerializeField]
    //private GameObject gmCommand;

    private InitialSettingData initSettingData=null;
    private bool errorConfig = false;
    public InitialSettingData InitSettingData{
        get { return initSettingData; }
    }

    [HideInInspector]
    public bool isInitial = false;
    [HideInInspector]
    public bool isInitialIAP = false;

    public void Start()
    {
        if (settingJson == null)
        {
            errorConfig = true;
        }
        else
        {
            try
            {
                initSettingData = JsonUtility.FromJson<InitialSettingData>(settingJson.text);
                if (initSettingData == null)
                {
                    errorConfig = true;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Ama GameInitialManager Exception:" + ex.Message);
                errorConfig = true;

            }
        }

        DoOnMainThread.Instance.Initial();
        //Initial Firebase
        FirebaseManager.Instance.Init();
        //Init Facebook
        FacebookManager.Instance.Init();
        //Init Config

        //if (ContentToolManager.instance && ContentToolManager.instance.isEnableContentTool)
        //{
        //    ConfigManagerBase.instance.isUsingCloud = false;
        //    ABTestManagerBase.instance.isEnabled = false;
        //}

        //StartCoroutine(ConfigManager.instance.InitConfig());


        //Initial remain game services
        StartCoroutine(RoutineInitService());
        //if (gmCommand != null)
        //{
        //    gmCommand.SetActive(false);
        //}

        isInitial = true;
    }

    private void OnEnable(){
        FirebaseManager.onRemoteConfigFetchComplete+=OnRemoteConfigFetchComplete;
        FirebaseManager.onCheckFirebaseDependencyFinish+=CheckFirebaseDependencyFinish;
        
    }
    private void OnDisable(){
        FirebaseManager.onRemoteConfigFetchComplete-=OnRemoteConfigFetchComplete;
        FirebaseManager.onCheckFirebaseDependencyFinish-=CheckFirebaseDependencyFinish;
    }

    private IEnumerator RoutineInitService(){
        //waiting for other service awake
        yield return new WaitForSeconds(0.1f);

        if (errorConfig) {
            ShowLog("Cannot Initial any Service dueto Error setting Json, please drag BuildGame/BuildSetting.json to GameInitialManager->settingJson");
        }
        else
        {
            InitialService(); 
        }

    }

    public void ShowGM(bool isShow){
        //if(gmCommand!=null){
        //    gmCommand.SetActive(isShow);
        //}
    }
    private void InitialService()
    {
        ShowLog("GameInitialManager:InitialService");
        Development.SetActiveLog(initSettingData.enable_log_analytics);
        //ShowGM(initSettingData.enable_gm_tool);
        //init ads    
        string keyIronsource = "";
        
#if UNITY_ANDROID
        keyIronsource = InitSettingData.ironsrc_android_key;
#elif UNITY_IOS
        keyIronsource = InitSettingData.ironsrc_ios_key;
#endif
        //AdHelper.Instance.Initialize(keyIronsource, UserPropertiesTracker.Instance);

        //init appsflyer
        //AppsFlyerService.Instance.Init();

        // init iap
        //InitIAPService();

        // initial analtics parameter -> user properties...
        ShowLog("Finish Initilal all third party services");
    }

    private void CheckFirebaseDependencyFinish(bool result){
        if(result){
            //Set default value for Firebase Remote Config
            SetDefaultValueForRemoteConfig();
        }
    }
#region Remote Config Fetch Result
    private void SetDefaultValueForRemoteConfig(){
        Development.Log("GameInitialManager->SetDefaultValueForRemoteConfig",LogType.Warning);
        Dictionary<string,object> defaults = new Dictionary<string,object>();

        //this is just test -> replace by your game
        defaults.Add("ama_test_value", "default value");
        defaults.Add("default_max_games_between_interstitial_ads", 1);
        defaults.Add("delay_in_seconds_between_interstitial_ads", 2.0);
        defaults.Add("variable_not_setup_in_remote_config", "not setup variable");

        defaults.Add("delayBeforeFirstInterstitial", 60);
        defaults.Add("delayBetweenInterstitial", 45);
        defaults.Add("delayBetweenRewardedVieoAndInterstitial", 45);
        defaults.Add("maxSessionBetweenInterstitial", 3);

        Firebase.RemoteConfig.FirebaseRemoteConfig.SetDefaults(defaults);
    }
#endregion
#region Remote Config Fetch Result
    private void OnRemoteConfigFetchComplete(bool result){
        Development.Log("GameInitialManager->OnRemoteConfigFetchComplete:"+result,LogType.Warning);

        
        //Init Firebase AB Test after Init Firebase
        //ABTestManagerBase.Instance.Initialize();
        //AdHelper.Instance.InitAdsConfig();
    }

#endregion

#region Initial IAP Item
    /*
    private void InitIAPService()
    {
        IAPService.Instance.Init(  InitSettingData.consumable_product_ids
                                 , InitSettingData.nonconsumable_product_ids
                                 , InitSettingData.subcription_product_ids
                                       , (onComplete) =>
       {
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            if (InitSettingData.consumable_product_ids != null)
            {
                count1 = InitSettingData.consumable_product_ids.Count;
            }
            if (InitSettingData.nonconsumable_product_ids != null)
            {
                count2 = InitSettingData.nonconsumable_product_ids.Count;
            }
            if (InitSettingData.subcription_product_ids != null)
            {
                count3 = InitSettingData.subcription_product_ids.Count;
            }

            ShowLog("InitIAPService:" + onComplete + ", comsumable=" + count1 + ", non-comsumable=" + count2 + ", subcription=" + count3);
           isInitialIAP = true;
       });
    }
    */
#endregion

#region Log Util
    public void ShowLog(string log){
        if(InitSettingData!=null&&InitSettingData.enable_log_analytics){
            Debug.LogWarning("GameInitialManager->"+log);
        }
    }

    public bool IsCanShowLog{
        get{
            if(InitSettingData==null){
                return true;
            }
            else if(InitSettingData!=null&&InitSettingData.enable_log_analytics){
                return true;
            }
            return false;
        }
    }
#endregion
}

