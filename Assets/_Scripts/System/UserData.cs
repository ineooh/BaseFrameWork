using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : UserDataBase
{

    public Action onCoinChanged = null;

    public bool isFirstTimeOpen = true;
    public bool isPlayingGame = false;

    public int _coin = 100;

    public int level = 1;
    public int star = 0;


    public UserData()
    {

    }

    public int coin
    {
        get
        {
            return _coin;
        }

        set
        {
            _coin = value;
            onCoinChanged?.Invoke();
        }
    }


}
