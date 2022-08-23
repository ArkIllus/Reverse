using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class AllSceneMusicManager : Singleton<AllSceneMusicManager>
{
    public float volume = 1f;
    //GUIStyle s1;
    //GUIStyle s2;
    AudioSource sound;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == GameData_SO.InitScene)
        {
            PlayBGM("Mantra");
        }
        else if(SceneManager.GetActiveScene().name == GameData_SO.AfterLoginScene)
        {
            PlayBGM("The Creation of Life");
        }
        else if (SceneManager.GetActiveScene().name == GameData_SO.Levels[0])
        {
            PlayBGM("Celestine Meowmie");
        }
        else if (SceneManager.GetActiveScene().name == GameData_SO.Levels[1])
        {
            PlayBGM("Universal Cycle");
            //PlayBGM_2("Universal Cycle");
        }
        else if (SceneManager.GetActiveScene().name == GameData_SO.Levels[2])
        {
            //PlayBGM("Gentle Fluttering Spirit");
            MusicManager.GetInstance().PlayBGM2("Gentle Fluttering Spirit");
        }
    }
    
    internal void PlayBGM(string bgmName)
    {
        MusicManager.GetInstance().PlayBGM(bgmName);
    }
    internal void PlayBGM(string bgmName, UnityAction action)
    {
        MusicManager.GetInstance().PlayBGM(bgmName, action);
    }
    internal void PlayBGM_2(string bgmName)
    {
        MusicManager.GetInstance().PlayBGM_2(bgmName);
    }
    internal void PlaySound(string soundName)
    {
        MusicManager.GetInstance().PlaySound(soundName, isLoop: false, (s) =>
        {
            sound = s;
        });
    }

    //private void OnGUI()
    //{
    //    //---BGM---
    //    if (GUI.Button(new Rect(0, 0, 100, 100), "Play BGM"))
    //    {
    //        MusicManager.GetInstance().PlayBGM("Unite In The Sky (short)");
    //        MusicManager.GetInstance().ChangeBGMVolume(0.1f);
    //    }

    //    if (GUI.Button(new Rect(0, 100, 100, 100), "Pause BGM"))
    //    {
    //        MusicManager.GetInstance().PauseBGM();
    //    }

    //    if (GUI.Button(new Rect(0, 200, 100, 100), "Stop BGM"))
    //    {
    //        MusicManager.GetInstance().StopBGM();
    //    }

    //    //volume = GUI.Slider(new Rect(0, 300, 100, 50), volume, 1, 0, 1, s1, s2, true, 0);
    //    //MusicManager.GetInstance().ChangeBGMVolume(volume);


    //    //---Sound---
    //    if (GUI.Button(new Rect(0, 300, 100, 100), "Play Sound"))
    //    {
    //        MusicManager.GetInstance().PlaySound("can", isLoop: false, (s) =>
    //        {
    //            sound = s;
    //        });
    //    }
    //    if (GUI.Button(new Rect(0, 400, 100, 100), "Stop Sound"))
    //    {
    //        MusicManager.GetInstance().StopSound(sound);
    //        sound = null;
    //    }
    //}
}
