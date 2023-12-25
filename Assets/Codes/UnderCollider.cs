using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderCollider : MonoBehaviour
{
    //4箇所当たり判定
    [SerializeField]
    [Tooltip("当たり判定用のオブジェクトを置く")]
    private GameObject collider1;
    [SerializeField]
    [Tooltip("当たり判定用のオブジェクトを置く")]
    private GameObject collider2;
    [SerializeField]
    [Tooltip("当たり判定用のオブジェクトを置く")]
    private GameObject collider3;
    [SerializeField]
    [Tooltip("当たり判定用のオブジェクトを置く")]
    private GameObject collider4;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
