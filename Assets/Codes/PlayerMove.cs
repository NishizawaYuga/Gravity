using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float inputHorizotal;
    private float inputVertical;
    private Rigidbody rb;

    [SerializeField]
    private float moveSpeed;

    //オブジェクト
    [SerializeField]
    [Tooltip("プレイヤーオブジェクトを置く")]
    private GameObject target;

    //カメラ
    [SerializeField]
    [Tooltip("カメラ")]
    private GameObject mainCamera;

    //スクリプト
    PlayerController script;
    UnderCollider underCollider;
    private CaemeraFollowTarget cF;     //カメラ追従制御

    //ジャンプフラグ
    bool isJump = false;
    //ジャンプ時の勢い
    float jumpPower = 3f;

    void Start()
    {
        TryGetComponent(out rb);
        //スクリプト登録
        script = target.GetComponent<PlayerController>();
        underCollider = target.GetComponent<UnderCollider>();
        cF = mainCamera.GetComponent<CaemeraFollowTarget>();
    }

    // Update is called once per frame
    void Update()
    {
        inputHorizotal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (script.GetCanMove() && !script.IsDead())
        {
            //移動
            Move();
        }
        else if(!script.GetCanMove() && !script.IsDead())
        {
            rb.velocity = new Vector3(0f, 0f, 0f);
        }
        else if (!script.GetCanMove() &&  script.IsDead())
        {

        }
    }

    //移動
    private void Move()
    {
        //カメラの方向から、X-Z平面の単位ベクトルを取得
        if (script.GetNum() == 0 || script.GetNum() == 1)
        {
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 0).normalized);
            //方向キーの入力値をカメラの向きから移動方向を決定
            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizotal;
            //移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す
            //重力方向に応じてジャンプ方向の速度ベクトル位置変更
            rb.velocity = moveForward * moveSpeed + new Vector3(0, rb.velocity.y +Vector3.up.y * Jump(), 0);
            //キー入力により移動方向が決まっている場合には、キャラクターの向きを進行方向に合わせる
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
        }
        else if (script.GetNum() == 2 || script.GetNum() == 3)
        {
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(0, 1, 0).normalized);
            //方向キーの入力値をカメラの向きから移動方向を決定
            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizotal;
            //移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す
            //重力方向に応じてジャンプ方向の速度ベクトル位置変更
            rb.velocity = moveForward * moveSpeed + new Vector3(rb.velocity.x + Vector3.up.y * Jump(), 0, 0);
            //キー入力により移動方向が決まっている場合には、キャラクターの向きを進行方向に合わせる
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
        }
        else if (script.GetNum() == 4 || script.GetNum() == 5)
        {
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(0, 1, 0).normalized);
            //方向キーの入力値をカメラの向きから移動方向を決定
            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizotal;
            //移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す
            //重力方向に応じてジャンプ方向の速度ベクトル位置変更
            rb.velocity = moveForward * moveSpeed + new Vector3(0, 0, rb.velocity.z + Vector3.up.y * Jump());
            //キー入力により移動方向が決まっている場合には、キャラクターの向きを進行方向に合わせる
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
        }
    }

    private float Jump()
    {
        //接地判定
        isJump = CheckGrounded();

        if (Input.GetKey(KeyCode.Space))
        {
            if (isJump)
            {
                if (script.GetNum() % 2 == 0)
                {
                    return jumpPower;
                }
                else if (script.GetNum() % 2 == 1)
                {
                    return -jumpPower;
                }
            }
        }
        return 0.0f;
    }

    bool CheckGrounded()
    {
        Vector3 direction = new Vector3(0,0,0);
        //放つ光線の初期位置と姿勢
        if (script.GetNum() == 0)
        {
            direction = Vector3.down;
        }
        else if (script.GetNum() == 1)
        {
            direction = Vector3.up;
        }
        else if (script.GetNum() == 2)
        {
            direction = Vector3.left;
        }
        else if (script.GetNum() == 3)
        {
            direction = Vector3.right;
        }
        else if (script.GetNum() == 4)
        {
            direction = Vector3.back;
        }
        else if (script.GetNum() == 5)
        {
            direction = Vector3.forward;
        }
        var ray = new Ray(transform.position, direction);
        //光線の距離
        var distance = 0.06f;

        Debug.DrawRay(ray.origin, new Vector3(0f,distance,0f), Color.red, 0.05f);

        return Physics.Raycast(ray, distance);
    }

    //様々な当たり判定
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "pipehole")
        {
            other.gameObject.GetComponent<Pipe>().StartWarp();
        }
        if(other.gameObject.tag == "MissArea")
        {
            script.SendCanMoveFlag(false);
            //target.SetActive(false);
            cF.IsFollow(false);
            script.ChangeDead(true);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            script.SendCanMoveFlag(false);
            collision.gameObject.GetComponent<Goal>().GetStar();
            script.ChangeGravity(6);
        }
    }
    //プレイヤーが起動できたりする有効範囲
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "GravityArrow")
        {
            if(this.gameObject.transform.position.y > other.gameObject.transform.position.y - 0.3f && this.gameObject.transform.position.y < other.gameObject.transform.position.y + 0.3f) {
                if (Input.GetMouseButton(1))
                {
                    other.gameObject.GetComponent<GravityArrow>().ChangeGravity();
                }
            }
        }
    }
}
