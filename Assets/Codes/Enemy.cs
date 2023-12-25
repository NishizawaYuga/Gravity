using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //自分自身
    GameObject enemy;
    //視界
    [SerializeField]
    [Tooltip("視界")]
    GameObject visibility;
    //オブジェクト
    [SerializeField]
    [Tooltip("プレイヤーオブジェクトを置く")]
    private GameObject target;
    //スクリプト
    private Visibility vis;
    private ChangeGravity cG;   //重力
    //追跡時間
    int chaseTime = 60;
    int chaseMaxTime = 60;
    //再度索敵までのクールタイム
    int coolSarchTime = 30;
    int coolSarchMaxTime = 30;
    //状態
    int stateNum = 0;
    bool Damage = false;
    //移動速度
    float speed = 2f;

    void Start()
    {
        enemy = this.gameObject;
        vis = visibility.GetComponent<Visibility>();
        cG = enemy.GetComponent<ChangeGravity>();
    }
    void FixedUpdate()
    {
        switch (stateNum)
        {
            case 0:
                //索敵範囲に入ったかチェック
                CheckSearch();
                break;
            case 1:
                //追跡
                Chase();
                break;
            case 2:
                //休憩
                BreakTime();
                break;
        }
    }
    void CheckSearch()
    {
        if (vis.GetSearch())
        {
            //追跡開始
            stateNum = 1;
        }
    }

    void Chase()
    {
        chaseTime--;
        Vector3 velocity = Vector3.zero;
        if (chaseTime > 0)
        {
            velocity = target.transform.position - enemy.transform.position;
            velocity = velocity.normalized;
            if (cG.GetNum() < 2)
            {
                //velocity.x *= speed;
                velocity.y = 0;
                //velocity.z *= speed;
                enemy.transform.position += velocity;
            }
            else if (cG.GetNum() > 1 && cG.GetNum() < 4)
            {
                velocity.x = 0;
                velocity.y *= speed;
                velocity.z *= speed;
                enemy.transform.position += velocity;
            }
            else if (cG.GetNum() > 3)
            {
                velocity.x *= speed;
                velocity.y *= speed;
                velocity.z = 0;
                enemy.transform.position += velocity;
            }
            //常に敵の注視点をプレイヤーに
            enemy.transform.rotation.SetLookRotation(target.transform.position);
        }
        else
        {
            chaseTime = chaseMaxTime;
            stateNum = 2;
        }
    }
    void BreakTime()
    {
        if(coolSarchTime > 0)
        {
            coolSarchTime--;
        }
        else
        {
            coolSarchTime = coolSarchMaxTime;
            stateNum = 0;
        }
    }
}
