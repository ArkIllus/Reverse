using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using System.IO;
using Minipb;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "GameData", menuName = "new GameData_SO")]
public class GameData_SO : ScriptableObject
{
    public TcpClient client = new TcpClient();

    //关卡名称
    public static string InitScene = "InitScene";
    public static string AfterLoginScene = "AfterLoginScene";
    public static string[] Levels = { "Level1", "Level2", "Level3" };

    [Header("用户数据")]
    public string username_memo;
    public string password_memo;
    public int uid;
    public int nameid; //唯一id
    public int bio; //签名

    [Header("关卡记录")]
    public List<LevelRecord> levelRecords = new List<LevelRecord>();
    public int lastLevel = -1;

    [Header("游戏数据(成就相关)")]
    public Ach_1_firstmeet ach_1_Firstmeet;
    public Ach_2_overload ach_2_Overload;
    public Ach_3_pass ach_3_Pass;
    public Ach_4_firstcake ach_4_Firstcake;
    public Ach_5_allcake ach_5_Allcake;
    public Ach_6_firstEnchant ach_6_FirstEnchant;

    [Header("能力解锁情况")]
    public bool gainReverse;

    public void MakeTcpConnect()
    {
        client.MakeTcpConnect("192.168.7.230", 9092, uid);
    }
    public void CloseTcpConnect()
    {
        client.Close();
    }

    public void ClearAllData()
    {
        for (int i = 0; i < levelRecords.Count; i++)
        {
            levelRecords[i].rebirth_pos = Vector3.zero;
            levelRecords[i].rebirth_Reverse = false;
        }

        ach_1_Firstmeet.currentAmount = 0;
        ach_1_Firstmeet.isComplete = false;

        ach_2_Overload.currentAmount = 0;
        ach_2_Overload.isComplete = false;

        ach_3_Pass.currentAmount = 0;
        ach_3_Pass.isComplete = false;

        ach_4_Firstcake.currentAmount = 0;
        ach_4_Firstcake.isComplete = false;

        ach_5_Allcake.currentAmount = 0;
        ach_5_Allcake.isComplete = false;

        ach_6_FirstEnchant.currentAmount = 0;
        ach_6_FirstEnchant.isComplete = false;

        lastLevel = -1;

        gainReverse = false;
    }
    #region 获取数据

    //从服务器更新所有需要的数据
    public void GetAllDataFromServer()
    {
        GetLevelRecords();
        GetAchievements();
        GetLastLevel();
        GetGainReverse();
    }

    public void GetLevelRecords()
    {
        HurdleData hdata = client.GetHurdleData(uid);

        for (int i = 0; i < levelRecords.Count; i++)
        {
            levelRecords[i].rebirth_pos.x = hdata.Hurdle[i].X;
            levelRecords[i].rebirth_pos.y = hdata.Hurdle[i].Y;
            levelRecords[i].rebirth_pos.z = hdata.Hurdle[i].Z;
            levelRecords[i].rebirth_Reverse = hdata.Hurdle[i].Reverse;
            //TODO 通关时间
        }
    }

    public void GetAchievements()
    {
        AchievementData adata = client.GetAchievementData(uid);

        ach_1_Firstmeet.currentAmount = adata.FirstMeet;
        ach_1_Firstmeet.isComplete = (ach_1_Firstmeet.currentAmount == ach_2_Overload.completeAmount);

        ach_2_Overload.currentAmount = adata.Overload;
        ach_2_Overload.isComplete = (ach_2_Overload.currentAmount == ach_2_Overload.completeAmount);

        ach_3_Pass.currentAmount = adata.Pass;
        ach_3_Pass.isComplete = (ach_3_Pass.currentAmount == ach_3_Pass.completeAmount);

        ach_4_Firstcake.currentAmount = (adata.Cake == 0) ? 0 : 1;
        ach_4_Firstcake.isComplete = (ach_4_Firstcake.currentAmount == ach_4_Firstcake.completeAmount);

        ach_5_Allcake.currentAmount = adata.Cake;
        ach_5_Allcake.isComplete = (ach_5_Allcake.currentAmount == ach_5_Allcake.completeAmount);

        ach_6_FirstEnchant.currentAmount = adata.FirstMagic;
        ach_6_FirstEnchant.isComplete = (ach_6_FirstEnchant.currentAmount == ach_6_FirstEnchant.completeAmount);
    }

    //TODO
    public void GetLastLevel()
    {
    }
    public void GetGainReverse()
    {
    }
    #endregion

    #region 上传数据
    public void UpdateLevelRecords()
    {
        //TODO 通关时间
        OneHurdleData level1 = TcpMessage.InitOneHurdleData(levelRecords[0].rebirth_pos.x, levelRecords[0].rebirth_pos.y, levelRecords[0].rebirth_pos.z, levelRecords[0].rebirth_Reverse, 9999);
        OneHurdleData level2 = TcpMessage.InitOneHurdleData(levelRecords[1].rebirth_pos.x, levelRecords[1].rebirth_pos.y, levelRecords[1].rebirth_pos.z, levelRecords[1].rebirth_Reverse, 9999);
        OneHurdleData level3 = TcpMessage.InitOneHurdleData(levelRecords[2].rebirth_pos.x, levelRecords[2].rebirth_pos.y, levelRecords[2].rebirth_pos.z, levelRecords[2].rebirth_Reverse, 9999);
        client.UpdateHurdleData(uid, level1, level2, level3);
    }
    public void UpdateAchievements()
    {
        client.UpdateAchievementData(uid, ach_1_Firstmeet.currentAmount, ach_2_Overload.currentAmount, ach_3_Pass.currentAmount,
            ach_5_Allcake.currentAmount, ach_6_FirstEnchant.currentAmount);
    }
    //TODO
    public void UpdateLastLevel()
    {
    }
    #endregion

    //关卡记录
    [System.Serializable]
    public class LevelRecord
    {
        public string sceneName;
        [Header("重生点")]
        public Vector3 rebirth_pos;
        public bool rebirth_Reverse;

        public bool isPass; //暂时与成就等无关
    }

    public void ClearLevelRecordsAndReverseAbility() 
    {
        for (int i = 0; i < levelRecords.Count; i++)
        {
            levelRecords[i].rebirth_pos = Vector3.zero;
            levelRecords[i].rebirth_Reverse = false;
        }
        gainReverse = false;
    }

    //获得蛋糕时调用:
    //GameManager_global.GetInstance().gameData_SO.UpdateCakes(e_Cake);
    public void UpdateCakes(E_cake e_Cake)
    {
        //[注] 不同位置的蛋糕 需要标志
        Debug.Log("更新蛋糕数量");
        ach_4_Firstcake.UpdateMe(1);
        ach_5_Allcake.UpdateMe(e_Cake);
    }
    //其他成就时调用:
    //GameManager_global.GetInstance().gameData_SO.【成就】.UpdateMe();
}