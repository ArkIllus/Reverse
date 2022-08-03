using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ColliderTag : EntityComponent
{
    public List<E_ColliderTag> tagList;


    // 寻找Tag所依附的（parent路径上）第一个GoEntity
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
    Player, // 玩家
    Wall, // 墙面
    Ground, // 地面
    SharpPoint, // 尖刺
    Platform, // 可从下方穿过的平台
    Movable, // 可以被移动
    //None //无Tag
}
