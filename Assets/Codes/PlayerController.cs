using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Jobs;

public class PlayerController : MonoBehaviour
{
    public float power = 0.75f;
    private float oldPower = 0.75f;
    public new Rigidbody rigidbody;

    //ChangeGravity cG;

    //自機本体とカメラのオブジェクト
    GameObject player;
    GameObject mainCamera;

    //今現在の重力の方向 0から下右上左手前奥の順
    int underNum = 0;

    //動けるかどうかのフラグ
    bool canMove = true;

    //生きているかどうか
    bool isDead = false;

    //残機
    int life = 4;
    //復活までのタイマー
    int restartTimer = 250;
    const int maxReStartTimer = 250;
    //復活地点
    Vector3 restartPoint;
    //初期重力
    int startGravityNum = 0;
    //スクリプト
    private CaemeraFollowTarget cF;     //カメラ追従制御
    private ChangeGravity cG;   //重力

    private float plTurn = 225;

    // Start is called before the first frame update
    void Start()
    {
        //cG = GetComponent<ChangeGravity>();
        //オブジェクト指定
        player = this.gameObject;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        restartPoint = player.transform.position;
        cF = mainCamera.GetComponent<CaemeraFollowTarget>();
        cG = player.GetComponent<ChangeGravity>();
        startGravityNum = cG.GetNum();
    }

    //重力方向取得
    public int GetNum()
    {
        return underNum;
    }
    //フラグ取得
    public bool GetCanMove()
    {
        return canMove;
    }
    //フラグ代入
    public void SendCanMoveFlag(bool flag)
    {
        canMove = flag;
    }
    public void ChangeActive(bool flag)
    {
        player.SetActive(flag);
    }

    //座標代入
    public void SetPosition(Vector3 pos)
    {
        player.transform.position.Set(pos.x, pos.y, pos.z);
    }

    //斜め減速処理
    private void DiagonalDeceleration(bool key1, bool key2, bool key3, bool key4)
    {
        if (key1 && key2 || key1 && key4 || key3 && key2 || key3 && key4)
        {
            power = oldPower * 0.65f;
        }
        else
        {
            power = oldPower;
        }
    }

    //方向転換
    private void Turn()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            plTurn = 225;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            plTurn = -45;
        }
    }

    public void ManualTurn(int gravityNum)
    {
        if(gravityNum == 0)
        {
            player.transform.rotation = Quaternion.Euler(0, plTurn, 0);
        }
        else if(gravityNum == 1)
        {
            player.transform.rotation = Quaternion.Euler(0, -plTurn, 180);
        }
        else if(gravityNum == 2)
        {
            player.transform.rotation = Quaternion.Euler(plTurn, 0, -90);
        }
        else if (gravityNum == 3)
        {
            player.transform.rotation = Quaternion.Euler(-plTurn, 0, 90);
        }
        else if (gravityNum == 4)
        {
            player.transform.rotation = Quaternion.Euler(-90, 0, -plTurn);
        }
        else if (gravityNum == 5)
        {
            player.transform.rotation = Quaternion.Euler(90, 0, plTurn);
        }
    }

    // Update is called once per frame
    void Update()
    {
        cG.Update();

        Turn();

        if (cG.GetGravity().y < 0)
        {
            underNum = 0;
            player.transform.rotation = Quaternion.Euler(0, plTurn, 0);
        }
        else if (cG.GetGravity().y > 0)
        {
            underNum = 1;
            player.transform.rotation = Quaternion.Euler(0, -plTurn, 180);
        }
        else if (cG.GetGravity().x < 0)
        {
            underNum = 2;
            player.transform.rotation = Quaternion.Euler(plTurn, 0, -90);
        }

        else if (cG.GetGravity().x > 0)
        {
            underNum = 3;
            player.transform.rotation = Quaternion.Euler(-plTurn, 0, 90);
        }
        else if (cG.GetGravity().z < 0)
        {
            underNum = 4;
            player.transform.rotation = Quaternion.Euler(-90, 0, -plTurn);
        }
        else if (cG.GetGravity().z > 0)
        {
            underNum = 5;
            player.transform.rotation = Quaternion.Euler(90, 0, plTurn);
        }
        //斜め減速処理
        DiagonalDeceleration(Input.GetKey(KeyCode.W), Input.GetKey(KeyCode.A), Input.GetKey(KeyCode.S), Input.GetKey(KeyCode.D));

        //死亡時処理
        if (isDead)
        {
            restartTimer--;
            if (restartTimer < 0)
            {
                restartTimer = maxReStartTimer;
                isDead = false;
                player.transform.position = restartPoint;
                cF.IsFollow(true);
                SendCanMoveFlag(true);
                cG.GravityDirection(startGravityNum);
            }
        }
    }

    public void ChangeGravity(int direction)
    {
        cG.GravityDirection(direction);
    }

    public void ChangeDead(bool dead)
    {
        isDead = dead;
        life--;
    }
    public bool IsDead()
    {
        return isDead;
    }
}
