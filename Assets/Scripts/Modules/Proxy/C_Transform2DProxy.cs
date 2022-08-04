using System.Collections;
using System.Collections.Generic;
using TinyCeleste._04_Extension._01_UnityComponent;
using UnityEngine;

/// <summary>
/// ����Entity������ڵ�Transform���������ƶ�
/// �ƶ���Transform Root����ĳ����������
/// ������2D��Ϸ��Z�����걣�ֲ��䣩
///
/// ̸�����Ҷ���Transform�Ŀ���
/// 1��һ����˵������һ���ж����������ɵĸ������壨��Player�����ؼ���Transform���ͨ��ֻ��1 ~ 3��
/// 2���������������壬������Ϊһ��Parent��ͬ���ͬ������������֯��һ��������GameManger���·��õĸ��ֵ���
/// 3��ͨ��������λ�ƻ���ٷ��������ƶ�ƽ̨�ĸ�Transform��һ�㲻�����仯���������������壨����·������ԣ�
/// 4�������Ļᷢ���ƶ���Transform���Ҿ��д�����
/// </summary>
public class C_Transform2DProxy : MonoBehaviour
{
    public new Transform transform;

    public Vector2 pos
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.SetPositionXY(value);
        }
    }

    public void Translate(Vector2 pos)
    {
        transform.Translate(-pos);
    }
}
