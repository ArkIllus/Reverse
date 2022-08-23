using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.IO;
using Minipb;
using UnityEngine.Networking;
using Google.Protobuf;


public class HttpClient : MonoBehaviour
{
    public const string url = "http://192.168.7.230:9090/minigame/user";
    public static IEnumerator Register(string url, string loginid, string password)
    {
        UnityWebRequest www = UnityWebRequest.Put(url, HttpMessage.EncodeHttpMessage(1000, loginid, password));
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            byte[] results = www.downloadHandler.data;
            CommunicationMessage cm = HttpMessage.DecodeCommunicationMessage(results);
            if (cm.MessageId == 2000)
            {
                Debug.Log("Register success");

                ///进行注册成功的处理
                //卸载Init_XXX面板 //TODO:优化
                //Init_UI.HidePanelsAfterLogin();
                //UIManager.GetInstance().GetPanel<Init_RegisterPanel>("Init_RegisterPanel").HideMe();
                UIManager.GetInstance().GetPanel<Init_RegisterPanel>("Init_RegisterPanel").HideMe(() =>
                {
                    //显示标题
                    UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").ShowTitle();
                    //显示按钮
                    External_UI.ShowPanelsAtStart();
                    //切换BGM
                    //AllSceneMusicManager.Instance.PlayBGM("The Creation of Life");
                });

                /*
                UserAccount ua = HttpMessage.DecodeUserAccount(cm.BytesData.ToByteArray());
                Debug.Log(ua.Uid);
                ///进行登录成功的处理
                //记录返回的uid
                GameManager_global.GetInstance().gameData_SO.uid = ua.Uid;


                ////tcp建立连接
                GameManager_global.GetInstance().gameData_SO.MakeTcpConnect();
                ////清零所有数据
                GameManager_global.GetInstance().gameData_SO.ClearAllData();
                */

                //Login(url, loginid, password);

                yield return Login(url, GameManager_global.GetInstance().gameData_SO.username_memo,
                    GameManager_global.GetInstance().gameData_SO.password_memo);
            }
            else if (cm.MessageId == 3000)
            {
                Debug.Log("Register fail");

                ///进行注册失败的处理
                UIManager.GetInstance().GetPanel<Init_RegisterPanel>("Init_RegisterPanel").ShowTip_exist();
            }
        }
    }

    public static IEnumerator Login(string url, string loginid, string password)
    {
        UnityWebRequest www = UnityWebRequest.Put(url, HttpMessage.EncodeHttpMessage(1001, loginid, password));
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            byte[] results = www.downloadHandler.data;
            CommunicationMessage cm = HttpMessage.DecodeCommunicationMessage(results);
            if (cm.MessageId == 2001)
            {
                Debug.Log("Login success");
                UserAccount ua = HttpMessage.DecodeUserAccount(cm.BytesData.ToByteArray());
                Debug.Log(ua.Uid);

                ///进行登录成功的处理
                //记录返回的uid
                GameManager_global.GetInstance().gameData_SO.uid = ua.Uid;
                UIManager.GetInstance().GetPanel<Init_LoginPanel>("Init_LoginPanel").HideMe(() =>
                {
                    //显示标题
                    UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").ShowTitle();
                    //显示按钮
                    UIManager.GetInstance().ShowPanel<Init_MainPanel>("Init_MainPanel", E_UI_Layer.Mid);
                    //切换BGM
                    //AllSceneMusicManager.Instance.PlayBGM("The Creation of Life");
                });

                ////tcp建立连接
                GameManager_global.GetInstance().gameData_SO.MakeTcpConnect();
                ////从服务器更新所有需要的数据
                GameManager_global.GetInstance().gameData_SO.GetAllDataFromServer();
            }
            else if (cm.MessageId == 3001)
            {
                Debug.Log("Login fail");

                ///进行登录失败的处理
                UIManager.GetInstance().GetPanel<Init_LoginPanel>("Init_LoginPanel").ShowTip_error();
            }
        }
    }

}