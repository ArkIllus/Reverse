using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Minipb;
using System.IO;
using Google.Protobuf;
using System;
using System.Text;

public class TcpMessage : MonoBehaviour
{
    public static byte[] EncodeCommunicationMessage(int messageid, byte[] buf)
    {
        CommunicationMessage cm = new CommunicationMessage(){
            MessageId = messageid,
            BytesData = Google.Protobuf.ByteString.CopyFrom(buf), //
        };
        return cm.ToByteArray();
    }

    public static byte[] EncodeUserAccount(string loginid, string password, int uid)
    {
        UserAccount useraccount = new UserAccount(){
            LoginId = loginid,
            Password = password,
            Uid = uid,
        };
        return useraccount.ToByteArray(); //
    }
    
    public static byte[] EncodeBaseUserData(string username, int nameid, int profid, string bio)
    {
        BaseUserData udata = new BaseUserData(){
            Username = username,
            Nameid = nameid,
            Profid = profid,
            Bio = bio,
        };
        return udata.ToByteArray();
    }

    public static byte[] EncodeUpdataUserDataMessage(int uid, byte[] buf)
    {
        UpdateUserDataMessage upmsg = new UpdateUserDataMessage(){
            Uid = uid,
            BaseUserData = Google.Protobuf.ByteString.CopyFrom(buf),
        };
        return upmsg.ToByteArray();
    }

    public static byte[] EncodeAchievementData(int score)
    {
        AchievementData adata = new AchievementData(){
            Score = score,
        };
        return adata.ToByteArray();
    }

    public static byte[] EncodeFriendRequest(int uid, int goaluid)
    {
        FriendRequest freq = new FriendRequest(){
            Uid = uid,
            GoalUid = goaluid,
        };
        return freq.ToByteArray();
    }

    public static CommunicationMessage DecodeCommunicationMessage(byte[] buf, int len)
    {
        MemoryStream ms=new MemoryStream();
        ms.Write(buf, 0, len);
        ms.Position = 0;
        CommunicationMessage cm = new CommunicationMessage();
        cm = CommunicationMessage.Parser.ParseFrom(ms);
        return cm;
    }

    public static UserAccount DecodeUserAccount(byte[] buf)
    {
        UserAccount ua = new UserAccount();
        ua.MergeFrom(buf);
        return ua;
    }

    public static BaseUserData DecodeBaseUserData(byte[] buf)
    {
        BaseUserData udata = new BaseUserData();
        udata.MergeFrom(buf);
        return udata;
    }

    public static AchievementData DecodeAchievementData(byte[] buf)
    {
        AchievementData adata = new AchievementData();
        adata.MergeFrom(buf);
        return adata;
    }

    public static UserFriends DecodeUserFriends(byte[] buf)
    {
        UserFriends ufriends = new UserFriends();
        ufriends.MergeFrom(buf);
        return ufriends;
    }
}
