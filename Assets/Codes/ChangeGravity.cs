using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGravity : MonoBehaviour
{
    [SerializeField] private Vector3 localGravity;
    private Rigidbody rBody;

    [SerializeField]
    [Tooltip("重力方向")]
    public int gravitySwitch;
    //切り替え用変数
    //int gravitySwitch = 2;

    // Use this for initialization
    private void Start()
    {
        rBody = this.GetComponent<Rigidbody>();
        rBody.useGravity = false; //最初にrigidBodyの重力を使わなくする
    }

    private void FixedUpdate()
    {
        SetLocalGravity(); //重力をAddForceでかけるメソッドを呼ぶ。FixedUpdateが好ましい。
    }

    private void SetLocalGravity()
    {
        rBody.AddForce(localGravity, ForceMode.Acceleration);
    }

    public Vector3 GetGravity()
    {
        return localGravity;
    }

    public int GetNum()
    {
        return gravitySwitch;
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Tab))
        {
            //gravitySwitch++;
            //if (gravitySwitch > 11)
            //{
            //    gravitySwitch = 0;
            //}
        }

        //01上下：23:左右：45前後
        if (gravitySwitch == 0)
        {
            localGravity.x = 0;
            localGravity.y = -9.81f;
            localGravity.z = 0;
        }
        else if (gravitySwitch == 1)
        {
            localGravity.x = 0;
            localGravity.y = 9.81f;
            localGravity.z = 0;
        }
        else if (gravitySwitch == 2)
        {
            localGravity.x = -9.81f;
            localGravity.y = 0;
            localGravity.z = 0;
        }
        
        else if (gravitySwitch == 3)
        {
            localGravity.x = 9.81f;
            localGravity.y = 0;
            localGravity.z = 0;
        }
        else if (gravitySwitch == 4)
        {
            localGravity.x = 0;
            localGravity.y = 0;
            localGravity.z = -9.81f;
        }
        else if (gravitySwitch == 5)
        {
            localGravity.x = 0;
            localGravity.y = 0;
            localGravity.z = 9.81f;
        }
        else if(gravitySwitch == 6)
        {
            //無重力
            localGravity.x = 0f;
            localGravity.y = 0f;
            localGravity.z = 0f;
        }
    }

    public void GravityDirection(int gravityNum)
    {
        gravitySwitch = gravityNum;
    }
}
