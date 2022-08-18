using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //[Header("游戏数据(成就相关)")]

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
    }

    public void ClearLevelRecords() 
    {
        for (int i = 0; i < levelRecords.Count; i++)
        {
            levelRecords[i].rebirth_pos = Vector3.zero;
            levelRecords[i].rebirth_Reverse = false;
        }
    }
}