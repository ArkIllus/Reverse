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
            BytesData = Google.Protobuf.ByteString.CopyFrom(buf),
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
        return useraccount.ToByteArray();
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

    public static byte[] EncodeUpdateUserDataMessage(int uid, byte[] buf)
    {
        UpdateUserDataMessage upmsg = new UpdateUserDataMessage(){
            Uid = uid,
            BaseUserData = Google.Protobuf.ByteString.CopyFrom(buf),
        };
        return upmsg.ToByteArray();
    }

    public static byte[] EncodeAchievementData(int firstmeet, int overload, int pass, int cake, int firstmagic)
    {
        AchievementData adata = new AchievementData(){
            FirstMeet = firstmeet,
            Overload = overload,
            Pass = pass,
            Cake = cake,
            FirstMagic = firstmagic,
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

    public static OneHurdleData InitOneHurdleData(float x = 0, float y = 0, float z = 0, bool reverse = false, int passtime = -1)
    {
        OneHurdleData odata = new OneHurdleData(){
            X = x,
            Y = y,
            Z = z,
            Reverse = reverse,
            PassTime = passtime,
        };
        return odata;
    }

    public static byte[] EncodeHurdleData(OneHurdleData level1, OneHurdleData level2, OneHurdleData level3)
    {
        HurdleData hdata = new HurdleData();
        hdata.Hurdle.Add(level1);
        hdata.Hurdle.Add(level2);
        hdata.Hurdle.Add(level3);
        return hdata.ToByteArray();
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

    public static HurdleData DecodeHurdleData(byte[] buf)
    {
        HurdleData hdata = new HurdleData();
        hdata.MergeFrom(buf);
        return hdata;
    }
    //var x = hdata.Hurdle[0].X;
    //public static OneHurdleData DecodeOneHurdleData(byte[] buf)
    //{
    //    OneHurdleData hdata = new OneHurdleData();
    //    hdata.MergeFrom(buf);
    //    return hdata;
    //}
}
