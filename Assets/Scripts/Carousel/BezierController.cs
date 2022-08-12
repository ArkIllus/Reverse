using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//贝塞尔曲线控制
public class BezierController : MonoBehaviour
{
    //起始和结束点最好有预留，否则拖动时可能会出现左右闪烁问题
    [Header("首尾消失距离")]
    [Range(0f, 0.5f)]
    [SerializeField] float leftDis = 0.05f; 
    [Range(0.5f, 1f)]
    [SerializeField] float rightDis = 0.95f;

    [Header("控制鼠标拖动物体速度")]
    [SerializeField] float speed = 1;

    [Header("物体容器")]
    public Transform content;//容器

    LineRenderer line;

    [Header("图片精灵列表，贝塞尔曲线点位")]
    public List<Sprite> spriteList;
    public List<Vector3> bezierPointList;

    [Header("自定义效果 By Bob")]
    public float addTofirst = 0.15f;
    public float maxScale = 1.5f;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 100;

        //初始化图片布局
        InitItem();
    }

    Vector2 oldPos;
    void Update()
    {
        //生成贝塞尔曲线点 LineRender
        Bezier_Creat();

        //按下I按键初始化item布局 
        if (Input.GetKey(KeyCode.I))
            InitItem(); //初始化

        #region 鼠标拖动控制左右移动
        if (Input.GetMouseButtonDown(0))
        {
            oldPos = Input.mousePosition;
            //Debug.Log("oldPos = " + oldPos);
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 pos = new Vector2(Input.mousePosition.x - oldPos.x, Input.mousePosition.y - oldPos.y);
            for (int i = 0; i < content.childCount; i++)
            {
                CarouselItem obj = content.GetChild(i).GetComponent<CarouselItem>();
                objMove(pos, obj);

                RectTransform rectTransform = obj.GetComponent<RectTransform>();
                //尺寸渐变
                // obj.BezierT 0.25 ~0.5 ~0.25
                // localScale  1 ~1.5 ~1
                if (obj.BezierT < 0.25f || obj.BezierT > 0.75f)
                {
                    rectTransform.localScale = Vector3.one;
                }
                else
                {
                    rectTransform.localScale = Vector3.one * (1.5f - Mathf.Abs(obj.BezierT - 0.5f) * 2f);
                }
            }
            oldPos = Input.mousePosition;
        }
        #endregion

    }

    #region 初始化item平均布局
    public void InitItem()
    {
        //Debug.Log("初始化");

        //计算物体在曲线中的t值（间距）
        float space = Mathf.Abs(rightDis - leftDis) / content.childCount;

        //循环初始化 
        for (int i = 0; i < content.childCount; i++)
        {
            CarouselItem obj = content.GetChild(i).GetComponent<CarouselItem>();
            obj.BezierT = leftDis + (i * space);

            //避免第一个图标看不见
            obj.BezierT = obj.BezierT + addTofirst;
            if (obj.BezierT > 1) obj.BezierT -= 1;

            //Debug.Log("obj.BezierT = " + obj.BezierT);
            Vector3 _pos = Bezier.N_OrderBezierLerp(bezierPointList, obj.BezierT);
            //Debug.Log("_pos = " + _pos);
            //obj.transform.position = _pos;
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.anchoredPosition3D = _pos;
            //尺寸渐变
            // obj.BezierT 0 ~0.5 ~1
            // localScale  1 ~1.5 ~1
            if (obj.BezierT < 0.3f)
            {
                rectTransform.localScale = Vector3.one;
            }
            else
            {
                rectTransform.localScale = Vector3.one * (1.5f - Mathf.Abs(obj.BezierT - 0.5f));
            }
            SetImageSprite(1, obj); //修改图片
        }
    }
    #endregion

    #region 物体移动

    void objMove(Vector2 pos, CarouselItem obj)
    {
        //计算物体BezierT的值
        float value = (pos.x * speed * Time.deltaTime) / Screen.width;
        obj.BezierT += value;

        //物体BezierT的值大于等于1 （到达最右边）
        if (obj.BezierT >= 1)
        {
            //BezierT值等于 除1的余数
            obj.BezierT = obj.BezierT % 1;
            SetImageSprite(1, obj);//切换下一张图片
        }

        //物体BezierT的值小于等于0 （到达最左边边）
        if (obj.BezierT <= 0)
        {
            //取除1的余数
            float newBezierT = obj.BezierT % 1;  
            //我们的贝塞尔T取值范围为0~1，不会出现负数 所以“取余的值”要为正，这里进行绝对值计算
            obj.BezierT = rightDis- Mathf.Abs(newBezierT);
            SetImageSprite(-1, obj);//切换上一张图片
        }

        //计算在贝塞尔曲线的位置并赋值
        Vector3 _pos = Bezier.N_OrderBezierLerp(bezierPointList, obj.BezierT);
        //Debug.Log("_pos = " + _pos);
        //obj.transform.position = _pos;
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition3D = _pos;
        //尺寸渐变
        // obj.BezierT 0 ~0.5 ~1
        // localScale  1 ~1.5 ~1
        rectTransform.localScale = Vector3.one * (1.5f - Mathf.Abs(obj.BezierT - 0.5f));
    }

    #endregion

    #region 图片替换
    int currentIndex = -1;
    /// <summary>
    /// 图片替换
    /// </summary>
    /// <param name="dir">移动方向</param>
    /// <param name="item">要替换的图片物体</param>
    void SetImageSprite(int dir, CarouselItem item)
    {
        if (dir > 0)
        {
            currentIndex++;
            //超出图片列表最大值 等于0
            if (currentIndex > spriteList.Count - 1)
                currentIndex = 0;
        }

        if (dir < 0)
        {
            currentIndex--;
            //超出图片列表最小值 等于列表最后一个
            if (currentIndex < 0)
                currentIndex = spriteList.Count - 1;
        }

        //修改item图片
        item.SetImage(spriteList[currentIndex]);

    }
    #endregion

    #region 生成贝塞尔曲线
    /// <summary>
    /// 生成贝塞尔曲线Line
    /// </summary>
    private void Bezier_Creat()
    {
        for (int i = 1; i < line.positionCount + 1; i++)
        {
            Vector3 Bezierposition = Bezier.N_OrderBezierLerp(bezierPointList, i*0.01f);
            line.SetPosition(i - 1, Bezierposition);
        }
    }
    #endregion


}
