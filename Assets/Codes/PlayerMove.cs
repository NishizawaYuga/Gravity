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

    //�I�u�W�F�N�g
    [SerializeField]
    [Tooltip("�v���C���[�I�u�W�F�N�g��u��")]
    private GameObject target;

    //�J����
    [SerializeField]
    [Tooltip("�J����")]
    private GameObject mainCamera;

    //�X�N���v�g
    PlayerController script;
    UnderCollider underCollider;
    private CaemeraFollowTarget cF;     //�J�����Ǐ]����

    //�W�����v�t���O
    bool isJump = false;
    //�W�����v���̐���
    float jumpPower = 3f;

    void Start()
    {
        TryGetComponent(out rb);
        //�X�N���v�g�o�^
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
            //�ړ�
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

    //�ړ�
    private void Move()
    {
        //�J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
        if (script.GetNum() == 0 || script.GetNum() == 1)
        {
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 0).normalized);
            //�����L�[�̓��͒l���J�����̌�������ړ�����������
            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizotal;
            //�ړ������ɃX�s�[�h���|����B�W�����v�◎��������ꍇ�́A�ʓrY�������̑��x�x�N�g���𑫂�
            //�d�͕����ɉ����ăW�����v�����̑��x�x�N�g���ʒu�ύX
            rb.velocity = moveForward * moveSpeed + new Vector3(0, rb.velocity.y +Vector3.up.y * Jump(), 0);
            //�L�[���͂ɂ��ړ����������܂��Ă���ꍇ�ɂ́A�L�����N�^�[�̌�����i�s�����ɍ��킹��
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
        }
        else if (script.GetNum() == 2 || script.GetNum() == 3)
        {
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(0, 1, 0).normalized);
            //�����L�[�̓��͒l���J�����̌�������ړ�����������
            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizotal;
            //�ړ������ɃX�s�[�h���|����B�W�����v�◎��������ꍇ�́A�ʓrY�������̑��x�x�N�g���𑫂�
            //�d�͕����ɉ����ăW�����v�����̑��x�x�N�g���ʒu�ύX
            rb.velocity = moveForward * moveSpeed + new Vector3(rb.velocity.x + Vector3.up.y * Jump(), 0, 0);
            //�L�[���͂ɂ��ړ����������܂��Ă���ꍇ�ɂ́A�L�����N�^�[�̌�����i�s�����ɍ��킹��
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
        }
        else if (script.GetNum() == 4 || script.GetNum() == 5)
        {
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(0, 1, 0).normalized);
            //�����L�[�̓��͒l���J�����̌�������ړ�����������
            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizotal;
            //�ړ������ɃX�s�[�h���|����B�W�����v�◎��������ꍇ�́A�ʓrY�������̑��x�x�N�g���𑫂�
            //�d�͕����ɉ����ăW�����v�����̑��x�x�N�g���ʒu�ύX
            rb.velocity = moveForward * moveSpeed + new Vector3(0, 0, rb.velocity.z + Vector3.up.y * Jump());
            //�L�[���͂ɂ��ړ����������܂��Ă���ꍇ�ɂ́A�L�����N�^�[�̌�����i�s�����ɍ��킹��
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
        }
    }

    private float Jump()
    {
        //�ڒn����
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
        //�������̏����ʒu�Ǝp��
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
        //�����̋���
        var distance = 0.06f;

        Debug.DrawRay(ray.origin, new Vector3(0f,distance,0f), Color.red, 0.05f);

        return Physics.Raycast(ray, distance);
    }

    //�l�X�ȓ����蔻��
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
    //�v���C���[���N���ł����肷��L���͈�
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
