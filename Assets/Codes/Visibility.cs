using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Visibility : MonoBehaviour
{
    //視界
    GameObject visibility;
    //発見フラグ
    bool search = false;

    // Start is called before the first frame update
    void Start()
    {
        visibility = this.gameObject;
    }

    //セッター・ゲッター
    public void SetSearch(bool flag)
    {
        search = flag;
    }
    public bool GetSearch()
    {
        return search;
    }    
    //索敵有効チェック
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            search = true;
        }
        else
        {
            search = false;
        }
    }
}
