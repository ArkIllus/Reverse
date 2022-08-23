using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TipBoard : MonoBehaviour
{
    //[SerializeField] private bool isEntered;
    public GameObject tip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //isEntered = true;
            tip.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //isEntered = false;
            tip.SetActive(false);
        }
    }
}
