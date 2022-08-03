using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EntityComponent : MonoBehaviour
{
    public T GetComponentNotNull<T>()
    {
        var cpt = GetComponent<T>();
        if (cpt == null)
        {
            throw new Exception("Component" + typeof(T) + "Not Found");
        }
        return cpt;
    }

    protected virtual void AfterFixedUpdate()
    {

    }

    protected void DoAfterFixedUpdate()
    {
        StartCoroutine(Co_AfterFixedUpdate());
    }

    private IEnumerator Co_AfterFixedUpdate()
    {
        //yield WaitForFixedUpdate Continue after all FixedUpdate has been called on all scripts.
        //If the coroutine yielded before FixedUpdate, then it resumes after FixedUpdate in the current frame.
        yield return new WaitForFixedUpdate();
        AfterFixedUpdate();
    }
}
