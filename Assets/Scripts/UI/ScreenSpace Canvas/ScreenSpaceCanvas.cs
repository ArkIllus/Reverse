using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpaceCanvas : Singleton<ScreenSpaceCanvas>
{
    protected override void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); //optional
        }
    }
}
