using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBackGround : MonoBehaviour
{
    private float rotation = 0f;

    void FixedUpdate()
    {
        this.gameObject.transform.rotation = Quaternion.Euler(90f, rotation, 0f);
        rotation += 0.05f;
        if (rotation > 360)
        {
            rotation = 0f;
        }
    }
}
