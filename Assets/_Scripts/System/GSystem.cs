using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSystem : SingletonMonoPersistent<GSystem>
{
    //public static string userDataKey = "UserData_colorball";

    //public override void Start()
    //{
    //	_userDataKey = userDataKey;
    //	LoadUserData();

    //	StartCoroutine(IEnumerator_EntryPoint());

    //}

    //public override IEnumerator IEnumerator_EntryPoint()
    //{
    //	//Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");

    //	StartCoroutine(base.IEnumerator_EntryPoint());
    //	//loading
    //	yield return new WaitUntil(() => isInitGameService);
    //	GUIManager.Instance.ShowGUI(GUIName.UIMainMenuHandler);
    //	//do your logic here
    //	//Debug.Log("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
    //}

    public static UserData userData;

    public override void Awake() {
        base.Awake();
        LoadUserData();
    }

    private void Start() {
        
    }

    public static void LoadUserData() {
        userData = UserData.LoadData<UserData>("UserData");
        if (userData == null) {
            userData = new UserData();
        }
    }

    public static void SaveUserData() {
        if (userData != null) {
            userData.SaveData<UserData>("UserData");
        }
    }

}


