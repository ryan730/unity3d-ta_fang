using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 声音管理器

public class AudioManager
{
    // 声明声音的路径
    public static string MusicPath = "audio/bg/";
    public static string SoundPath = "audio/sound/";
    // 背景音乐的开关
    public static bool mIsMusicOn = true;
    // 背景声音的开关
    public static bool mIsSoundOn = true;

    // initData 初始化
    public static void initData()
    {
        // mIsMusicOn = (PlayerPrefs.GetInt("is_music_on", 1) == 1);
        // mIsSoundOn = (PlayerPrefs.GetInt("is_sound_on", 1) == 1);
    }

    //设置音乐开关
    public static void setMusicOn(bool isOn)
    {
        mIsMusicOn = isOn;
        PlayerPrefs.SetInt("is_music_on", isOn ? 1 : 0);
        // 通过 isOn 判断是否播放音乐
        if (isOn)
        {
            if (_as_music && !_as_music.isPlaying)
            {
                _as_music.Play();
            }
        }
        else
        {
            if (_as_music && _as_music.isPlaying)
            {
                _as_music.Stop();
            }
        }
    }

    //设置声音开关
    public static void setSoundOn(bool isOn)
    {
        mIsSoundOn = isOn;
        PlayerPrefs.SetInt("is_sound_on", isOn ? 1 : 0);
        for (int i = 0; i < ASList.Count; i++)
        {
            if (ASList[i].isPlaying && isOn == false)
            {
                ASList[i].Stop();
            }
        }
    }

    // 声音源头
    private static AudioSource _as_music = null;
    // 播放声音源
    public static void PlayMusic(string path)
    {
        if (_as_music == null)
        {
            // 给 camera 上添加声音源
            _as_music = Camera.main.gameObject.AddComponent<AudioSource>();
        }
        if (!mIsMusicOn)
        {
            return;
        }

        Debug.Log("MusicPath+path:" + MusicPath + path);
        // 从资源库加载声音
        _as_music.clip = Resources.Load<AudioClip>(MusicPath + path);
        //循环
        _as_music.loop = true;
        // 开始播放
        _as_music.Play();

    }

    // 初始化声音源，列表类型
    public static List<AudioSource> ASList = new List<AudioSource>();
    // 播放声音源
    public static void PlaySound(string path)
    {
        if(!mIsSoundOn){
            return;
        }
        AudioSource _as_sound = null;
        for (int i = 0; i < ASList.Count; i++)
        {
            if (ASList[i].isPlaying == false)
            {
                _as_sound = ASList[i];
                break;
            }
        }
        if (_as_sound == null)
        {
            // 给 camera 上添加声音源
            _as_sound = Camera.main.gameObject.AddComponent<AudioSource>();
        }

        // 从资源库加载声音
        _as_sound.clip = Resources.Load<AudioClip>(SoundPath + path);
        //循环
        _as_sound.loop = false;
        // 开始播放
        _as_sound.Play();

    }
}
