using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveBlock : MonoBehaviour
{
    public Vector3 canMoveAreaPuls = new Vector3(0, 0, 0);
    public Vector3 canMoveAreaMinus = new Vector3(0, 0, 0);
    public float speed = 1;
    public bool wandering = false;

    private bool moveDirectionstate = false;
    private int wandertingNum = 0;
    private GameObject me;
    private bool ifArrivalX, ifArrivalY, ifArrivalZ = false;

    private int timer = 0;
    private int maxTimer = 30;
    private Easing ease;

    private void Start()
    {
        me = this.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!wandering)
        {
            RoundTrip();
        }
    }

    private void RoundTrip()
    {

        if (!moveDirectionstate)
        {
            if (ifArrivalX && ifArrivalY && ifArrivalZ)
            {
                moveDirectionstate = true;
                ifArrivalX = false;
                ifArrivalY = false;
                ifArrivalZ = false;
                timer = 0;
            }
            else
            {
                timer++;
            }
            //me.transform.position = ease.InQuadVec3(canMoveAreaPuls - canMoveAreaMinus, canMoveAreaPuls, maxTimer, timer);
            me.transform.position += new Vector3(0.01f, 0, 0);
            //me.transform.position -= DirectionSpeed(canMoveAreaPuls, canMoveAreaMinus);
            if (me.transform.position.x <= canMoveAreaMinus.x)
            {
                ifArrivalX = true;
                //me.transform.position = new Vector3(canMoveAreaMinus.x, me.transform.position.y, me.transform.position.z);
            }
            if (me.transform.position.y <= canMoveAreaMinus.y)
            {
                ifArrivalY = true;
                //me.transform.position = new Vector3(me.transform.position.y, canMoveAreaMinus.y, me.transform.position.z);
            }
            if (me.transform.position.z <= canMoveAreaMinus.z)
            {
                ifArrivalZ = true;
               //me.transform.position = new Vector3(me.transform.position.x, me.transform.position.y, canMoveAreaMinus.z);
            }
        }
        else if (moveDirectionstate)
        {
            me.transform.position += DirectionSpeed(canMoveAreaMinus, canMoveAreaPuls);
            if (me.transform.position.x >= canMoveAreaPuls.x)
            {
                ifArrivalX = true;
                me.transform.position = new Vector3(canMoveAreaMinus.x, me.transform.position.y, me.transform.position.z);
            }
            if (me.transform.position.y >= canMoveAreaPuls.y)
            {
                ifArrivalY = true;
                me.transform.position = new Vector3(me.transform.position.y, canMoveAreaMinus.y, me.transform.position.z);
            }
            if (me.transform.position.z >= canMoveAreaPuls.z)
            {
                ifArrivalZ = true;
                me.transform.position = new Vector3(me.transform.position.x, me.transform.position.y, canMoveAreaMinus.z);
            }
            if (ifArrivalX && ifArrivalY && ifArrivalZ)
            {
                moveDirectionstate = true;
                ifArrivalX = false;
                ifArrivalY = false;
                ifArrivalZ = false;
            }
        }
    }

    private Vector3 DirectionSpeed(Vector3 plus, Vector3 minus)
    {
        Vector3 direction = new Vector3(1, 1, 1);
        if (plus.x < minus.x)
        {
            direction.x = -1;
        }
        if (plus.y < minus.y)
        {
            direction.y = -1;
        }
        if (plus.z < minus.z)
        {
            direction.z = -1;
        }
        return new Vector3(direction.x * speed, direction.y * speed, direction.z * speed);
    }
}
