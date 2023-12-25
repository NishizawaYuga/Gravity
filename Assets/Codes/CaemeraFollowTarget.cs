using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class CaemeraFollowTarget : MonoBehaviour
{
    //オブジェクト
    [SerializeField]
    [Tooltip("testplayer")]
    private GameObject target;

    //スクリプト
    PlayerController script;

    //座標と回転
    private Vector3 offset;
    private Vector3 rotation;

    //追従するかどうか
    bool isFollow = true;
    bool setDefault = true;

    //カメラ回転（手動）
    float rotY = 0f;

    //可動域
    private float moveY = 10;
    private float moveX = 10;
    public float maxMoveY = 50;
    public float minMoveY = 0;
    public float maxMoveX = 0;
    public float minMoveX = 0;

    //カメラ距離
    private float distance = -1.05f;
    //PV用
    private float posZ = 1.05f;
    Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {

        //ゲーム開始時点のカメラとターゲットの距離
        offset = gameObject.transform.position - target.transform.position;
        //スクリプト登録
        script = target.GetComponent<PlayerController>();
    }

    private void UpdatePos(float x, float y, float z)
    {
        velocity.x = 1.05f - distance;
        velocity.y = 1.05f - distance;
        velocity.z = 1.05f - distance;
        //velocity = gameObject.transform.position - target.transform.position;
        //velocity = velocity.normalized;
        //velocity.x *= distance;
        //velocity.y *= distance;
        //velocity.z *= distance;
        //PV用
        //if (Input.GetKey(KeyCode.Q))
        //{
        //    if (distance > -0.5f)
        //    {
        //        distance -= 0.01f;
        //    }
        //}
        //else if (Input.GetKey(KeyCode.E))
        //{
        //    if (distance < 0.5f)
        //    {
        //        distance += 0.01f;
        //    }
        //}
        //distance = Input.GetAxis("Mouse ScrollWheel");
        //distance *= Input.mouseScrollDelta.y * 1.05f;


        //常にターゲットから一定距離離す
        //引数で打ち消したり逆転させる
        Vector3 pos = new Vector3(x * velocity.x, y * velocity.y, z * velocity.z);
        //相対座標でカメラの座標を決める
        pos += target.transform.position;

        offset = pos - target.transform.position;
    }

    //プレイヤーが移動した後にカメラが移動するようにする
    private void LateUpdate()
    {
        int underNum = script.GetNum();
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (underNum == 0)
            {
                //rotY -= 0.5f;
            }
        }
        if (isFollow)
        {
            //カメラの位置をターゲットの位置にオフセットを足した場所にする
            gameObject.transform.position = target.transform.position + offset;
            //可動域
            moveX = gameObject.transform.position.x;
            moveY = gameObject.transform.position.y;
            if(moveX > maxMoveX)
            {
                moveX = maxMoveX;
            }
            else if(moveX < minMoveX)
            {
                moveX = minMoveX;
            }
            if(moveY > maxMoveY)
            {
                moveY = maxMoveY;
            }
            else if(moveY < minMoveY)
            {
                moveY = minMoveY;
                //moveY = 50;
            }
            gameObject.transform.position = new Vector3(moveX, moveY, gameObject.transform.position.z);

            //カメラの回転
            //重力：下
            if (setDefault)
            {
                if (underNum == 0)
                {
                    gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    //gameObject.transform.RotateAround(target.transform.position, Vector3.up, 0.1f);
                    UpdatePos(0.0f, 0.0f, -1.05f);
                }
                //重力：上
                else if (underNum == 1)
                {
                    //gameObject.transform.rotation = Quaternion.Euler(-30, 0, 180);
                    //UpdatePos(0.0f, -0.60f, -1.05f);
                    //gameObject.transform.rotation = Quaternion.Euler(-30, 0, 0);
                    gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
                    UpdatePos(0.0f, 0.0f, -1.05f);
                }
                //重力：左
                else if (underNum == 2)
                {
                    gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                    UpdatePos(0.0f, 0.0f, -1.05f);
                }
                //重力：右
                else if (underNum == 3)
                {
                    gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                    UpdatePos(-0.0f, 0.0f, -1.05f);
                }
                //重力：手前
                else if (underNum == 4)
                {
                    gameObject.transform.rotation = Quaternion.Euler(120, 0, 0);
                    UpdatePos(0.0f, -1.05f, 0.0f);
                }
                //重力：奥
                else if (underNum == 5)
                {
                    gameObject.transform.rotation = Quaternion.Euler(-60, 0, 0);
                    UpdatePos(0.0f, -1.05f, -0.0f);
                }
            }
        }
    }

    public void ChangeCameraRotation(int underNum)
    {
        //カメラの回転
        //重力：下
        if (underNum == 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            UpdatePos(0.0f, 0.0f, -1.05f);
        }
        //重力：上
        else if (underNum == 1)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
            UpdatePos(0.0f, -0.0f, -1.05f);
        }
        //重力：左
        else if (underNum == 2)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
            UpdatePos(0.0f, 0.0f, -1.05f);
        }
        //重力：右
        else if (underNum == 3)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
            UpdatePos(-0.0f, 0.0f, -1.05f);
        }
        //重力：手前
        else if (underNum == 4)
        {
            gameObject.transform.rotation = Quaternion.Euler(120, 0, 0);
            UpdatePos(0.0f, -1.05f, 0.0f);
        }
        //重力：奥
        else if (underNum == 5)
        {
            gameObject.transform.rotation = Quaternion.Euler(-60, 0, 0);
            UpdatePos(0.0f, -1.05f, -0.0f);
        }
    }
    public void IsFollow(bool change)
    {
        isFollow = change;
    }
    public void IsDefault(bool change)
    {
        setDefault = change;
    }
    public void SetPos()
    {
        gameObject.transform.position = target.transform.position + offset;
        if (gameObject.transform.position.y > maxMoveY)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, maxMoveY, gameObject.transform.position.z);
        }
        else if (gameObject.transform.position.y < minMoveY)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, minMoveY, gameObject.transform.position.z);
        }
    }
    public void SetRotNum(float x, float y, float z)
    {
        gameObject.transform.rotation = Quaternion.Euler(x, y, z);
    }
    public Vector3 GetVector()
    {
        return gameObject.transform.position;
    }
    public Quaternion GetRotation()
    {
        return gameObject.transform.rotation;
    }
    public Quaternion GetEndRotation(int num)
    {
        Quaternion quaternion = Quaternion.identity;

        if (num == 0)
        {
            quaternion = Quaternion.Euler(0, 0, 0);
        }
        else if (num == 1)
        {
            quaternion = Quaternion.Euler(0, 0, 180);
        }
        else if (num == 2)
        {
            quaternion = Quaternion.Euler(0, 0, -90);
        }
        else if (num == 3)
        {
            quaternion = Quaternion.Euler(0, 0, 90);
        }
        else if (num == 4)
        {
            quaternion = Quaternion.Euler(90, 0, 0);
        }
        else if (num == 5)
        {
            quaternion = Quaternion.Euler(-90, 0, 0);
        }
        return quaternion;
    }
    public Vector3 GetVector(int num)
    {
        Vector3 vector = Vector3.zero;

        if (num == 0)
        {
            vector = new Vector3(0.0f, 0.0f, -1.05f);
        }
        //重力：上
        else if (num == 1)
        {
            vector = new Vector3(0.0f, -0.0f, -1.05f);
        }
        //重力：左
        else if (num == 2)
        {
            vector = new Vector3(0.0f, 0.0f, -1.05f);
        }
        //重力：右
        else if (num == 3)
        {
            vector = new Vector3(-0.0f, 0.0f, -1.05f);
        }
        //重力：手前
        else if (num == 4)
        {
            vector = new Vector3(0.0f, 1.05f, 0.60f);
        }
        //重力：奥
        else if (num == 5)
        {
            vector = new Vector3(0.0f, -1.05f, -0.60f);
        }
        return vector;
    }

    public void SetOffset(Vector3 pos)
    {
        //velocity = pos;
        UpdatePos(pos.x, pos.y, pos.z);
    }
}
