using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    //オブジェクト
    [SerializeField]
    [Tooltip("プレイヤー")]
    private GameObject player;

    private GameObject followCamera;

    //カメラ距離
    private float distance = 0.0f;

    //カメラの状態フラグ
    bool isRotate = true;
    bool isMove = true;
    bool isFollow = true;
    void Start()
    {
        followCamera = this.gameObject;
    }
    void Update()
    {
        CheckCameraIsFollow();
        if (isRotate && isMove && isFollow)
        {
            //followCamera.transform.position = GetVector(player.GetComponent<PlayerController>().GetNum()) - new Vector3(distance, distance, distance);
        }
    }
    //座標セッター
    public void SetPos(Vector3 pos)
    {
        if (isMove)
        {
            followCamera.transform.position = pos;
        }
    }
    //座標ゲッター
    public Vector3 GetPos()
    {
        return followCamera.transform.position;
    }
    //回転セッター
    public void SetRotate(float x, float y, float z)
    {
        if (isRotate)
        {
            followCamera.transform.rotation = Quaternion.Euler(x, y, z);
        }
    }
    //各フラグセッター
    public void IsMove(bool flag)
    {
        isMove = flag;
    }
    public void IsRotation(bool flag)
    {
        isRotate = flag;
    }
    public void IsFollow(bool flag)
    {
        isFollow = flag;
    }
    //カメラ追尾チェック
    private void CheckCameraIsFollow()
    {
        //直前の座標取得
        Vector3 previouPosition = followCamera.transform.position;
        if (!isFollow)
        {
            followCamera.transform.position = previouPosition;
        }
    }
    private void ZoomInOut()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            if (distance > -0.5f)
            {
                distance -= 0.01f;
            }
        }
        else if (Input.GetKey(KeyCode.E))
        {
            if (distance < 0.5f)
            {
                distance += 0.01f;
            }
        }
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
    public Vector3 GetVector()
    {
        return followCamera.transform.position;
    }

    public Vector3 GetVector(int num)
    {
        Vector3 vector = Vector3.zero;

        if (num == 0)
        {
            vector = new Vector3(0.0f, 0.60f, -1.0f);
        }
        //重力：上
        else if (num == 1)
        {
            vector = new Vector3(0.0f, -0.60f, -1.0f);
        }
        //重力：左
        else if (num == 2)
        {
            vector = new Vector3(0.60f, 0.0f, -1.0f);
        }
        //重力：右
        else if (num == 3)
        {
            vector = new Vector3(-0.60f, 0.0f, -1.0f);
        }
        //重力：手前
        else if (num == 4)
        {
            vector = new Vector3(0.0f, 1.0f, 0.60f);
        }
        //重力：奥
        else if (num == 5)
        {
            vector = new Vector3(0.0f, -1.0f, -0.60f);
        }
        return vector;
    }
}
