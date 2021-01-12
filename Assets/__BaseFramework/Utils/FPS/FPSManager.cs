using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FPSManager : SingletonMono <FPSManager>
{
    // Start is called before the first frame update
    public static int graphicLevel = 2;
    void Start()
    {
        Application.targetFrameRate = 60;
        switch (DeviceProfile.GetDeviceProfile())
        {
            case ProfileRange.VeryHigh: graphicLevel = 4; break;
            case ProfileRange.High:     graphicLevel = 3; break;
            case ProfileRange.Mid:      graphicLevel = 2; break;
            case ProfileRange.Low:      graphicLevel = 1; break;
            case ProfileRange.VeryLow:  graphicLevel = 0; break;
            default: graphicLevel = 1; break;
        }
        UpdateSettingGraphic();
    }

    public static void UpdateSettingGraphic()
    {
        int value = graphicLevel;
        //if (GSystem.instance.userData.settingGraphic_Force != -1)
        //{
        //    isAutoDownGradeGraphicSetting = true;
        //    value = GSystem.instance.userData.settingGraphic_Force;
        //}
        Do_UpdateSettingGraphic(value);
    }
    static bool isAutoDownGradeGraphicSetting = false;
    //public static void UpdateSettingGraphic_AutoDownGrade()
    //{
    //    if (isAutoDownGradeGraphicSetting || GSystem.instance.userData.settingGraphic_Force != -1) return;
    //    isAutoDownGradeGraphicSetting = true;
    //    int value = GSystem.instance.userData.settingGraphic_AutoDetect - 1;
    //    value = Mathf.Max(0, value);
    //    GSystem.instance.userData.settingGraphic_AutoDetect = value;
    //    GSystem.instance.SaveUserData();
    //    Do_UpdateSettingGraphic(value);
    //}
    static void Do_UpdateSettingGraphic(int value)
    {
        float ratio = Screen.dpi / 300f;
        switch (value)
        {
            case 0: QualitySettings.resolutionScalingFixedDPIFactor = 0.7f * ratio; break;
            case 1: QualitySettings.resolutionScalingFixedDPIFactor = 0.8f * ratio; break;
            case 2: QualitySettings.resolutionScalingFixedDPIFactor = 0.9f * ratio; break;
            case 3: QualitySettings.resolutionScalingFixedDPIFactor = 1f * ratio; break;
            case 4: QualitySettings.resolutionScalingFixedDPIFactor = 1.1f * ratio; break;
        }
        //QualitySettings.resolutionScalingFixedDPIFactor = 1f * ratio;
        QualitySettings.antiAliasing = 2;
        QualitySettings.SetQualityLevel(QualitySettings.GetQualityLevel(), true);
        Instance.Invoke("Off", 0.1f);
    }
    void Off()
    {
        QualitySettings.antiAliasing = 0;
    }
}
