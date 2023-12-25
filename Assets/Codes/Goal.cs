using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    //増減変数
    float rotY = 0f;
    private const float rotX = -90f;
    //クリアフラグ
    bool isClear = false;

    //クリア後処理
    private int waitTimer = 60;
    private const int maxWaitTimer = 60;

    GameObject goal;

    [SerializeField]
    [Tooltip("変わる箇所は回転方向")]
    private int direction;

    void Start()
    {
        goal = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        rotY += 0.8f;
        if (direction == 0)
        {
            goal.transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        }
        else if (direction == 1)
        {
            goal.transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        }
        else if (direction == 2)
        {
            goal.transform.rotation = Quaternion.Euler(0, rotX, rotY);
        }
        else if (direction == 3)
        {
            goal.transform.rotation = Quaternion.Euler(0, rotX, rotY);
        }
        else if (direction == 4)
        {
            goal.transform.rotation = Quaternion.Euler(rotY, 90, 0);
        }
        else if (direction == 5)
        {
            goal.transform.rotation = Quaternion.Euler(rotY, -90, 0);
        }

        if (rotY > 180f)
        {
            rotY = -180f;
        }

        GoalScene();
    }
    public void GetStar()
    {
        //goal.SetActive(false);
        goal.transform.position = new Vector3(0f, 0f, 0f);
        isClear = true;
    }
    private void GoalScene()
    {
        if (isClear)
        {
            if (waitTimer > 0)
            {
                waitTimer--;
            }
            else
            {
                waitTimer = maxWaitTimer;
                isClear = false;
                SceneManager.LoadScene("title");
            }
        }
    }
}
