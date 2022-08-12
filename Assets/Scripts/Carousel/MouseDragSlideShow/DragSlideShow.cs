using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DragSlideShow : MonoBehaviour
{
    //年份按钮最左侧和最右侧子物体下标
    int yearBtnLeftIndex;
    int yearBtnRightIndex;

    public float speed;
    public float space;
    public RectTransform left;
    public RectTransform right;
    public RectTransform content;

    void Start()
    {
        yearBtnLeftIndex = 0;
        yearBtnRightIndex = content.childCount - 1;

        for (int i = 0; i < content.childCount; i++)
            content.GetChild(i).GetChild(0).GetComponent<Text>().text = i.ToString();
    }

    float oldX;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            oldX = Input.mousePosition.x;

        if (Input.GetMouseButton(0))
        {
            for (int i = 0; i < content.childCount; i++)
            {
                DragItem(Input.mousePosition.x - oldX, content.GetChild(i).GetComponent<RectTransform>());
            }

            oldX = Input.mousePosition.x;
        }
    }

    void DragItem(float x, RectTransform item)
    {
        item.anchoredPosition += new Vector2(x * speed * Time.deltaTime, 0);

        //x大于0 并且位置超出右侧指定距离时
        if (x > 0 && item.anchoredPosition.x >= right.anchoredPosition.x)
        {
            //修改位置到最左边按钮位置
            RectTransform Obj = content.GetChild(yearBtnLeftIndex).GetComponent<RectTransform>();
            item.anchoredPosition = new Vector2(Obj.anchoredPosition.x - space, Obj.anchoredPosition.y);


            //修改最左和最右边物体下标
            yearBtnLeftIndex = item.GetSiblingIndex();
            yearBtnRightIndex = yearBtnLeftIndex + 1;

            if (yearBtnRightIndex > content.childCount - 1)
                yearBtnRightIndex = yearBtnLeftIndex - 1;

        }

        if (x < 0 && item.anchoredPosition.x <= left.anchoredPosition.x)
        {

            //修改位置到最左边按钮位置
            RectTransform Obj = content.GetChild(yearBtnRightIndex).GetComponent<RectTransform>();
            item.anchoredPosition = new Vector2(Obj.anchoredPosition.x  + space, Obj.anchoredPosition.y);


            //修改最左和最右边物体下标
            yearBtnRightIndex = item.GetSiblingIndex();
            yearBtnLeftIndex = yearBtnRightIndex - 1;

            if (yearBtnLeftIndex < 0)
                yearBtnLeftIndex = yearBtnRightIndex + 1;
        }

    }
}
