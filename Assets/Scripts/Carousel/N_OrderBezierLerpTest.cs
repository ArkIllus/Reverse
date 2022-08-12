using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// N阶贝塞尔曲线测试
/// </summary>
public class N_OrderBezierLerpTest : MonoBehaviour
{
    LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 100;
    }

    void Update()
    {
        //实时创建贝塞尔曲线点
        for (int i = 1; i < line.positionCount + 1; i++)
        {
            Vector3 Bezierposition = N_OrderBezierLerp(i * 0.01f);
            line.SetPosition(i - 1, Bezierposition);
        }
    }

    #region N阶贝塞尔曲线
    //贝塞尔曲线点  外部赋值
    public List<Vector3> bezierPointList;


    //存储计算的贝塞尔曲线结果
    List<Vector3> calculateBezierPointList=new List<Vector3>(); 
    public Vector3 N_OrderBezierLerp(float t) 
    {
        //点为空或为0
        if (bezierPointList==null|| bezierPointList.Count<=0)
            return Vector3.zero;

        //点为1
        if (bezierPointList.Count == 1)
            return bezierPointList[0];

        Vector3 result = Vector3.zero; //返回的结果

        //列表赋值
        calculateBezierPointList = bezierPointList;

        //当列表的值大于等于2时执行
        while (calculateBezierPointList.Count>=2)
        {
            List<Vector3> pointList = new List<Vector3>();

            for (int i = 0; i < calculateBezierPointList.Count-1; i++)
            {
                Vector3 posA = calculateBezierPointList[i];
                Vector3 posB = calculateBezierPointList[i+1];
                pointList.Add(Vector3.Lerp(posA, posB,t));
            }
            //将计算好的贝塞尔曲线列表赋值给calculateBezierPointList
            calculateBezierPointList = pointList;
        }

        //最终计算后列表中只有1个点
        result = calculateBezierPointList[0]; 
        return result;
    }
    #endregion


}
