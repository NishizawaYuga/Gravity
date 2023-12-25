using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    //惑星本体と自機
    private GameObject planet;
    private GameObject player;

    //重力変動処理を入れるためのスクリプト
    ChangeGravity script;

    //重力発生範囲
    const float gravityArea = 3.0f;
    //中心から足場までの距離
    const float distance = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //オブジェクトとスクリプト登録
        planet = this.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        script = player.GetComponent<ChangeGravity>();
    }

    // Update is called once per frame
    void Update()
    {
        //惑星の座標を中心に一定距離以内だったら重力を発生させる
        //下方向
        if (planet.transform.position.y - player.transform.position.y <= -distance)
        {
            script.GravityDirection(0);
        }
        //右方向
        else if (planet.transform.position.x - player.transform.position.x >= distance)
        {
            script.GravityDirection(1);
        }
        //上方向
        else if (planet.transform.position.y - player.transform.position.y >= distance)
        {
            script.GravityDirection(2);
        }
        //左方向
        else if (planet.transform.position.x - player.transform.position.x <= -distance)
        {
            script.GravityDirection(3);
        }
        //手前方向
        else if (planet.transform.position.z - player.transform.position.z >= distance)
        {
            script.GravityDirection(4);
        }
        //奥方向
        else if (planet.transform.position.z - player.transform.position.z <= -distance)
        {
            script.GravityDirection(5);
        }
    }
}
