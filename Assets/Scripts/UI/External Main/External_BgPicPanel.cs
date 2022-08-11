using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class External_BgPicPanel : BasePanel
{
    public override void HideMe()
    {
        this.gameObject.SetActive(false);
    }
}
