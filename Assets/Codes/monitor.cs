using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.XR.OpenVR;
using UnityEngine;

public class monitor : MonoBehaviour
{
    //プレイヤーオブジェクト
    [SerializeField]
    [Tooltip("プレイヤーオブジェクト")]
    private GameObject player;

    private Easing ease;

    private bool viewMode = false;
    private int maxTimer = 30;
    private int timer = 0;

    void Start()
    {
        ease = new Easing();
        this.gameObject.transform.localScale = new Vector3(0.5f, 0f, 0.5f);
    }

    void FixedUpdate()
    {
        if (player.transform.position.x < this.gameObject.transform.position.x + 1.0f && player.transform.position.x > this.gameObject.transform.position.x - 1.0f)
        {
            if (!viewMode)
            {
                if (timer < maxTimer)
                {
                    this.gameObject.transform.localScale = new Vector3(0.5f, ease.OutQuad(0.5f, 0f, maxTimer, timer), 0.5f);
                    timer++;
                }
                else
                {
                    viewMode = true;
                    timer = 0;
                    this.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
            }
        }
        //else
        //{
        //    if (viewMode)
        //    {
        //        if (timer < maxTimer)
        //        {
        //            this.gameObject.transform.localScale = new Vector3(0.5f, ease.OutQuad(-0.5f, 0.5f, maxTimer, timer), 0.5f);
        //            timer++;
        //        }
        //        else
        //        {
        //            viewMode = false;
        //            timer = 0;
        //            this.gameObject.transform.localScale = new Vector3(0.5f, 0f, 0.5f);
        //        }
        //    }
        //}
    }
}
