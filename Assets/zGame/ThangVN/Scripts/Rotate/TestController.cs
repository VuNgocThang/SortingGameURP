using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    public EasyController easyController;
    public Transform nX;

    //private void Update()
    //{

    //    Vector3 dv = easyController.dV;
    //    if (dv != Vector3.zero)
    //    {
    //        nX.localEulerAngles += new Vector3(0, -dv.y, 0) * 10f * Time.deltaTime;
    //    }
    //}

    Vector3 initialPosition;
    Vector3 currentPosition;

    public float topBorder;
    public float bottomBorder;
    public float leftBorder;
    public float rightBorder;

    bool IsInside(Vector3 p)
    {
        if (p.x < leftBorder)
            return false;

        if (p.x > Screen.width - rightBorder)
            return false;

        if (p.y < bottomBorder)
            return false;

        if (p.y > Screen.height - topBorder)
            return false;

        return true;
    }
    //public Rect touchRegion = new Rect(100, 100, 500, 500);

    void Update()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            //if (touchRegion.Contains(touch.position))
            //if (IsInside(touch.position))
            //{
            //Debug.Log("in region");
            if (touch.phase == TouchPhase.Began)
            {
                initialPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                currentPosition = touch.position;

                Vector2 initialVector = initialPosition - Camera.main.WorldToScreenPoint(transform.position);
                Vector2 currentVector = currentPosition - Camera.main.WorldToScreenPoint(transform.position);

                float angle = Vector2.SignedAngle(initialVector, currentVector);
                Debug.Log("angle: " + angle);

                if (angle > 1)
                {
                    Debug.Log("Clockwise rotation detected.");
                    nX.localEulerAngles += new Vector3(0, -2f, 0);
                }
                else if (angle < -1)
                {
                    Debug.Log("Counterclockwise rotation detected.");
                    nX.localEulerAngles += new Vector3(0, +2f, 0);

                }

                initialPosition = currentPosition;
            }
        }
        //else
        //{
        //    Debug.Log("out region");
        //}
    }
}
