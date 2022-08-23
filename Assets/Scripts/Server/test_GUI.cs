using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.IO;
using Minipb;
using UnityEngine.Networking;

public class Test
{
    public static string test_loginid = "xingcuo";
    public static string test_password = "123456";
    public static string url = "http://192.168.7.230:9090/minigame/user";
    public static string ip = "192.168.7.230";
    public static int port = 9092;
    public static int uid = 10041463;

    public static string username = "testname";
    public static int profid = 11;
    public static string bio = "testbio";
    public static int firstmeet = 1;
    public static int overload = 1;
    public static int pass = 1;
    public static int cake = 1;
    public static int firstmagic = 1;
}


public class test_GUI : MonoBehaviour
{
    public int uid = 0;
    TcpClient client = new TcpClient();
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 100), "Register"))
        {
            StartCoroutine(HttpClient.Register(Test.url, Test.test_loginid, Test.test_password));
        }
        if (GUI.Button(new Rect(120, 10, 100, 100), "Login"))
        {
            StartCoroutine(HttpClient.Login(Test.url, Test.test_loginid, Test.test_password));
        }

        if(GUI.Button(new Rect(10, 120, 100, 100), "Connect_Tcp"))
        {
            client.MakeTcpConnect(Test.ip, Test.port, Test.uid);
        }
        if(GUI.Button(new Rect(120, 120, 100, 100), "Close_Tcp"))
        {
            client.Close();
        }

        if(GUI.Button(new Rect(10, 230, 100, 100), "GetUserData"))
        {
            BaseUserData udata = client.GetUserData(Test.uid);
        }
        if(GUI.Button(new Rect(120, 230, 100, 100), "UpdateUserData"))
        {
            client.UpdateUserData(Test.uid, Test.username, Test.profid, Test.bio);
        }

        if(GUI.Button(new Rect(10, 340, 100, 100), "GetAchievementData"))
        {
            AchievementData adata = client.GetAchievementData(Test.uid);
        }
        if(GUI.Button(new Rect(120, 340, 100, 100), "UpdateAchievementData"))
        {
            client.UpdateAchievementData(Test.uid, Test.firstmeet, Test.overload, Test.pass, Test.cake, Test.firstmagic);
        }

        if(GUI.Button(new Rect(400, 10, 100, 100), "GetHurdleData"))
        {
            HurdleData hdata = client.GetHurdleData(Test.uid);
        }
        if(GUI.Button(new Rect(510, 10, 100, 100), "UpdateHurdleData"))
        {
            OneHurdleData level1 = TcpMessage.InitOneHurdleData(float.Parse("1.0"), float.Parse("1.0"), float.Parse("1.0"), false, 10);
            OneHurdleData level2 = TcpMessage.InitOneHurdleData(float.Parse("11.0"), float.Parse("1.0"), float.Parse("1.0"), false, 100);
            OneHurdleData level3 = TcpMessage.InitOneHurdleData(float.Parse("111.0"), float.Parse("1.0"), float.Parse("1.0"), false, 1000);
            client.UpdateHurdleData(Test.uid, level1, level2, level3);
        }
    }
}
