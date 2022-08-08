using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Minipb;
//using Google.Protobuf;  //引用Google的Protobuf库
using System;

public class TestProto : MonoBehaviour
{
    //发送数据时：序列化操作：（结构体 -> byte[]）  
    //new一个你定义的消息类
    //UserAccount useraccount = new UserAccount();

    //public void Test()
    //{
    //    //为UserAccount各个属性赋值，可省缺某些属性  
    //    useraccount.LoginId = "yueliang";
    //    useraccount.Password = "123456";
    //    //定义一个储存序列化后的消息数据，大小为上面的UserAccount类的大小  
    //    Byte[] useraccountData = new byte[useraccount.CalculateSize()];

    //    //定义一个Google库的输出流,传入useraccountData  
    //    CodedOutputStream UserAccountOutput = new CodedOutputStream(useraccountData);
    //    //将useraccount类写入到输出流就完成了序列化  
    //    useraccount.WriteTo(UserAccountOutput);

    //    //接收数据时：反序列化操作：（byte[] -> 结构体）  
    //    //new一个你定义的消息类  
    //    UserAccount _useraccunt = new UserAccount();
    //    //定义一个储存序列化后的消息数据，大小为上面的UserAccount类的大小  
    //    Byte[] useraccountData = new byte[_useraccount.CalculateSize()];
    //    //定义一个Google库的输入流,传入useraccountData  
    //    CodedInputStream UserAccountInput = new CodedInputStream(useraccountData);
    //    //传入定义的那个输入流  
    //    _useraccunt.MergeFrom(UserAccountInput);
    //    //此时结构体赋值完成  
    //    Console.WriteLine("返回的Uid为" + _useraccunt.Uid);
    //}
}
