using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemRepair_Controller : MonoBehaviour
{
    //落下速度
    private float speed = -2.0f;
    //回転体を入れる
    private GameObject ItemRot;
    private GameObject Rot;
    //回転体transform取得用
    private Transform ItemRotTor;
    //回転体回転速度
    private float rotSpeed = 50f;
    //Butoonを入れる
    private GameObject Recovery;
    //Playerを入れる
    private GameObject Player;
    //Playerのスクリプトを入れる
    private Player_Controller PlayerScr;

    // Start is called before the first frame update
    void Start()
    {

        //回転体を取得
        ItemRot = GameObject.Find("RepairBallLines");
        //回転体transform取得用
        ItemRotTor = ItemRot.GetComponent<Transform>();
        //Butoonの取得
        Recovery = GameObject.Find("RepairButton");
        //Playerを取得
        Player = GameObject.Find("Player");
        //Playerのスクリプトを取得
        PlayerScr = Player.GetComponent<Player_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        //落下
        if (this.transform.position.y > -6)
        {
            transform.Translate(0, speed * Time.deltaTime, 0, Space.World);
        }
        else
        {
            //自身を破壊
            Destroy(this.gameObject);
        }
        //回転
        ItemRotTor.Rotate(0, this.rotSpeed * Time.deltaTime, 0, Space.World);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //SEを呼ぶ
            PlayerScr.ItemSE();
            //ItemButtonを押せる状態にする
            Recovery.GetComponent<RectTransform>().anchoredPosition = new Vector2(-80, -35);
            //自身を破壊
            Destroy(this.gameObject);
        }
    }
}