using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    //public EasyController easyController;
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

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

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

                if (angle > 0)
                {
                    Debug.Log("Clockwise rotation detected.");
                    //nX.localEulerAngles += new Vector3(0, nX.localRotation.y, 0) * 10f * Time.deltaTime;
                }
                else if (angle < 0)
                {
                    Debug.Log("Counterclockwise rotation detected.");

                }

                // Update initial position for next frame
                initialPosition = currentPosition;
            }
        }
    }
}
