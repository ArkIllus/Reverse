using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "new GameData_SO")]
public class GameData_SO : ScriptableObject
{
    //�ؿ�����
    public static string InitScene = "InitScene";
    public static string AfterLoginScene = "AfterLoginScene";
    public static string[] Levels = { "Level1", "Level2", "Level3" };

    [Header("�û�����")]
    public string username_memo;
    public string password_memo;
    public int uid;
    public int nameid; //Ψһid
    public int bio; //ǩ��

    [Header("�ؿ���¼")]
    public List<LevelRecord> levelRecords = new List<LevelRecord>();

    //[Header("��Ϸ����(�ɾ����)")]

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

    //�ؿ���¼
    [System.Serializable]
    public class LevelRecord
    {
        public string sceneName;
        [Header("������")]
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