using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GSystemBase : SingletonMonoPersistent<GSystemBase>
{
    public static UserDataBase userData;
    public static string _userDataKey = "UserData_[YourGameName]";
    public static bool isInitGameService = false;
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
    }
    public virtual void Start()
    {
        LoadUserData();
        StartCoroutine(IEnumerator_EntryPoint());
    }

    public virtual IEnumerator IEnumerator_EntryPoint()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => GameInitialManager.Instance.isInitial);
        isInitGameService = true;
    }

    public static void LoadUserData()
    {
        userData = UserDataBase.LoadData<UserDataBase>(_userDataKey);
        if (userData == null)
        {
            userData = new UserDataBase();
        }
    }

    public static void SaveUserData()
    {
        if (userData != null)
        {
            userData.SaveData<UserDataBase>(_userDataKey);
        }
    }

}
