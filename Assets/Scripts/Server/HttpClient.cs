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
            if (cm.MessageId == 2000) {
                Debug.Log("Register success");

                ///进行注册成功的处理
                //卸载Init_XXX面板 //TODO:优化
                //FindObjectOfType<Init_UI>().HideAndDestroyAllPanels();
                Init_UI.HidePanelsAfterLogin();
                ////异步加载ExternalScene  加载过程中播放转场画面（+进度条）
                //SceneMgr.GetInstance().LoadSceneAsync(SceneData_SO.externalScene);
                //FindObjectOfType<External_UI>().gameObject.SetActive(true);
                External_UI.ShowPanelsAtStart();
            }
            else if (cm.MessageId == 3000) {
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
            if (cm.MessageId == 2001) {
                Debug.Log("Login success");
                UserAccount ua = HttpMessage.DecodeUserAccount(cm.BytesData.ToByteArray());
                Debug.Log(ua.Uid);
                
                ///进行登录成功的处理
                //记录返回的uid
                GameManager_global.GetInstance().gameData_SO.uid = ua.Uid;
                Init_UI.HidePanelsAfterLogin();
                UIManager.GetInstance().ShowPanel<Init_MainPanel>("Init_MainPanel", E_UI_Layer.Mid);
                //显示标题
                UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").ShowTitle();
            }
            else if (cm.MessageId == 3001) {
                Debug.Log("Login fail");

                ///进行登录失败的处理
                UIManager.GetInstance().GetPanel<Init_LoginPanel>("Init_LoginPanel").ShowTip_error();
            }
        }
    }

}