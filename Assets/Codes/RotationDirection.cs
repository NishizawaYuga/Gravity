using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationDirection : MonoBehaviour
{
    public float GetMax(int startDirection,int endDirection)
    {
        float maxTime = 0f;
        if (endDirection + startDirection == 1 || endDirection + startDirection == 5)
        {
            maxTime = 180f;
        }
        else
        {
            maxTime = 90f;
        }
        return maxTime;
    }
    public int BackReverce(int startDirection_, int endDirection_)
    {
        //様々なパターンに応じて回転させる角度等を設定する
        if (startDirection_ == 0 && endDirection_ == 2 || startDirection_ == 1 && endDirection_ == 3 || startDirection_ == 2 && endDirection_ == 1 || startDirection_ == 3 && endDirection_ == 0)
        {
            return -1;
        }

        return 1;
    }
    public void RotationalCorrection(GameObject rotObject,int startDirection_)
    {
        if (startDirection_ == 0)
        {
            rotObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (startDirection_ == 1)
        {
            rotObject.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (startDirection_ == 2)
        {
            rotObject.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (startDirection_ == 3)
        {
            rotObject.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }
    public float GetStart(int startDirection_)
    {
        float startRotate = 0f;
        if (startDirection_ == 0)
        {
            startRotate = 0f;
        }
        else if (startDirection_ == 1)
        {
            startRotate = 180f;
        }
        else if (startDirection_ == 2)
        {
            startRotate = -90f;
        }
        else if (startDirection_ == 3)
        {
            startRotate = 90f;
        }
        return startRotate;
    }
}
