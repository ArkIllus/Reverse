using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public int lastLevel = -1;

    [Header("��Ϸ����(�ɾ����)")]
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

    //�ؿ���¼
    [System.Serializable]
    public class LevelRecord
    {
        public string sceneName;
        [Header("������")]
        public Vector3 rebirth_pos;
        public bool rebirth_Reverse;

        //[Header("�ɾ����")]
        //public int cake;
        //public int cake_t; //�ؿ���������
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
        //[ע] ��ͬλ�õĲ�ݮ ��Ҫ��־
        Debug.Log("���²�ݮ����");
        ach_4_Firstcake.UpdateMe(1);
        ach_5_Allcake.UpdateMe(e_Cake);
    }
}