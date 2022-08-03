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

    //需要检测的碰撞体的ColliderTag
    public List<E_ColliderTag> acceptTags = new List<E_ColliderTag>();

    //碰撞检测的collider
    public Collider2D collider;

    // 每帧更新，表示是否与目标ColliderTag的碰撞体发生碰撞
    public bool isHit;

    // 碰撞事件（上一帧无碰撞，这一帧碰撞，则触发）
    public bool hitEvent;

    // 碰撞到的物体列表（Tag）
    public List<C_ColliderTag> targetTagList;
    public C_ColliderTag firstTargetTag => targetTagList[0];
    // 碰撞到的物体列表（Collider2D）
    public List<Collider2D> targetColliderList;
    public Collider2D firstTargetCollider => targetColliderList[0];

    //目前仅能检测一个ColliderTag
    public void ColliderCheckerSystem()
    {
        //获取与该Collider重叠的所有Collider的列表
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
            //目前仅能检测一个ColliderTag
            if (colliderTag.tagList.Any(t1 => acceptTags.Any(t2 => t1 == t2))){
                targetTagList.Add(colliderTag);
                targetColliderList.Add(collider);
            }
        }

        hitEvent = targetTagList.Count > 0 && !isHit;
        isHit = targetTagList.Count > 0;
    }
}
