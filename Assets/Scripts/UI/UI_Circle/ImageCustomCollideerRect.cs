using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ImageCustomCollideerRect : Image
{
    private CircleCollider2D collider2d;
    protected override void Start()
    {
        collider2d = GetComponent<CircleCollider2D>();
    }
    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)

    {
        bool isRay = base.IsRaycastLocationValid(screenPoint, eventCamera);
        if (isRay && (collider2d != null))
        {
            bool isTrig = collider2d.OverlapPoint(Camera.main.ScreenToWorldPoint(screenPoint));
            //Debug.Log("screenPoint" + screenPoint);
            //Debug.Log("collider2d.transform.position" + collider2d.transform.position);
            //Debug.Log("isTrig" + isTrig);
            return isTrig;
        }
        //Debug.Log("isRay" + isRay);
        return isRay;
    }
}