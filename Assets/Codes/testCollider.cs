using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCollider : MonoBehaviour
{
    GameObject player;
    GameObject me;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        me = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        me.transform.position = new Vector3(player.transform.position.x, player.transform.position.y-0.5f, player.transform.position.z);
    }
}
