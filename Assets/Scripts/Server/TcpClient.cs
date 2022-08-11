using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using Google.Protobuf;
using System;
using System.Text;
using Minipb;
using System.Linq;
using System.Threading;

public class TcpClient
{
    public Socket _socket = null;
    public BaseUserData udata;
    public AchievementData adata;
    public UserFriends ufriends;
    /*
    ManualResetEvent udataevent = new ManualResetEvent(false);
    ManualResetEvent adataevent = new ManualResetEvent(false);
    ManualResetEvent ufriendsevent = new ManualResetEvent(false);
    */
    public Dictionary<int, ManualResetEvent>EventsMap = new Dictionary<int, ManualResetEvent>();
    
    public void MakeTcpConnect(string ip, int port, int uid) 
    {
        try
        {
            
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(ip, port);
            Debug.Log("Connect success");

            for(int i = 1002; i < 1020; i++){
                EventsMap[i] = new ManualResetEvent(false);
            }

            _socket.Send(TcpMessage.EncodeCommunicationMessage(1002, TcpMessage.EncodeUserAccount("", "", uid)));
            byte[] buf = new byte[1024];
            int len = _socket.Receive(buf);
            CommunicationMessage cm = TcpMessage.DecodeCommunicationMessage(buf, len);
            Debug.Log(cm.MessageId);
            /*
            登录成功事件
            */
            Thread r_thread = new Thread(ReceiveMessage);
            r_thread.IsBackground = true;
            r_thread.Start();
        }
        catch (Exception ex) 
        {
            Debug.Log(ex.Message);
        }
    }

    public void Close()
    {
        _socket.Shutdown(SocketShutdown.Both);
        _socket.Close();
        _socket = null;
    }

    public void ReceiveMessage()
    {
        while (true)
        {
            try{
                byte[] buf = new byte[1024];
                int len = _socket.Receive(buf);
                if (len == 0)
                {
                    break;
                }
                CommunicationMessage cm = TcpMessage.DecodeCommunicationMessage(buf, len);
                Debug.Log(cm.MessageId);
                switch (cm.MessageId)
                {
                    case 2003:
                        udata = TcpMessage.DecodeBaseUserData(cm.BytesData.ToByteArray());
                        EventsMap[1003].Set();
                        break;
                    case 2004:
                        EventsMap[1004].Set();
                        break;
                    case 2005:
                        adata = TcpMessage.DecodeAchievementData(cm.BytesData.ToByteArray());
                        EventsMap[1005].Set();
                        break;
                    case 2006:
                        EventsMap[1006].Set();
                        break;
                    case 2007:
                        ufriends = TcpMessage.DecodeUserFriends(cm.BytesData.ToByteArray());
                        EventsMap[1007].Set();
                        break;
                    case 2008:
                        EventsMap[1008].Set();
                        break;
                    case 3008:
                        EventsMap[1008].Set();
                        Debug.Log("已经是好友");
                        break;
                    case 2009:
                        EventsMap[1009].Set();
                        break;
                    case 2010:
                        EventsMap[1010].Set();
                        break;
                    case 2013:
                        EventsMap[1013].Set();
                        break;
                    case 1011:
                        Debug.Log("该更新好友申请了");
                        break;
                    case 1012:
                        Debug.Log("该更新好友列表了");
                        break;
                    case 3002:
                        Debug.Log("账户在另一处登录");
                        Close();
                        break;
                }
            }
            catch {}
        }
    }

    public void SendMessage(int messageid, byte[] buf)
    {
        _socket.Send(TcpMessage.EncodeCommunicationMessage(messageid, buf));
    }
    
    public BaseUserData GetUserData(int uid)
    {
        _socket.Send(TcpMessage.EncodeCommunicationMessage(1003, TcpMessage.EncodeUpdataUserDataMessage(uid, TcpMessage.EncodeBaseUserData("", -1, -1, ""))));
        EventsMap[1003].WaitOne();
        Debug.Log(udata.Username);
        EventsMap[1003].Reset();
        return udata;
    }

    public void UpdateUserData(int uid, string username, int profid, string bio)
    {
        _socket.Send(TcpMessage.EncodeCommunicationMessage(1004, TcpMessage.EncodeUpdataUserDataMessage(uid, TcpMessage.EncodeBaseUserData(username, -1, profid, bio))));
        EventsMap[1004].WaitOne();
        EventsMap[1004].Reset();
    }

    public AchievementData GetAchievementData(int uid)
    {
        _socket.Send(TcpMessage.EncodeCommunicationMessage(1005, TcpMessage.EncodeUpdataUserDataMessage(uid, TcpMessage.EncodeAchievementData(-1))));
        EventsMap[1005].WaitOne();
        Debug.Log(adata.Score);
        EventsMap[1005].Reset();
        return adata;
    }

    public void UpdataAchievementData(int uid, int score)
    {
        _socket.Send(TcpMessage.EncodeCommunicationMessage(1006, TcpMessage.EncodeUpdataUserDataMessage(uid, TcpMessage.EncodeAchievementData(score))));
        EventsMap[1006].WaitOne();
        EventsMap[1006].Reset();
    }

    public UserFriends GetUserFriends(int uid)
    {
        _socket.Send(TcpMessage.EncodeCommunicationMessage(1007, TcpMessage.EncodeUserAccount("", "", uid)));
        EventsMap[1007].WaitOne();
        EventsMap[1007].Reset();
        Debug.Log(ufriends.FriendsUidList);
        return ufriends;
    }

    public void SendFriendRequest(int uid, int goaluid)
    {
        _socket.Send(TcpMessage.EncodeCommunicationMessage(1008, TcpMessage.EncodeFriendRequest(uid, goaluid)));
        EventsMap[1008].WaitOne();
        EventsMap[1008].Reset();
    }

    public void AgreeWithFriendRequest(int uid, int goaluid)
    {
        _socket.Send(TcpMessage.EncodeCommunicationMessage(1009, TcpMessage.EncodeFriendRequest(uid, goaluid)));
        EventsMap[1009].WaitOne();
        EventsMap[1009].Reset();
    }

    public void RejectFriendRequest(int uid, int goaluid)
    {
        _socket.Send(TcpMessage.EncodeCommunicationMessage(1010, TcpMessage.EncodeFriendRequest(uid, goaluid)));
        EventsMap[1010].WaitOne();
        EventsMap[1010].Reset();
    }

    public void DeleteFriend(int uid, int goaluid)
    {
        _socket.Send(TcpMessage.EncodeCommunicationMessage(1013, TcpMessage.EncodeFriendRequest(uid, goaluid)));
        EventsMap[1013].WaitOne();
        EventsMap[1013].Reset();
    }
    
}