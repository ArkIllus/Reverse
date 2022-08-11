using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using Minipb;
using System;

public class HttpMessage : MonoBehaviour
{
    public static byte[] EncodeHttpMessage(int messageid, string loginid, string password)
    {
        UserAccount useraccount = new UserAccount(){
            LoginId = loginid,
            Password = password,
        };
        CommunicationMessage cm = new CommunicationMessage(){
            MessageId = messageid,
            BytesData = Google.Protobuf.ByteString.CopyFrom(useraccount.ToByteArray()),
        };
        return cm.ToByteArray();
    }

    public static CommunicationMessage DecodeCommunicationMessage(byte[] buf)
    {
        CommunicationMessage cm = new CommunicationMessage();
        cm.MergeFrom(buf);
        return cm;
    }

    public static UserAccount DecodeUserAccount(byte[] buf)
    {
        UserAccount ua = new UserAccount();
        ua.MergeFrom(buf);
        return ua;
    }
}
