using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plyaer_HP_Controller : MonoBehaviour
{
    //自身のtransformを入れる
    private Transform mytransform;
    //Plyaer HP
    private int HP;
    //xposition計算用
    private float x;

    //Plyaerを入れる
    private GameObject Player;
    //Playerのスクリプトを入れる
    private Player_Controller PlayerScr;

    // Start is called before the first frame update
    void Start()
    {
        //自身のtransformを取得
        mytransform = transform;
        //Plyaerを取得
        Player = GameObject.Find("Player");
        //Playerのスクリプトを取得
        PlayerScr = Player.GetComponent<Player_Controller>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Scale()
    {
        //PlayerのHPを参照する
        this.HP = PlayerScr.HP;
        if (HP < 0)
        {
            this.HP = 0;
        }
        else
        {
            x = 100 - this.HP;
        }
        //ゲージを増減する
        this.transform.localPosition = new Vector3(x * -0.005f, 0, -0.1f);
        mytransform.localScale = new Vector3(this.HP * 0.01f, 1, 1);
    }
}
