using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement_SO : ScriptableObject
{
    public int currentAmount;
    public int completeAmount;
    public bool isComplete;

    public virtual void UpdateMe(int add)
    {
        if (isComplete)
            return;
        currentAmount += add;
        if (currentAmount >= completeAmount)
        {
            isComplete = true;
        }
    }

    public void ShowTip()
    {

    }
}
