using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Audio;
//using GFramework;

public class SoundManagerBase : SingletonMono<SoundManagerBase>
{

    const string KEY_USER_SOUND = "sound";
    const string KEY_USER_MUSIC = "music";
    public AudioMixerGroup audioMixer_Music;
    public AudioMixerGroup audioMixer_SFX;
    Dictionary<int, AudioClip> soundsDic = new Dictionary<int, AudioClip>();
    AudioSource[] channels;
    public const int CHANNEL_MUSIC_1 = 0;
    public const int CHANNEL_MUSIC_2 = CHANNEL_MUSIC_1 + 1;
    public const int CHANNEL_MUSIC_3 = CHANNEL_MUSIC_2 + 1;
    public const int CHANNEL_SFX_1 = CHANNEL_MUSIC_3 + 1;
    public const int CHANNEL_SFX_2 = CHANNEL_SFX_1 + 1;
    public const int CHANNEL_SFX_3 = CHANNEL_SFX_2 + 1;
    public const int CHANNEL_SFX_4 = CHANNEL_SFX_3 + 1;
    public const int CHANNEL_SFX_5 = CHANNEL_SFX_4 + 1;
    public const int CHANNEL_SFX_6 = CHANNEL_SFX_5 + 1;
    public const int CHANNEL_SFX_7 = CHANNEL_SFX_6 + 1;
    public const int MAX_CHANNELS = CHANNEL_SFX_7 + 1;

    const int CHANNEL_SFX_START = CHANNEL_SFX_1;
    const int CHANNEL_SFX_END = CHANNEL_SFX_3;
    bool isInit = false;
    public bool isMusicOn = true;
    public bool isSoundOn = true;
    bool paused = false;
	public virtual void Load()
	{

	}
	public virtual void LoadSound( string[] soundNameArray)
    {
        if (channels == null)
        {
            channels = new AudioSource[MAX_CHANNELS];
            for (int i = 0; i < channels.Length; i++)
            {
                channels[i] = this.gameObject.AddComponent<AudioSource>();
                if( i < CHANNEL_SFX_1)
                    channels[i].outputAudioMixerGroup = ( audioMixer_Music) ;
                else
                    channels[i].outputAudioMixerGroup =  (audioMixer_SFX);
            }
            isInit = true;


            if (PlayerPrefs.HasKey(KEY_USER_MUSIC))
            {
                isMusicOn = PlayerPrefs.GetInt(KEY_USER_MUSIC) == 1;
                isSoundOn = PlayerPrefs.GetInt(KEY_USER_SOUND) == 1;
            }
            else
            {
                isMusicOn = true;
                isSoundOn = true;
                PlayerPrefs.SetInt(KEY_USER_MUSIC, 1);
                PlayerPrefs.SetInt(KEY_USER_SOUND, 1);
                PlayerPrefs.Save();
            }

            paused = false;
            int index = 0;
			foreach (string item in soundNameArray)
			{
				string clipName = item.ToString();
				AudioClip clip = Resources.Load<AudioClip>(clipName);
				soundsDic.Add(index, clip);
                index++;

            }

		}
    }



