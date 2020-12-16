using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemRepair_Controller : MonoBehaviour
{
    //落下速度
    private float speed = -2.0f;
    //Transformのキャッシュ
    private Transform _transform;
    //Butoonのゲームオブジェクト/RectTransformを入れる
    private GameObject Repair;
    private RectTransform RepairTra;
    //Playerを入れる
    private GameObject Player;
    //Playerのスクリプトを入れる
    private Player_Controller PlayerScr;
    //時間計算
    private float delta;
    //WaveContact用変数
    private bool WaveContact;

    // Start is called before the first frame update
    void Start()
    {
        //自身のTransformのキャッシュ
        _transform = GetComponent<Transform>();
        //Playerを取得
        Player = GameObject.Find("Player");
        //Playerのスクリプトを取得
        PlayerScr = Player.GetComponent<Player_Controller>();
        //Butoonの取得
        Repair = GameObject.Find("RepairButton");
        RepairTra = Repair.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //落下
        if (this._transform.position.y > -6)
        {
            _transform.Translate(0, speed * Time.deltaTime, 0, Space.World);
        }
        else
        {
            //自身を破壊
            Destroy(this.gameObject);
        }
        //時間計算
        delta += Time.deltaTime;
    }
    void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.tag == "Player" || WaveContact == true )&& delta >= 0.5f)
        {
            //SEを呼ぶ
            PlayerScr.ItemSE();
            //ItemButtonを押せる状態にする
            RepairTra.anchoredPosition = new Vector2(-80, -35);
            //自身を破壊
            Destroy(this.gameObject);
        }
    }
    //パーティクル当たり判定
    void OnParticleCollision(GameObject obj)
    {
        //Waveに接触した際はRescue shipに直行
        if (obj.gameObject.tag == "Wave")
        {
            WaveContact = true;
        }
    }
}