using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Env_Ice : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    if (collision.CompareTag("Boss_Fire"))
    {
        Invoke("Destroy_Ice", 0.1f);
    }
    }

    private void Destroy_Ice()
    {
        Destroy(gameObject);
    }
}