    void Update()
    {
        //if (isInit && !paused)
        //{
        //    if (!getChannel(CHANNEL_MUSIC).isPlaying)
        //    {
        //        //playThemeMusic();
        //    }
        //}

        //if(Input.GetKeyDown(KeyCode.M))
        //{
        //    resumeBGM();
        //}
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    stopBGM();
        //}

    }
    void OnApplicationFocus(bool focusStatus)
    {
        //paused = !focusStatus;
        //if (paused)
        //{
        //    pauseBGM();
        //    stopSFX();
        //}
        //else
        //{
        //    resumeBGM(CHANNEL_MUSIC);
        //    resumeBGM(CHANNEL_MUSIC_2);
        //}
    }
    public void Pause_All_BGM()
    {
        AudioSource audioChannel = getChannel(CHANNEL_MUSIC_1);
        if (audioChannel.isPlaying)
        {
            audioChannel.Pause();
        }
        audioChannel = getChannel(CHANNEL_MUSIC_2);
        if (audioChannel.isPlaying)
        {
            audioChannel.Pause();
        }
        audioChannel = getChannel(CHANNEL_MUSIC_3);
        if (audioChannel.isPlaying)
        {
            audioChannel.Pause();
        }
    }
    public void Resume_All_BGM(int channel)
    {
        AudioSource audioChannel = getChannel(channel);
        if (audioChannel.isPlaying)
        {
            audioChannel.Play();
        }
        audioChannel = getChannel(CHANNEL_MUSIC_2);
        if (audioChannel.isPlaying)
        {
            audioChannel.Play();
        }
        audioChannel = getChannel(CHANNEL_MUSIC_3);
        if (audioChannel.isPlaying)
        {
            audioChannel.Play();
        }
    }
    public void onOffMusic()
    {
        isMusicOn = !isMusicOn;
        PlayerPrefs.SetInt(KEY_USER_MUSIC, isMusicOn ? 1 : 0);
        PlayerPrefs.Save();
        if (!isMusicOn)
        {

            AudioSource audioChannel = getChannel(CHANNEL_MUSIC_1);
            vol1 = audioChannel.volume;
            if (audioChannel.isPlaying)
                StartCoroutine(SmoothDownVolumn(0.5f, CHANNEL_MUSIC_1, 0));

            audioChannel = getChannel(CHANNEL_MUSIC_2);
            vol2 = audioChannel.volume;
            if (audioChannel.isPlaying)
                StartCoroutine(SmoothDownVolumn(0.5f, CHANNEL_MUSIC_2, 0));

            audioChannel = getChannel(CHANNEL_MUSIC_3);
            vol3 = audioChannel.volume;
            if (audioChannel.isPlaying)
                StartCoroutine(SmoothDownVolumn(0.5f, CHANNEL_MUSIC_3, 0));
        }
        else
        {
            AudioSource audioChannel = getChannel(CHANNEL_MUSIC_1);
            if (audioChannel.isPlaying)
                StartCoroutine(IE_DimVolumn( CHANNEL_MUSIC_1, vol1 , 0.5f));


            audioChannel = getChannel(CHANNEL_MUSIC_2);
            if (audioChannel.isPlaying)
                StartCoroutine(IE_DimVolumn( CHANNEL_MUSIC_2, vol2, 0.5f));

            audioChannel = getChannel(CHANNEL_MUSIC_3);
            if (audioChannel.isPlaying)
                StartCoroutine(IE_DimVolumn( CHANNEL_MUSIC_3, vol3 , 0.5f));

        }
    }
    float vol1 = 1;
    float vol2 = 1;
    float vol3 = 1;
    public void onOffSound()
    {
        isSoundOn = !isSoundOn;
        PlayerPrefs.SetInt(KEY_USER_SOUND, isSoundOn ? 1 : 0);
        PlayerPrefs.Save();
        if (!isSoundOn)
        {
            stopSFX();

        }
        //onOffMusic();
    }
    public AudioSource getChannel(int channelID)
    {
        return channels[channelID];
    }
    void playAudioClip(int channel, AudioClip audioClip)
    {
        AudioSource audioChannel = getChannel(channel);
        audioChannel.Play();
    }
    public void stopBGM( int channel  ,float duration)
    {
        if (duration == 0)
        {
            AudioSource audioChannel = getChannel(channel);
            if (audioChannel.isPlaying)
            {
                audioChannel.Stop();
            }
        }
        else
        {
            StartCoroutine(StopBGM(duration, channel));
        }
    }
	public void PauseBGM(int channel)
	{
		AudioSource audioChannel = getChannel(channel);
		if (audioChannel.isPlaying)
			audioChannel.Pause();
	}
	public void ResumeBGM(int channel)
	{
		AudioSource audioChannel = getChannel(channel);
		if (!audioChannel.isPlaying)
			audioChannel.UnPause();
	}
	
