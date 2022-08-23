using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Achievement_SO : ScriptableObject
{
    public int currentAmount;
    public int completeAmount;
    public bool isComplete;
    public string acName;
    public int acIndex;

    public virtual void UpdateMe(int add)
    {
        if (isComplete)
            return;
        currentAmount += add;
        if (currentAmount >= completeAmount)
        {
            isComplete = true;
            if (SceneManager.GetActiveScene().buildIndex != 0 ||
                SceneManager.GetActiveScene().buildIndex != 1) //ֻ����Ϸ�ڳ����ŵ����ɾ���ʾ
            {
                ShowTip();
            }
        }
    }

    //�����ɾ���ʾ
    public virtual void ShowTip()
    {
        AchievementTipCanvas.Instance.ShowMe(this);
        GameManager_global.GetInstance().gameData_SO.UpdateAchievements();
    }
}
