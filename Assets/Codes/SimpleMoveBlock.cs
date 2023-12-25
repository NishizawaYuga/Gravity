using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveBlock : MonoBehaviour
{
    public Vector3 speed = new Vector3(0, 0, 0);
    public int moveTime = 1;
    public bool isMove = true;

    private GameObject me;
    private bool roundState = false;
    private int timer = 0;
    void Start()
    {
        me = this.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMove)
        {
            if (!roundState)
            {
                if(moveTime > timer)
                {
                    timer++;
                    me.transform.position += new Vector3(speed.x * 0.01f,speed.y * 0.01f,speed.z * 0.01f);
                    me.transform.localScale -= new Vector3(speed.z * 0.001f, speed.z * 0.001f, speed.z * 0.001f);
                }
                else
                {
                    timer = 0;
                    roundState = true;
                }
            }
            else if (roundState)
            {
                if (moveTime > timer)
                {
                    timer++;
                    me.transform.position -= new Vector3(speed.x * 0.01f, speed.y * 0.01f, speed.z * 0.01f);
                    me.transform.localScale += new Vector3(speed.z * 0.001f, speed.z * 0.001f, speed.z * 0.001f);
                }
                else
                {
                    timer = 0;
                    roundState = false;
                }
            }
        }
    }
}
