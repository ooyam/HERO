using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour
{
    //x軸稼働限度
    private float XRange = 6;
    //x軸中央値
    private float XcenterRange = 2;
    //y軸稼働限度
    private float YRange = 2.14f;
    //x軸移動速度
    private float EnemySpeedX = 3;
    //y軸移動速度
    private float EnemySpeedY = 4;
    //初期位置ランダム用変数
    private int pos;
    //行動パターンランダム用変数
    private int Pattern;
    //往復動作用変数
    private float sin;
    //初期移動停止用変数
    private int stop;
    //保持用変数
    private int keep;

    //円運動の移動速度
    private float Rotspeed = 4f;
    //円を描く半径
    private float radius = 5f;
    //回転運動のx軸用変数
    float sinx;
    //回転運動のz軸用変数
    float cosy;

    //スパイクボールを入れる
    public GameObject spikeballPrefab;
    //エフェクトを入れる
    public GameObject Effect;
    //時間計算用変数
    private float delta;

    //接触検知用変数
    private bool Contact = false;
    //Playerのゲームオブジェクトを入れる
    private GameObject Player;
    //score_textのゲームオブジェクトを入れる
    private GameObject ScoreText;
    //GameOver_Textのゲームオブジェクトを入れる
    private GameObject GameOverText;
    //Playerのアニメーションコンポーネントを入れる
    private Animator PlayerAnimator;
    //Playerアニメーション状態取得用変数
    private bool Slide;
    private bool SlideStart;

    // Start is called before the first frame update
    void Start()
    {
        //定位置のランダム指定
        pos = Random.Range(-1, 5);
        //定位置まで移動条件
        stop = 0;
        //行動パターンのランダム選出
        Pattern = Random.Range(3,5);

        //Playerのゲームオブジェクトとアニメーターコンポーネントの取得
        this.Player = GameObject.Find("Player");
        this.PlayerAnimator = Player.GetComponent<Animator>();

        //score_textゲームオブジェクトの取得
        ScoreText = GameObject.Find("score_text");
        //GameOver_Textゲームオブジェクトの取得
        GameOverText = GameObject.Find("GameOver_Text");
    }
    // Update is called once per frame
    void Update()
    {
        //定位置まで移動
        if (this.transform.position.x > pos * 2 && stop == 0)
        {
            transform.Translate(-EnemySpeedY * Time.deltaTime, 0, 0);
        }

        if (this.transform.position.x <= pos * 2 || keep >= 1)
        {
            keep = 1;
            stop = 1;
            delta += Time.deltaTime;
            //パターン1
            if (Pattern == 1)
            {
                sin = Mathf.Sin(Time.time * EnemySpeedY);
                transform.position = new Vector2(pos * 2, sin * YRange);
            }
            //パターン2
            else if (Pattern == 2)
            {
                sin = Mathf.Sin(Time.time * EnemySpeedX);
                transform.position = new Vector2(sin * XRange + XcenterRange, this.transform.position.y);
            }
            //パターン3
            else if (Pattern == 3)
            {
                //X軸の設定
                sinx = radius * Time.deltaTime * Mathf.Sin(Time.time * Rotspeed);
                //y軸の設定
                cosy = radius * Time.deltaTime * Mathf.Cos(Time.time * Rotspeed);
                //自分のいる位置から座標を動かす。
                this.transform.position = new Vector2(sinx + this.transform.position.x, cosy + this.transform.position.y);
            }
            //パターン4
            else if (Pattern == 4)
            {
                sinx = radius * Time.deltaTime * Mathf.Sin(Time.time * Rotspeed);
                cosy = radius * Time.deltaTime * Mathf.Cos(Time.time * Rotspeed);
                this.transform.position = new Vector2(this.transform.position.x + sinx, this.transform.position.y - cosy);
            }
            //1個/2秒スパイクボール生成
            if (delta >= 2)
            {
                GameObject Spikeball = Instantiate(spikeballPrefab);
                Spikeball.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
                delta = 0;
            }
        }

        // Playerアニメーションの状態取得
        Slide = PlayerAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Slide"));
        SlideStart = PlayerAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Slide-Start"));
        //plyaer攻撃時に接触したら破壊
        if (Contact == true)
        {
            //ポイントの加算(score_textの呼び出し)
            ScoreText.GetComponent<score_text_Controller>().EnemyScore();
            GameOverText.GetComponent<GameOver_Text_Controller>().Enemy1Score();
            //Effectを呼び出す
            GameObject effect = Instantiate(Effect);
            effect.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
            //破壊
            Destroy(this.gameObject);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.tag == "Player" && (Slide == true || SlideStart == true)))
        {
            Contact = true;
        }
    }
    //パーティクル当たり判定
    void OnParticleCollision(GameObject obj)
    {
        Contact = true;
    }
}