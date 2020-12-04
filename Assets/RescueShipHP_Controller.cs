using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueShipHP_Controller : MonoBehaviour
{
    //自身のtransformを入れる
    private Transform mytransform;
    //Plyaer HP
    private float HP = 151.0f;
    //xposition計算用
    private float x;

    // Start is called before the first frame update
    void Start()
    {
        //自身のtransformを取得
        mytransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = new Vector3(x * -0.0033333333333333f, 0, -0.1f);
        mytransform.localScale = new Vector3(this.HP * 0.00662252f, 1, 1);
    }
    public void Scale()
    {
        this.HP -= 10;
        if (HP < 0)
        {
            HP = 0;
        }
        else
        {
            x = 151 - this.HP;
        }
    }
}