	IEnumerator StopBGM(float duration, int channel = 0)
    {
        AudioSource audioChannel = getChannel(channel);
        if (audioChannel.isPlaying)
        {
            float volumn = audioChannel.volume;
            float cur = audioChannel.volume;
            float time = Time.time;
            while (cur > 0)
            {
                if (Time.timeScale > 0)

                    cur = Mathf.Lerp(volumn, 0, (Time.time - time) / duration);
                else
                    cur = Mathf.Lerp(volumn, 0, 1);

                audioChannel.volume = cur;
                //Debug.Log(string.Format("Channel:{0} ; Time:{1} ; Vol: {2}", channel, Time.time - time, cur));
                yield return new WaitForEndOfFrame();
            }
            audioChannel.Stop();
        }
        yield break;
    }
    public void stopSFX()
    {
        for (int i = CHANNEL_SFX_START; i <= CHANNEL_SFX_END; i++)
        {
            AudioSource audioChannel = getChannel(i);
            if (audioChannel.isPlaying)
            {
                audioChannel.Stop();
            }
        }
    }
    public void play(int id , bool priority = false, bool mutiPlay = true , bool OverLap = true , float volumn = 1 , float pitch = 1)
    {
        //Debug.Log("pitch111:::" + pitch);
        if (soundsDic.ContainsKey(id))
        {
            playAudio(soundsDic[id], false, false, priority, 0, mutiPlay, OverLap, volumn, pitch);
        }
    }
    public void playMusic(int id, bool loop = false)
    {
        if (soundsDic.ContainsKey(id))
            playAudio(soundsDic[id], true, loop);
    }
    public void playMusic(int id, float timeToMaxVolumn, int channel, float volumn = 1 , bool playFromBegin = true)
    {
        AudioSource audioChannel = getChannel(channel);
		if (playFromBegin)
			audioChannel.Stop();


		if (audioChannel.isPlaying && audioChannel.clip == soundsDic[id]) return;

        if (/*id != SFX.TOTAL && */soundsDic.ContainsKey(id))
            playAudio(soundsDic[id], true, true, false, channel);
        if (timeToMaxVolumn == 0)
        {
            SetVolumnLastChannelPlay(volumn);
        }
        else StartCoroutine(IE_DimVolumn( channel, volumn, timeToMaxVolumn));
    }
    public void DimVolumn(int channel, float targetVolumn, float timeToMaxVolumn)
	{
        StartCoroutine(IE_DimVolumn(channel, targetVolumn, timeToMaxVolumn));
	}
    IEnumerator IE_DimVolumn( int channel, float targetVolumn , float duration)
    {
        if (isMusicOn)
        {
            AudioSource audioChannel = getChannel(channel);
            audioChannel.volume = 0;
            float timeBegin = Time.time;
            float volFrom = audioChannel.volume;
            float vol = volFrom;
            while (vol < targetVolumn)
            {
                if (!isMusicOn)
                {
                    audioChannel.volume = 0;
                    yield break;
                }
                if (Time.timeScale > 0)
                    vol = Mathf.Lerp(volFrom, targetVolumn, (Time.time - timeBegin) / duration);
                else
                    vol = Mathf.Lerp(volFrom, targetVolumn, 1);

                audioChannel.volume = vol;
                yield return new WaitForEndOfFrame();
            }
        }
        yield break;
    }
    IEnumerator SmoothDownVolumn(float timeToMinVolumn, int channel, float targetVolumn)
    {
        //if (isSoundOn)
        //{
        AudioSource audioChannel = getChannel(channel);
        float time = Time.time;
        float vol = audioChannel.volume;
        float begin = audioChannel.volume;
        while (vol > targetVolumn)
        {
            if (isMusicOn)
            {
                //audioChannel.volume = targetVolumn;
                yield break;
            }
            if (Time.timeScale > 0)
                vol = Mathf.Lerp(begin, targetVolumn, (Time.time - time) / timeToMinVolumn);
            else
                vol = Mathf.Lerp(begin, targetVolumn, 1);

            audioChannel.volume = vol;
            yield return new WaitForEndOfFrame();
        }
        //}
        yield break;
    }
    int LastchannelPlay = 0;
    public void SetVolumnLastChannelPlay(float vol)
    {
        AudioSource audioChannel = getChannel(LastchannelPlay);
        audioChannel.volume = vol;
    }
    public void playAudio(AudioClip audioClip, bool isBGM = false, bool loop = false, bool priority = false, int channel = 0, bool mutiPlay = true, bool OverLap = true , float volumn =1 , float pitch = 1)
    {
        //Debug.Log(audioClip.name);
        if (isBGM)
        {
            //if (/*isMusicOn*/isSoundOn)
            {
                AudioSource audioChannel = getChannel(channel);
                if (audioChannel.isPlaying)
                {
                    audioChannel.Stop();
                }
                audioChannel.clip = audioClip;
                audioChannel.loop = loop;
                if (isMusicOn)
                    audioChannel.volume = 1;
                else
                    audioChannel.volume = 0;
				audioChannel.pitch = pitch;
				//audioChannel.outputAudioMixerGroup.audioMixer.SetFloat("Master.Pitch", 1f / pitch);
                audioChannel.Play();
                LastchannelPlay = channel;
            }
        }
        else if (isSoundOn)
        {
            if (!OverLap)
            {
                for (int i = CHANNEL_SFX_START + 1; i < MAX_CHANNELS; i++)
                {
                    AudioSource audioChannel = getChannel(i);
                    if (audioChannel.isPlaying && audioChannel.clip == audioClip)
                    {
                        return;
                    }
                }
            }
			//Debug.Log("pitch:::" + pitch);

			if (priority)
            {
                for (int i = CHANNEL_SFX_START + 1; i < MAX_CHANNELS; i++)
                {
                    AudioSource audioChannel = getChannel(i);
                    if (!audioChannel.isPlaying || (mutiPlay == false && audioChannel.clip == audioClip))
                    {
                        audioChannel.Stop();
                        audioChannel.loop = false;
                        audioChannel.clip = audioClip;
                        LastchannelPlay = i;
                        audioChannel.volume = volumn;
						//audioChannel.pitch = pitch;
                        audioChannel.outputAudioMixerGroup.audioMixer.SetFloat("Master.Pitch",  pitch);
                        audioChannel.Play();

                        return;
                    }
                }

                int j = Random.Range(CHANNEL_SFX_END + 1, MAX_CHANNELS);
                AudioSource audioChannel2 = getChannel(j);
                audioChannel2.Stop();
                audioChannel2.loop = false;
                audioChannel2.clip = audioClip;
                LastchannelPlay = j;
                audioChannel2.volume = volumn;
				//audioChannel2.pitch = pitch;
                audioChannel2.outputAudioMixerGroup.audioMixer.SetFloat("Master.Pitch",  pitch);
                audioChannel2.Play();

                return;
            }
            else
            {
                for (int i = CHANNEL_SFX_START; i <= CHANNEL_SFX_END; i++)
                {
                    AudioSource audioChannel = getChannel(i);
                    if (!audioChannel.isPlaying || (mutiPlay == false && audioChannel.clip == audioClip))
                    {
                        //Debug.Log("PlaySFX_" + audioClip.name);
                        audioChannel.clip = audioClip;
                        audioChannel.loop = false;
                        LastchannelPlay = i;
                        audioChannel.volume = volumn;
						//audioChannel.pitch = pitch;
                        audioChannel.outputAudioMixerGroup.audioMixer.SetFloat("Master.Pitch",  pitch);
                        audioChannel.Play();

                        return;
                    }
                }

                for (int i = CHANNEL_SFX_START; i <= CHANNEL_SFX_END; i++)
                {
                    AudioSource audioChannel = getChannel(i);
                    if (!audioChannel.isPlaying || audioChannel.clip == audioClip)
                    {
                        //Debug.Log("PlaySFX_" + audioClip.name);
                        audioChannel.clip = audioClip;
                        audioChannel.loop = false;
                        LastchannelPlay = i;
                        audioChannel.volume = volumn;
						//audioChannel.pitch = pitch;
                        audioChannel.outputAudioMixerGroup.audioMixer.SetFloat("Master.Pitch", pitch);
                        audioChannel.Play();

                        return;
                    }
                }
            }
        }
    }

    //public void ChangeSoundState()
    //{
    //    if ( isSoundOn )
    //    {
    //        isMusicOn = false;
    //        isSoundOn = false;
    //        PlayerPrefs.SetInt(KEY_USER_MUSIC, 0);
    //        PlayerPrefs.SetInt(KEY_USER_SOUND, 0);
    //    }
    //    else
    //    {
    //        isMusicOn = true;
    //        isSoundOn = true;
    //        PlayerPrefs.SetInt(KEY_USER_MUSIC, 1);
    //        PlayerPrefs.SetInt(KEY_USER_SOUND, 1);
    //    }
    //    PlayerPrefs.Save();
    //}

	
    static public void Pause_MBG_Action()
	{
		Instance.PauseBGM(CHANNEL_MUSIC_1);
	}
	static public void Resume_MBG_Action()
	{
		Instance.ResumeBGM(CHANNEL_MUSIC_1);
	}
    
}

