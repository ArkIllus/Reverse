using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicManager : BaseManager<MusicManager>
{
    //文件路径
    public static string path_BGM = "Music/BGM/";
    public static string path_Sound = "Music/Sound/";

    //BGM
    private AudioSource bgm = null;
    private float bgmVolume = 1;

    //其他所有音效
    //[注]目前音效都挂在一个soundObj下，而不是发出声音的GameObject下，
    //没有距离区分，不适合3D游戏
    private GameObject soundObj = null;
    private List<AudioSource> soundList = new List<AudioSource>();
    private float soundVolume = 1;

    public MusicManager()
    {
        MonoManager.GetInstance().AddUpdateListener(Update);
    }

    //当一个AudioSource播放完了，把它移除掉
    //[注]Unity不会自动移除播放完的AudioSource，而且没有提供一个AudioSource播放完执行的回调函数，
    //所以只能在Update里检测它是否播放完了（？）
    private void Update()
    {
        for(int i = soundList.Count - 1; i >= 0; --i)
        {
            if (!soundList[i].isPlaying)
            {
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name"></param>
    public void PlayBGM(string name)
    {
        if (bgm == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BGM";
            bgm = obj.AddComponent<AudioSource>();
        }
        //异步加载BGM，加载完成后 播放
        ResourceManager.GetInstance().LoadAsync<AudioClip>(path_BGM + name, (clip) =>
        {
            bgm.clip = clip;
            bgm.volume = bgmVolume;
            bgm.loop = true; //默认循环播放
            bgm.Play();
        });
    }
    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBGM()
    {
        if (bgm == null)
            return;
        bgm.Pause();
    }
    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBGM()
    {
        if (bgm == null)
            return;
        bgm.Stop();
    }
    /// <summary>
    /// 改变背景音乐 音量大小
    /// </summary>
    /// <param name="vol"></param>
    public void ChangeBGMVolume(float vol)
    {
        bgmVolume = vol;
        if (bgm == null)
            return;
        bgm.volume = bgmVolume;
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callBack">用于把这个sound传出去</param>
    public void PlaySound(string name, bool isLoop, UnityAction<AudioSource> callBack = null)
    {
        if (soundObj == null)
        {
            soundObj = new GameObject();
            soundObj.name = "Sound";
        }
        //异步加载音效资源，加载完成后 添加 播放
        ResourceManager.GetInstance().LoadAsync<AudioClip>(path_Sound + name, (clip) =>
        {
            AudioSource sound = soundObj.AddComponent<AudioSource>();
            sound.clip = clip;
            sound.loop = isLoop; //默认不循环播放
            sound.volume = bgmVolume;
            sound.Play();
            soundList.Add(sound);

            if (callBack != null)
                callBack(sound);
        });
    }
    /// <summary>
    /// 停止音效
    /// </summary>
    /// <param name="name"></param>
    public void StopSound(AudioSource audioSource)
    {
        if (soundList.Contains(audioSource))
        {
            soundList.Remove(audioSource);
            audioSource.Stop();
            GameObject.Destroy(audioSource); //TODO:优化 缓存池
        }
    }
    /// <summary>
    /// 改变（所有）音效 音量大小
    /// </summary>
    /// <param name="vol"></param>
    public void ChangeSoundVolume(float vol)
    {
        soundVolume = vol;
        foreach (var sound in soundList)
        {
            sound.volume = vol;
        }
    }
}
