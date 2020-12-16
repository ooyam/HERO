using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemWave_Controller : MonoBehaviour
{
    //落下速度
    private float speed = -2.0f;
    //Transformのキャッシュ
    private Transform _transform;
    //Butoonのゲームオブジェクト/RectTransformを入れる
    private GameObject Wave;
    private RectTransform WaveTra;
    //時間計算用変数
    private float delta;
    //Playerを入れる
    private GameObject Player;
    //Playerのスクリプトを入れる
    private Player_Controller PlayerScr;
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
        Wave = GameObject.Find("WaveButton");
        WaveTra = Wave.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;
        //落下
        if (this._transform.position.y > -6)
        {
            _transform.Translate(0, speed * Time.deltaTime, 0, Space.World);
        }
        else if(delta >= 0.5f)
        {
            //自身を破壊
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Player" || WaveContact == true) && delta >= 0.5f)
        {
            //SEを呼ぶ
            PlayerScr.ItemSE();
            //ItemButtonを押せる状態にする
            WaveTra.anchoredPosition = new Vector2(0, -35);
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
