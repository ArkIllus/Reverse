using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ColliderTag : EntityComponent
{
    public List<E_ColliderTag> tagList;


    // Ѱ��Tag�������ģ�parent·���ϣ���һ��GoEntity
    public Entity GetEntityObject()
    {
        Transform t = transform;
        while (t != null)
        {
            var goEntity = t.GetComponent<Entity>();
            if (goEntity != null) return goEntity;
            t = t.parent;
        }
        return null;
    }

    public void AddTag(E_ColliderTag tag)
    {
        foreach (var eTag in tagList)
        {
            if (eTag == tag) return;
        }
        tagList.Add(tag);
    }

    public void RemoveTag(E_ColliderTag tag)
    {
        tagList.Remove(tag);
    }
}

public enum E_ColliderTag
{
    Player, // ���
    Wall, // ǽ��
    Ground, // ����
    SharpPoint, // ���
    Platform, // �ɴ��·�������ƽ̨
    Movable, // ���Ա��ƶ�
    //None //��Tag
}
