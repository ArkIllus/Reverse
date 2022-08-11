using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "new GameData_SO")]
public class GameData_SO : ScriptableObject
{
    [Header("��Ϸ����(�ɾ����)")]
    //public int gametime;//�������
    //public int biggestfruit;//���ɵ����ˮ��
    //public int watermelontime;//�������ϴ���
    //public int highestscore;//��ʷ��ߵ÷�

    //public int currScore;//��ǰ�÷�

    [Header("�û�����")]
    public string username_memo;
    public string password_memo;
    public int uid;
    public int nameid; //Ψһid
    public int bio; //ǩ��

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
}
