using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Minipb;
//using Google.Protobuf;  //����Google��Protobuf��
using System;

public class TestProto : MonoBehaviour
{
    //��������ʱ�����л����������ṹ�� -> byte[]��  
    //newһ���㶨�����Ϣ��
    //UserAccount useraccount = new UserAccount();

    //public void Test()
    //{
    //    //ΪUserAccount�������Ը�ֵ����ʡȱĳЩ����  
    //    useraccount.LoginId = "yueliang";
    //    useraccount.Password = "123456";
    //    //����һ���������л������Ϣ���ݣ���СΪ�����UserAccount��Ĵ�С  
    //    Byte[] useraccountData = new byte[useraccount.CalculateSize()];

    //    //����һ��Google��������,����useraccountData  
    //    CodedOutputStream UserAccountOutput = new CodedOutputStream(useraccountData);
    //    //��useraccount��д�뵽���������������л�  
    //    useraccount.WriteTo(UserAccountOutput);

    //    //��������ʱ�������л���������byte[] -> �ṹ�壩  
    //    //newһ���㶨�����Ϣ��  
    //    UserAccount _useraccunt = new UserAccount();
    //    //����һ���������л������Ϣ���ݣ���СΪ�����UserAccount��Ĵ�С  
    //    Byte[] useraccountData = new byte[_useraccount.CalculateSize()];
    //    //����һ��Google���������,����useraccountData  
    //    CodedInputStream UserAccountInput = new CodedInputStream(useraccountData);
    //    //���붨����Ǹ�������  
    //    _useraccunt.MergeFrom(UserAccountInput);
    //    //��ʱ�ṹ�帳ֵ���  
    //    Console.WriteLine("���ص�UidΪ" + _useraccunt.Uid);
    //}
}
