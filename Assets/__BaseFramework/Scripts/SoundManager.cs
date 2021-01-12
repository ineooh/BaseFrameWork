using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SFX
{
    mbg_menu,
    btn_click,
    heartbeat,
    hit,
    lose_1,
    lose_2,
    matching,
    mbg_ap,
    star,
    win_1,
    win_2,
    win_3,
    wrong,
    startgame,
    bang,
    coin1,
    coin2,
    TOTAL
}

public class SoundManager : SoundManagerBase
{
    public override void Load()
    {
        SFX[] list = (SFX[])System.Enum.GetValues(typeof(SFX));
        string[] soundNameArray = new string[list.Length];
        int count = 0;
        foreach (SFX item in (SFX[])System.Enum.GetValues(typeof(SFX)))
        {
            string clipName = item.ToString();
            soundNameArray[count] = clipName;
            count++;
        }
        LoadSound(soundNameArray);
    }
    public override void LoadSound( string [] soundNameArray)
    {
        base.LoadSound(soundNameArray);
    }
	static public void PlayMusic(bool play)
	{
		//return;
		if (play)
			Instance.playMusic((int)SFX.mbg_ap, 1f, CHANNEL_MUSIC_1, 1f);
		else
			Instance.stopBGM(CHANNEL_MUSIC_1, 0.5f);
	}
    static public void MusicBG_UP()
    {
        Instance.DimVolumn(CHANNEL_MUSIC_1, 1f, 1f);
    }
    static public void MusicBG_DOWN()
    {
        Instance.DimVolumn(CHANNEL_MUSIC_1, 0.4f, 1f);
    }

    static public void Play_SFX(SFX sound)
    {
        Instance.play( (int)sound, true, true, true, 1);
    }
    static public void Play_btnClick()
    {
        Instance.play((int)SFX.btn_click, true, false, true, 1);
    }
  
    static public void Play_GetHit()
    {
        Instance.play((int)SFX.hit, true, false, true, 1);
    }
    static public void Play_Die()
    {
        Instance.play((int)SFX.heartbeat, true, false, true, 1);
    }
    static public void Play_StartGame()
    {
        Instance.play((int)SFX.startgame, true, false, true, 1);
    }
    static public void Play_Bang()
    {
        Instance.play((int)SFX.bang, true, false, true, 1);
    }
    static public void Play_Coin1()
    {
        Instance.play((int)SFX.coin1, false, false, true, 0.5f);
    }
    static public void Play_Coin2()
    {
        //Instance.play((int)SFX.coin2, true, false, true, 1);
        if (Time.realtimeSinceStartup - time_star < 0.2f)
            pitch_star += 0.01f;
        else
            pitch_star = 1;
        time_star = Time.realtimeSinceStartup;
        //Debug.Log(" pitch_star:" + pitch_star);
        float pitch = pitch_star;
        Instance?.play((int)SFX.coin2, false, false, true, .5f, pitch);
    }
    public static float time_star = 0;
    public static float pitch_star = 1;
    static public void Play_Star()
    {
        if (Time.realtimeSinceStartup - time_star < 0.2f)
            pitch_star += 0.015f;
        else
            pitch_star = 1;
        time_star = Time.realtimeSinceStartup;
        //Debug.Log(" pitch_star:" + pitch_star);
        float pitch = pitch_star;
        Instance?.play((int)SFX.star, true, false, true, .8f , pitch);
    }

    static public void Play_Win()
    {
        Instance.play( Random.Range( (int)SFX.win_1 , (int)SFX.win_3 + 1), true, true, true, 1);
    }
}
