
using UnityEngine;
//using GFramework;
public class UserDataBase : MonoBehaviour
{
    private static string saveName = "StorageData";

    /// <summary>
    /// Convert this class to JSon string
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public string ToJSonString<T>() where T : UserDataBase
    {
        return JsonUtility.ToJson(this);
    }

    /// <summary>
    /// Convert JSon string to this class structure
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T FromJSonString<T>(string json) where T : UserDataBase
    {
        return JsonUtility.FromJson<T>(json);
    }

    /// <summary>
    /// Save data store on this class to device with specific name provided by parametter "name"
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    public void SaveData<T>(string name) where T : UserDataBase
    {
        string sJSonData = ToJSonString<T>();
        //Debug.LogError("sJSonData Save: " + sJSonData);
        PlayerPrefs.SetString(name, sJSonData);
    }

    /// <summary>
    /// Load data store on the device to this class structure with specific name provided by parametter "name"
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T LoadData<T>(string name) where T : UserDataBase
    {
        string sJSonData = "";
        if (PlayerPrefs.HasKey(name))
        {
            sJSonData = PlayerPrefs.GetString(name);
            //Debug.LogError("sJSonData Load: " + sJSonData);
            return UserDataBase.FromJSonString<T>(sJSonData);
        }

        return null;
    }

    /// <summary>
    /// Save data store on this class to device with default name
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void SaveData<T>() where T : UserDataBase
    {
        SaveData<T>(saveName);
    }

    /// <summary>
    /// Load data store on the device to this class structure with default name
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T LoadData<T>() where T : UserDataBase
    {
        return LoadData<T>(saveName);
    }


}