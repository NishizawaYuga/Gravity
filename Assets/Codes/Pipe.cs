using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField]
    [Tooltip("接続先オブジェクト")]
    private GameObject destination;

    [SerializeField]
    [Tooltip("タイプ番号:0は何もなし、1でドカン処理実行")]
    private int type;

    [SerializeField]
    [Tooltip("ドカンの向き：番号は重力方向番号準拠、0が↓")]
    private int direction;

    [SerializeField]
    [Tooltip("ワープ中カメラ移動させるか")]
    private bool cameraMove = false;

    [SerializeField]
    [Tooltip("ワープ中カメラ回転させるか")]
    private bool cameraRotation = false;

    [SerializeField]
    [Tooltip("エリア移動用か")]
    private bool changeArea = false;

    //プレイヤーオブジェクト
    [SerializeField]
    [Tooltip("プレイヤーオブジェクト")]
    private GameObject player;

    //カメラ
    [SerializeField]
    [Tooltip("カメラ")]
    private GameObject mainCamera;

    //スクリプト
    PlayerController pCon;              //プレイヤー操作
    private CaemeraFollowTarget cF;     //カメラ追従制御
    private Pipe dPipe;                 //接続先ドカン
    private ChangeGravity cg;           //重力方向
    private Easing ease;                //イージング
    private RotationDirection rotD;     //回転処理


    //内部値
    private int progressNum = 0;        //進行状況
    private float nowTime = 0f;            //経過時間
    private const float maxTime = 1f;     //移動時間
    private Vector3 playerPos = new Vector3(0f, 0f, 0f);       //代入用
    private Vector3 playerOldPos = new Vector3(0f, 0f, 0f);    //プレイヤーのスタート地点格納用
    private int waitTimer = 0;          //待機時間
    private float inoutPipeSpeed = 0.002f;    //ドカンを出入りする際の速度
    private int inoutTimer = 0; //土管を出入りしている時間
    private float startObjectRotate = 0f;
    //カメライージング（移動）
    private Vector3 cameraPos = new Vector3(0f, 0f, 0f);
    private Vector3 oldCameraPos = new Vector3(0f, 0f, 0f);

    void Start()
    {
        //スクリプト登録
        if (player != null)
        {
            pCon = player.GetComponent<PlayerController>();
            cF = mainCamera.GetComponent<CaemeraFollowTarget>();
            dPipe = destination.GetComponent<Pipe>();
            cg = player.GetComponent<ChangeGravity>();
        }
        ease = new Easing();
        rotD = new RotationDirection();
        //ドカン方向矯正
        PipeDirection();

    }

    void FixedUpdate()
    {
        if (player != null)
        {
            if (type == 1)
            {
                if (progressNum == 1)
                {   //土管の向きに応じて
                    if (direction <= 1)
                    {
                        //重力：上下
                        playerPos.x = ease.OutQuad(this.gameObject.transform.position.x - playerOldPos.x, playerOldPos.x, maxTime, nowTime);
                        playerPos.z = ease.OutQuad(this.gameObject.transform.position.z - playerOldPos.z, playerOldPos.z, maxTime, nowTime);
                    }
                    else if (direction >= 2 && direction <= 3)
                    {
                        //重力：左右
                        playerPos.y = ease.OutQuad(this.gameObject.transform.position.y - playerOldPos.y, playerOldPos.y, maxTime, nowTime);
                        playerPos.z = ease.OutQuad(this.gameObject.transform.position.z - playerOldPos.z, playerOldPos.z, maxTime, nowTime);
                    }
                    else if (direction >= 4)
                    {
                        //重力：前後
                        playerPos.x = ease.OutQuad(this.gameObject.transform.position.x - playerOldPos.x, playerOldPos.x, maxTime, nowTime);
                        playerPos.y = ease.OutQuad(this.gameObject.transform.position.y - playerOldPos.y, playerOldPos.y, maxTime, nowTime);
                    }
                    player.transform.position = playerPos;
                    if (nowTime < maxTime)
                    {
                        nowTime += 0.02f;
                    }
                    else
                    {
                        if (direction <= 1)
                        {
                            player.transform.position = new Vector3(this.gameObject.transform.position.x, player.transform.position.y, this.gameObject.transform.position.z);
                        }
                        else if (direction >= 2 && direction <= 3)
                        {
                            player.transform.position = new Vector3(player.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
                        }
                        else if (direction >= 4)
                        {
                            player.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, player.transform.position.z);
                        }
                        nowTime = 0f;
                        progressNum++;
                        player.GetComponent<Collider>().enabled = false;
                        cg.GravityDirection(6);
                        if (cameraMove || cameraRotation)
                        {
                            cF.IsFollow(false);
                            cF.IsDefault(false);
                        }
                        else
                        {
                            cF.IsDefault(false);
                        }
                    }
                }
                else if (progressNum == 2)
                {
                    //待機時間
                    if (waitTimer < 5 && inoutTimer < 50)
                    {
                        waitTimer++;
                    }
                    else
                    {
                        if (inoutTimer < 50)
                        {
                            inoutTimer++;
                            //ドカンに入る
                            //向きに応じて動かす座標を変える
                            InWarp(direction, inoutPipeSpeed);
                        }
                        else
                        {
                            if (direction == 0)
                            {
                                player.transform.position = this.gameObject.transform.position + new Vector3(0f, -inoutPipeSpeed * inoutTimer, 0f);
                            }
                            else if (direction == 1)
                            {
                                player.transform.position = this.gameObject.transform.position + new Vector3(0f, +inoutPipeSpeed * inoutTimer, 0f);
                            }
                            else if (direction == 2)
                            {
                                player.transform.position = this.gameObject.transform.position + new Vector3(-inoutPipeSpeed * inoutTimer, 0f, 0f);
                            }
                            else if (direction == 3)
                            {
                                player.transform.position = this.gameObject.transform.position + new Vector3(+inoutPipeSpeed * inoutTimer, 0f, 0f);
                            }
                            else if (direction == 4)
                            {
                                player.transform.position = this.gameObject.transform.position + new Vector3(0f, 0f, -inoutPipeSpeed * inoutTimer);
                            }
                            else if (direction == 5)
                            {
                                player.transform.position = this.gameObject.transform.position + new Vector3(0f, 0f, +inoutPipeSpeed * inoutTimer);
                            }
                            oldCameraPos = player.transform.position + cF.GetVector(direction);
                            oldCameraPos.y = cF.transform.position.y;
                            if (waitTimer > 0)
                            {
                                waitTimer--;
                            }
                            else
                            {
                                pCon.ManualTurn(dPipe.GetDirection());
                                progressNum = 3;
                                if (dPipe.GetDirection() == 0)
                                {
                                    player.transform.position = destination.transform.position + new Vector3(0f, -inoutPipeSpeed * inoutTimer, 0f);
                                }
                                else if (dPipe.GetDirection() == 1)
                                {
                                    player.transform.position = destination.transform.position + new Vector3(0f, +inoutPipeSpeed * inoutTimer, 0f);
                                }
                                else if (dPipe.GetDirection() == 2)
                                {
                                    player.transform.position = destination.transform.position + new Vector3(-inoutPipeSpeed * inoutTimer, 0f, 0f);
                                }
                                else if (dPipe.GetDirection() == 3)
                                {
                                    player.transform.position = destination.transform.position + new Vector3(+inoutPipeSpeed * inoutTimer, 0f, 0f);
                                }
                                else if (dPipe.GetDirection() == 4)
                                {
                                    player.transform.position = destination.transform.position + new Vector3(0f, 0f, -inoutPipeSpeed * inoutTimer);
                                }
                                else if (dPipe.GetDirection() == 5)
                                {
                                    player.transform.position = destination.transform.position + new Vector3(0f, 0f, +inoutPipeSpeed * inoutTimer);
                                }
                                cameraPos = player.transform.position + cF.GetVector(dPipe.GetDirection());
                                cameraPos.y = cF.transform.position.y;
                                //カメラを切り替えるため一瞬だけ重力変更
                                if (cameraRotation || changeArea)
                                {
                                    cF.ChangeCameraRotation(dPipe.GetDirection());
                                }
                                cF.SetPos();
                                inoutTimer = 0;
                                waitTimer = 0;
                            }
                        }
                    }
                }
                else if (progressNum == 3)
                {
                    if (waitTimer < rotD.GetMax(direction,dPipe.GetDirection()))
                    {
                        waitTimer += 4;
                        if (cameraRotation)
                        {
                            cF.transform.rotation = Quaternion.Euler(0f, 0f, ease.OutQuad(rotD.GetMax(direction, dPipe.GetDirection()) * rotD.BackReverce(direction, dPipe.GetDirection()), startObjectRotate, rotD.GetMax(direction, dPipe.GetDirection()), waitTimer));
                        }
                        if (cameraMove)
                        {
                            if (waitTimer > 0)
                            {
                                cF.transform.position = ease.OutQuadVec3(cameraPos - oldCameraPos, oldCameraPos, rotD.GetMax(direction, dPipe.GetDirection()), waitTimer) + cF.GetVector(dPipe.GetDirection());
                            }
                        }
                    }
                    else
                    {
                        if (inoutTimer < 75)
                        {
                            inoutTimer++;
                            //ドカンに入る
                            //向きに応じて動かす座標を変える
                            InWarp(dPipe.GetDirection(), -inoutPipeSpeed);
                            cF.SetPos();
                        }
                        else
                        {
                            if (cameraMove || cameraRotation || changeArea)
                            {
                                cF.IsFollow(true);
                            }
                            cg.GravityDirection(dPipe.GetDirection());
                            pCon.SendCanMoveFlag(true);
                            progressNum = 0;
                            player.GetComponent<Collider>().enabled = true;
                            waitTimer = 0;
                            inoutTimer = 0;
                        }
                    }
                }
            }
        }
    }
    //土管の方向矯正
    private void PipeDirection()
    {
        //土管の方向矯正
        if (direction == 0)
        {
            this.gameObject.transform.parent.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (direction == 1)
        {
            this.gameObject.transform.parent.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else if (direction == 2)
        {
            this.gameObject.transform.parent.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
        else if (direction == 3)
        {
            this.gameObject.transform.parent.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else if (direction == 4)
        {
            this.gameObject.transform.parent.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
        else if (direction == 5)
        {
            this.gameObject.transform.parent.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        }
    }
    //ワープ中
    private void InWarp(int direction, float speed)
    {
        if (direction == 0)
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - speed, player.transform.position.z);
        }
        else if (direction == 1)
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + speed, player.transform.position.z);
        }
        else if (direction == 2)
        {
            player.transform.position = new Vector3(player.transform.position.x - speed, player.transform.position.y, player.transform.position.z);
        }
        else if (direction == 3)
        {
            player.transform.position = new Vector3(player.transform.position.x + speed, player.transform.position.y, player.transform.position.z);
        }
        else if (direction == 4)
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - speed);
        }
        else if (direction == 5)
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + speed);
        }
    }

    public void StartWarp()
    {
        if (player != null)
        {
            pCon.SendCanMoveFlag(false);
            progressNum++;
            playerPos = player.transform.position;
            playerOldPos = playerPos;
        }
    }

    //接続先に渡す変数
    public Vector3 GetPos()
    {
        return this.transform.position;
    }
    public int GetDirection()
    {
        return direction;
    }

}
