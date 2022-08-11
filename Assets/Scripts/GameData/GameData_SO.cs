using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "new GameData_SO")]
public class GameData_SO : ScriptableObject
{
    [Header("游戏数据(成就相关)")]
    //public int gametime;//游玩次数
    //public int biggestfruit;//生成的最大水果
    //public int watermelontime;//生成西瓜次数
    //public int highestscore;//历史最高得分

    //public int currScore;//当前得分

    [Header("用户数据")]
    public string username_memo;
    public string password_memo;
    public int uid;
    public int nameid; //唯一id
    public int bio; //签名

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
