using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    [SerializeField]
    private GameObject gm;

    //•`‰æŒn
    private const float maxSize = 0.25f;
    private const float minSize = 0f;
    private int maxTimer = 30;
    private int nowTimer = 0;
    Easing ease;
    private bool inStart = false;
    private bool inEnd = false;

    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        ease = new Easing();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inStart)
        {
            InAnimation();
        }
        if (Input.GetKeyDown(KeyCode.Space) && inStart)
        {
            inEnd = true;
        }
        if(inEnd)
        {
            EndAnimation();
        }
    }

    void InAnimation()
    {
        if (nowTimer < maxTimer)
        {
            rectTransform.localScale = new Vector2(0.25f,ease.InOutQuad(maxSize, minSize, maxTimer, nowTimer));
            nowTimer++;
        }
        else
        {
            nowTimer = 0;
            rectTransform.localScale = new Vector2(0.25f, maxSize);
            inStart = true;
        }
    }
    void EndAnimation()
    {
        if (nowTimer < maxTimer)
        {
            rectTransform.localScale = new Vector2(0.25f, ease.InOutQuad(-maxSize, maxSize, maxTimer, nowTimer));
            nowTimer++;
        }
        else
        {
            nowTimer = 0;
            rectTransform.localScale = new Vector2(0.25f, minSize);
            inStart = true;
            gm.GetComponent<GameManager>().ChangeScene("testStage3");
        }
    }
}
