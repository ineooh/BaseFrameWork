using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GSystemBase : SingletonMonoPersistent<GSystemBase>
{
    public static UserData userData;
    public static string _userDataKey = "UserData_[YourProjectName]";
    public static bool isInitGameService = false;
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        LoadUserData();
    }
    public virtual void Start()
    {
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
        userData = UserDataBase.LoadData<UserData>(_userDataKey);
        if (userData == null)
        {
            userData = new UserData();
        }
    }

    public static void SaveUserData()
    {
        if (userData != null)
        {
            userData.SaveData<UserData>(_userDataKey);
        }

    }

}
