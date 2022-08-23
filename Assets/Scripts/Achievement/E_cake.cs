using UnityEngine;

[System.Serializable]
public struct E_cake //需要重载 == 运算符
//public class E_cake
{
    [Header("从0开始")]
    public int level; // from 0
    public int index; // from 0

    public static bool operator ==(E_cake lhs, E_cake rhs)
    {
        if (lhs.level == rhs.level && lhs.index == rhs.index && lhs.index == rhs.index)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool operator !=(E_cake lhs, E_cake rhs)
    {
        return !(lhs == rhs);
    }
    public override bool Equals(object obj)
    {
        E_cake e_cake = (E_cake)obj;
        if (level.Equals(e_cake.level) == true && index.Equals(e_cake.index) == true)
            return true;
        else
            return false;
    }
    public override int GetHashCode()
    {
        return (int)(level + index); //可改进...
    }
}
