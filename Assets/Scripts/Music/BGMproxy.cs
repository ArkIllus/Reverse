using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMproxy : Singleton<BGMproxy>
{
    //BGM
    public AudioSource bgm;

    protected override void Awake()
    {
        base.Awake();
        MusicManager.GetInstance().bgm = bgm;
    }

    private void OnDisable()
    {
        MusicManager.GetInstance().bgm = null;
    }
}
