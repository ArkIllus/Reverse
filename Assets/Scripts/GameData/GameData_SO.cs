using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameData", menuName = "new GameData_SO")]
public class GameData_SO : ScriptableObject
{
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

    public void UpdateFromServer()
    {
        //GameData gameData = GameData.CreateFromJSON(ServerHttp.Instance.text_receive);
        //Debug.Log(ServerHttp.Instance.text_receive);

        //gametime = gameData.gametime;
        //biggestfruit = gameData.biggestfruit;
        //watermelontime = gameData.watermelontime;
        //highestscore = gameData.score;

        //currScore = 0;
    }

    //关卡记录
    [System.Serializable]
    public class LevelRecord
    {
        public string sceneName;
        [Header("重生点")]
        public Vector3 rebirth_pos;
        public bool rebirth_Reverse;

        //[Header("成就相关")]
        //public int cake;
        //public int cake_t; //关卡蛋糕总数
    }

    public void ClearLevelRecords() 
    {
        for (int i = 0; i < levelRecords.Count; i++)
        {
            levelRecords[i].rebirth_pos = Vector3.zero;
            levelRecords[i].rebirth_Reverse = false;
        }
    }

    public void UpdateCakes(E_cake e_Cake)
    {
        //[注] 不同位置的草莓 需要标志
        Debug.Log("更新草莓数量");
        ach_4_Firstcake.UpdateMe(1);
        ach_5_Allcake.UpdateMe(e_Cake);
    }
}