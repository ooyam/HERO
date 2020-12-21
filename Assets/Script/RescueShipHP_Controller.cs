using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueShipHP_Controller : MonoBehaviour
{
    //自身のtransformを入れる
    private Transform mytransform;
    //Plyaer HP
    private float HP;
    //xposition計算用
    private float x;

    //Rescueshipを入れる
    private GameObject Rescueship;
    //Rescueshipのスクリプトを入れる
    private Rescueship_controller RescueshipScr;

    // Start is called before the first frame update
    void Start()
    {
        //自身のtransformを取得
        mytransform = transform;

        //Rescueshipを取得
        Rescueship = GameObject.Find("Rescue ship");
        //Rescueshipのスクリプトを取得
        RescueshipScr = Rescueship.GetComponent<Rescueship_controller>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Scale()
    {
        //RescueshipのHPを参照する
        this.HP = RescueshipScr.HP;
        if (HP < 0)
        {
            HP = 0;
        }
        else
        {
            x = 151 - this.HP;
        }
        //ゲージの増減
        mytransform.localPosition = new Vector3(x * -0.0033333333333333f, 0, -0.1f);
        mytransform.localScale = new Vector3(this.HP * 0.00662252f, 1, 1);
    }
}