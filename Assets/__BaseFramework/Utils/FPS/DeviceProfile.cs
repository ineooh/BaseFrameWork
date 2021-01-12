using Softdrink;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public enum ProfileRange
{
    VeryLow = 0,
    Low,
    Mid,
    High,
    VeryHigh,
    Unknow
}
public class DeviceProfile
{
    public static ProfileRange GetDeviceProfile()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return GetDeviceProfileForiOS();
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            return GetDeviceProfileForAndroid();
        }

        return ProfileRange.Unknow;
    }

    private static ProfileRange GetDeviceProfileForiOS()
    {
#if UNITY_IOS
        if (Device.generation >= DeviceGeneration.iPhoneX)
        {
            return ProfileRange.VeryHigh;
        }

        if (Device.generation >= DeviceGeneration.iPhone7)
        {
            return ProfileRange.High;
        }

        if (Device.generation >= DeviceGeneration.iPhone6S)
        {
            return ProfileRange.Mid;
        }

        if (Device.generation >= DeviceGeneration.iPhone5)
        {
            return ProfileRange.Low;
        }
#endif
        return ProfileRange.High;
    }

    private static ProfileRange GetDeviceProfileForAndroid()
    {
        HardwareInfo.Instance.CalculateHardwareScore();
        var score = HardwareInfo.Instance.userHardwareScore;
        return GetDeviceProfileByScore(score);
    }

    public static ProfileRange GetDeviceProfileByScore(float score)
    {
        if (score >= 65) return ProfileRange.VeryHigh;
        if (score >= 50) return ProfileRange.High;
        if (score >= 35) return ProfileRange.Mid;
        if (score >= 20) return ProfileRange.Low;
        if (score < 20) return  ProfileRange.VeryLow;
        return ProfileRange.Mid;
    }
}
