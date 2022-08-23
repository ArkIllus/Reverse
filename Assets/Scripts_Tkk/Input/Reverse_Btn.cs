using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Reverse_Btn : MonoBehaviour, IPointerDownHandler
{
    public bool Reverse;
   


    public void OnPointerDown(PointerEventData eventData)
    {
    
        Reverse = true;
        Invoke("Set_False", 0.01f);

    }
    private void Set_False()
    {
        Reverse = false;
    }

   
}
