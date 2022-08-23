using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dash_Btn : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
   public bool Dash;
 
   

    public void OnPointerDown(PointerEventData eventData)
    {
       Dash = true;
        Invoke("Set_False", 0.01f);
      
    }
    private void Set_False()
    {
        Dash = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
      
    } 
}
