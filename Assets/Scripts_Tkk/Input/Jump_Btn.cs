using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Jump_Btn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool down;
    public bool up;



    public void OnPointerDown(PointerEventData eventData)
    {
        print("���£�������");
        down = true;
        Invoke("SetDown_False", 0.01f);



    }
    private void SetDown_False()
    {
        down = false;

    }


    private void SetUp_False()
    {
        up = false;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        // print("̧�𣡣�����");
        up = true;
        Invoke("SetUp_False", 0.01f);


    }
}
