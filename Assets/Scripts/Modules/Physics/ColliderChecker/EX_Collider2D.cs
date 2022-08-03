using System.Collections.Generic;
using TinyCeleste._04_Extension._03_CSharp;
using UnityEngine;


public static class EX_Collider2D
{
    // 实时返回与当前碰撞体发生碰撞且具有acceptTags中的任意一个E_Tag的Tag列表
    public static List<C_ColliderTag> GetTags(this Collider2D collider, List<E_ColliderTag> acceptTags)
    {
        var result = new Collider2D[100];
        var filter = new ContactFilter2D().NoFilter();

        int num = collider.OverlapCollider(filter, result);
        var tagList = new List<C_ColliderTag>();
        for(int j = 0;j < num;j ++)
        {
            var tag = result[j].GetComponent<C_ColliderTag>();
            if (tag != null && EX_List.Match(tag.tagList, acceptTags))
            {
                tagList.Add(tag);
            }
        }

        return tagList;
    }

    public static Collider2D[] GetOverlapColliders(this Collider2D collider, bool includeTrigger = true)
    {
        var contacts = new Collider2D[100];
        var filter = new ContactFilter2D().NoFilter();
        filter.useTriggers = includeTrigger;
            
        int num = collider.OverlapCollider(filter, contacts);
        var result = new Collider2D[num];
        for (int i = 0; i < num; i++)
        {
            result[i] = contacts[i];
        }

        return result;
    }
}