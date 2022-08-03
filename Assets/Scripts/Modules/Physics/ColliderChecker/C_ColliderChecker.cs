using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class C_ColliderChecker : EntityComponent
{
    public List<ColliderCheckerItem> checkers;

    public ColliderCheckerItem GetChecker(string checkerName)
    {
        foreach (var checker in checkers)
        {
            if(checker.name.Equals(checkerName))
            {
                return checker;
            }
        }
        return null;
    }

    public void ColliderCheckerSystem()
    {
        foreach (var checker in checkers)
        {
            checker.ColliderCheckerSystem();
        }
    }
}

[Serializable]
public class ColliderCheckerItem
{
    public string name = "Ground Checker";

    //��Ҫ������ײ���ColliderTag
    public List<E_ColliderTag> acceptTags = new List<E_ColliderTag>();

    //��ײ����collider
    public Collider2D collider;

    // ÿ֡���£���ʾ�Ƿ���Ŀ��ColliderTag����ײ�巢����ײ
    public bool isHit;

    // ��ײ�¼�����һ֡����ײ����һ֡��ײ���򴥷���
    public bool hitEvent;

    // ��ײ���������б�Tag��
    public List<C_ColliderTag> targetTagList;
    public C_ColliderTag firstTargetTag => targetTagList[0];
    // ��ײ���������б�Collider2D��
    public List<Collider2D> targetColliderList;
    public Collider2D firstTargetCollider => targetColliderList[0];

    //Ŀǰ���ܼ��һ��ColliderTag
    public void ColliderCheckerSystem()
    {
        //��ȡ���Collider�ص�������Collider���б�
        Collider2D[] contacts = collider.GetOverlapColliders();
        targetTagList.Clear();
        targetColliderList.Clear();
        foreach (var collider in contacts)
        {
            var colliderTag = collider.GetComponent<C_ColliderTag>();
            if (colliderTag == null)
            {
                continue;
            }
            //Ŀǰ���ܼ��һ��ColliderTag
            if (colliderTag.tagList.Any(t1 => acceptTags.Any(t2 => t1 == t2))){
                targetTagList.Add(colliderTag);
                targetColliderList.Add(collider);
            }
        }

        hitEvent = targetTagList.Count > 0 && !isHit;
        isHit = targetTagList.Count > 0;
    }
}
