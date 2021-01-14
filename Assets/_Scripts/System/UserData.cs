using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : UserDataBase {

    public Action onCoinChanged;

    public bool isFirstTimeOpen;
    public bool isPlayingGame;

    public int money;
    public int Money {
        get { return money; }
        set {
            money = value;
            onCoinChanged?.Invoke();
        }
    }

    public int level;
    public int stage;

    public int[] itemIndices;

    public static int weaponCount = 15, shieldCount = 16, hatCount = 19, skinCount = 20 ;

    public int[][] itemsProgress;       // 0 locked, 1 unlocked


    public UserData() {
        onCoinChanged = null;
        isFirstTimeOpen = true;
        isPlayingGame = false;
        Money = 0;
        level = 1;
        stage = 1;
        itemIndices = new int[] { -1, -1, -1, -1 };

        itemsProgress = new int[4][] { new int[15], new int[16], new int[19], new int[24] };

        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < itemsProgress[i].Length; j++) {
                itemsProgress[i][j] = 1;
            }
        }
    }


}
