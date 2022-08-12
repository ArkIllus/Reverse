using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 贝塞尔曲线工具脚本
/// </summary>
public class Bezier
{
    //存储计算的贝塞尔曲线结果
    static List<Vector3> calculateBezierPointList = new List<Vector3>();
    /// <summary>
    /// N阶贝塞尔曲线
    /// </summary>
    /// <param name="bezierPointList">贝塞尔曲线列表</param>
    /// <param name="t">t值</param>
    /// <returns></returns>
    public static Vector3 N_OrderBezierLerp(List<Vector3> bezierPointList, float t)
    {
        //点为空或为0
        if (bezierPointList == null || bezierPointList.Count <= 0)
            return Vector3.zero;

        //点为1
        if (bezierPointList.Count == 1)
            return bezierPointList[0];

        Vector3 result = Vector3.zero; //返回的结果

        //列表赋值
        calculateBezierPointList = bezierPointList;

        //当列表的值大于等于2时执行
        while (calculateBezierPointList.Count >= 2)
        {
            List<Vector3> pointList = new List<Vector3>();

            for (int i = 0; i < calculateBezierPointList.Count - 1; i++)
            {
                Vector3 posA = calculateBezierPointList[i];
                Vector3 posB = calculateBezierPointList[i + 1];
                pointList.Add(Vector3.Lerp(posA, posB, t));
            }
            //将计算好的贝塞尔曲线列表赋值给calculateBezierPointList
            calculateBezierPointList = pointList;
        }

        //最终计算后列表中只有1个点
        result = calculateBezierPointList[0];

        result.x = result.x * 0.5f * Screen.width / 8.53f - Screen.width * 0.5f;
        result.y = result.y * 0.5f * Screen.height / 5.85f - Screen.height * 0.5f;
        return result;
    }
}