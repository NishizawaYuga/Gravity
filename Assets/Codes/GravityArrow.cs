using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GravityArrow : MonoBehaviour
{
    [SerializeField]
    [Tooltip("起動前の重量方向、基準は他と同じ")]
    private int startDirection;

    [SerializeField]
    [Tooltip("起動後の重量方向、基準は他と同じ")]
    private int endDirection;

    //プレイヤーオブジェクト
    [SerializeField]
    [Tooltip("プレイヤーオブジェクト")]
    private GameObject player;

    //カメラ
    [SerializeField]
    [Tooltip("カメラ")]
    private GameObject mainCamera;

    [SerializeField]
    [Tooltip("他の重力矢印に連動させるなら")]
    private GameObject other;

    //スクリプト
    private ChangeGravity cg;           //重力方向
    private CaemeraFollowTarget cF;     //カメラ追従制御
    private GravityArrow ga;            //連動させる方
    //private CameraFollow cF;
    private Easing ease;                //イージング
    private RotationDirection rotD; //回転処理

    //起動フラグ
    bool isStartUp = false;
    //初期地点と終着点を格納する変数
    Vector3 startPos;
    Quaternion startRot;
    Vector3 endPos;
    Quaternion endRot;
    private float nowTime = 0f;            //経過時間
    private float maxTime = 0f;     //移動時間
    private float rotateTime = 0f;
    private bool isRotate = false;
    private float startObjectRotate = 0f;
    private bool alreadyStarted = false;


    void Start()
    {
        //スクリプト登録
        cg = player.GetComponent<ChangeGravity>();
        cF = mainCamera.GetComponent<CaemeraFollowTarget>();
        if (other != null)
        {
            ga = other.GetComponent<GravityArrow>();
        }
        //cF = mainCamera.GetComponent<CameraFollow>();
        ease = new Easing();
        rotD = new RotationDirection();
        maxTime = rotD.GetMax(startDirection, endDirection);
        rotD.RotationalCorrection(this.gameObject, startDirection);
        startObjectRotate = rotD.GetStart(startDirection);
        //RotationalCorrection(startDirection);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStartUp && !alreadyStarted)
        {
            //if (cF.GetRotation() != endRot)
            if (nowTime < maxTime && !isRotate)
            {
                //cF.SetPos();
                //RotateObject();
                this.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, ease.OutQuad(maxTime * rotD.BackReverce(startDirection, endDirection), startObjectRotate, maxTime, nowTime));
                nowTime += 4f;
            }
            else
            {
                isRotate = true;
                nowTime = 0;
                //isStartUp = false;
                //cF.IsFollow(false);
                cF.IsDefault(false);
                cg.GravityDirection(endDirection);
                //this.gameObject.transform.rotation = endRot;
            }
        }
        else if(other != null)
        {
            if (ga.GetOtherStart())
            {
                //MakeAvailableAgain();
            }
        }
        if (isRotate && !alreadyStarted)
        {
            if (rotateTime < maxTime + 1)
            {
                cF.transform.rotation = Quaternion.Euler(0f, 0f, ease.OutQuad(maxTime * rotD.BackReverce(startDirection, endDirection), startObjectRotate, maxTime, rotateTime));
                rotateTime += 4f;
                if (rotateTime > maxTime)
                {
                    rotateTime = maxTime + 1;
                }
            }
            else
            {
                //cF.ChangeCameraRotation(endDirection);

                rotateTime = 0f;
                isRotate = false;
                alreadyStarted = true;
                //cF.IsFollow(true);
                //cF.SetPos(player.transform.position - cF.GetVector(endDirection) / 2);
            }
        }
    }


    public void ChangeGravity()
    {
        if (!isStartUp)
        {
            isStartUp = true;
            //cF.IsFollow(false);
            //cF.IsDefault(false);
            startPos = cF.GetVector();
            //startRot = cF.GetRotation();
            startRot = Quaternion.Euler(0, 0, 0);
            //endRot = cF.GetEndRotation(endDirection);
            endRot = Quaternion.Euler(0, 0, 180);
        }
    }
    private void MakeAvailableAgain()
    {
        if (nowTime < rotD.GetMax(endDirection,startDirection))
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, ease.OutQuad(rotD.GetMax(endDirection, startDirection) * rotD.BackReverce(endDirection, startDirection), rotD.GetStart(endDirection), rotD.GetMax(endDirection, startDirection), nowTime));
            nowTime += 4f;
        }
        else
        {
            nowTime = 0;
            alreadyStarted = false;
        }
    }
    public bool GetOtherStart()
    {
        return isStartUp;
    }
}
