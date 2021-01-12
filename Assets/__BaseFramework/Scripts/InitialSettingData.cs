using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[Serializable]
public class InitialSettingData
{
    public string ironsrc_android_key = "";      // iron src Android ID
    public string ironsrc_ios_key = "";          // iron src IOS ID
    public bool is_banner_active = true;
    public bool is_intertitial_reward_active = true;

    public string appsflyer_dev_key = "";        // appsflyer Dev Key (use both for Android/IOS) -> provide PO
    public string appsflyer_ios_itune_app_id = "";// itune app id -> Provide by PO

    public bool enable_appsflyer = true;         // enable appsflyer SDK services
    public bool enable_log_analytics = true;    // enable log analytics for tracking, debugger
    public string ios_team_developer_id = "";

    public string admobID = "";

    public List<string> consumable_product_ids;  // list item iap consumable - can buy many time(buy ruby, buy life, buy item...)
    public List<string> nonconsumable_product_ids;// list item iap non consumable -buy one time (remove ads, vip pass...)
    public List<string> subcription_product_ids; // list item iap subcription (1week, 1 month, 1year...) 

    public string termsPrivacyUrl;
    public string aboutUsUrl;
}
