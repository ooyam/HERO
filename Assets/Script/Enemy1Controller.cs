using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour
{
    //x軸稼働限度
    private float XRange = 6;
    //x軸中央値
    private float XCenterRange = 2;
    //y軸稼働限度
    private float YRange = 2.14f;
    //x軸移動速度
    private float EnemySpeedX = 3;
    //y軸移動速度
    private float EnemySpeedY = 4;
    //画面右出現時の初期位置ランダム用変数
    private int RightPos;
    //画面上下出現時の初期位置ランダム用変数
    private int HightPos;
    //出現位置判断用座標
    private int HightLine = 6;
    private int RightLine = 9;
    //初期位置を記憶するための変数
    private Vector3 InitialPos;
    //行動パターンランダム用変数
    private int Pattern;
    //往復動作用変数
    private float sin;
    //初期位置移動完了変数
    private bool Done;
    //transformのキャッシュ
    private Transform _transform;

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
    public GameObject spikeball_Red;
    //エフェクトを入れる
    public GameObject Effect;
    //Itemを入れる
    public GameObject ItemWave;
    public GameObject ItemRepair;
    public GameObject ItemRecovery;
    //ItemDropランダム用変数
    private int Drop;
    //時間計算用変数
    private float delta;

    //接触検知用変数
    private bool Contact = false;
    //Playerのゲームオブジェクトを入れる
    private GameObject Player;
    //score_textのゲームオブジェクトを入れる
    private GameObject ScoreText;
    //score_textのスクリプトを入れる
    private score_text_Controller ScoreTextScr;
    //GameOver_Textのゲームオブジェクトを入れる
    private GameObject GameOverText;
    //GameOver_Textのスクリプトを入れる
    private GameOver_Text_Controller GameOverTextScr;
    //Playerのアニメーションコンポーネントを入れる
    private Animator PlayerAnimator;
    //Playerアニメーション状態取得用変数
    private bool Slide;
    private bool SlideStart;
    //WaveEffectの有無確認用変数
    private int WaveCounts;

    // Start is called before the first frame update
    void Start()
    {
        //transformのキャッシュ
        _transform = GetComponent<Transform>();
        //初期位置を記憶
        InitialPos = this._transform.position;
        //定位置のランダム指定
        RightPos = Random.Range(-1, 5);
        //定位置のランダム指定
        HightPos = Random.Range(-2, 3);
        //行動パターンのランダム選出
        Pattern = Random.Range(1, 5);
        //DropItemのランダム指定
        Drop = Random.Range(1, 76);
        //Playerのゲームオブジェクトとアニメーターコンポーネントの取得
        Player = GameObject.Find("Player");
        PlayerAnimator = Player.GetComponent<Animator>();
        //score_textゲームオブジェクト/スクリプトの取得
        ScoreText = GameObject.Find("score_text");
        ScoreTextScr = ScoreText.GetComponent<score_text_Controller>();
        //GameOver_Textゲームオブジェクト/スクリプトの取得
        GameOverText = GameObject.Find("GameOver_Text");
        GameOverTextScr = GameOverText.GetComponent<GameOver_Text_Controller>();
    }
    // Update is called once per frame
    void Update()
    {
        //定位置まで移動
        if (Done == false)
        {
            if (InitialPos.y >= HightLine && this._transform.position.y > HightPos)
            {
                _transform.Translate(0, -EnemySpeedY * Time.deltaTime, 0);
            }
            else if (InitialPos.y <= -HightLine && this._transform.position.y < HightPos)
            {
                _transform.Translate(0, EnemySpeedY * Time.deltaTime, 0);
            }
            else if (InitialPos.x >= RightLine && this._transform.position.x > RightPos * 2)
            {
                _transform.Translate(-EnemySpeedY * Time.deltaTime, 0, 0);
            }
            else
            {
                Done = true;
            }
        }
        //定位置まで移動後､攻撃開始
        if (Done == true)
        {
            delta += Time.deltaTime;
            //パターン1
            if (Pattern == 1)
            {
                sin = Mathf.Sin(Time.time * EnemySpeedY);
                _transform.position = new Vector2(this._transform.position.x, sin * YRange);
            }
            //パターン2
            else if (Pattern == 2)
            {
                sin = Mathf.Sin(Time.time * EnemySpeedX);
                _transform.position = new Vector2(sin * XRange + XCenterRange, this.transform.position.y);
            }
            //パターン3
            else if (Pattern == 3)
            {
                //X軸の設定
                sinx = radius * Time.deltaTime * Mathf.Sin(Time.time * Rotspeed);
                //y軸の設定
                cosy = radius * Time.deltaTime * Mathf.Cos(Time.time * Rotspeed);
                //自分のいる位置から座標を動かす。
                this._transform.position = new Vector2(sinx + this._transform.position.x, cosy + this._transform.position.y);
            }
            //パターン4
            else if (Pattern == 4)
            {
                sinx = radius * Time.deltaTime * Mathf.Sin(Time.time * Rotspeed);
                cosy = radius * Time.deltaTime * Mathf.Cos(Time.time * Rotspeed);
                this._transform.position = new Vector2(this._transform.position.x + sinx, this._transform.position.y - cosy);
            }
            //1個/2秒スパイクボール生成
            if (delta >= 2)
            {
                GameObject Spikeball = Instantiate(spikeballPrefab);
                Spikeball.transform.position = new Vector2(this._transform.position.x, this._transform.position.y);
                //Enemy1_2の場合は救助船も攻撃
                if (InitialPos.x <= 9f)
                {
                    GameObject SpikeballRed = Instantiate(spikeball_Red);
                    SpikeballRed.transform.position = new Vector2(this._transform.position.x, this._transform.position.y);
                }
                delta = 0;
            }
        }

        // Playerアニメーションの状態取得
        Slide = PlayerAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Slide"));
        SlideStart = PlayerAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Slide-Start"));
        //plyaer攻撃時に接触したら破壊
        if (Contact == true)
        {
            //WaveEffectの有無
            WaveCounts = GameObject.FindGameObjectsWithTag("WaveEffect").Length;
            //ポイントの加算(score_textの呼び出し)
            ScoreTextScr.EnemyScore();
            GameOverTextScr.Enemy1Score();
            //Effectの生成
            GameObject effect = Instantiate(Effect);
            effect.transform.position = new Vector2(this._transform.position.x, this._transform.position.y);
            //ItemWaveの生成
            if(Drop == 1 && WaveCounts == 0)
            {
                GameObject Item = Instantiate(ItemWave);
                Item.transform.position = new Vector2(this._transform.position.x, this._transform.position.y);
            }
            //ItemRepairの生成
            else if (Drop <= 4)
            {
                GameObject Item = Instantiate(ItemRepair);
                Item.transform.position = new Vector2(this._transform.position.x, this._transform.position.y);
            }
            //ItemRecoveryの生成
            else if (Drop <= 7)
            {
                GameObject Item = Instantiate(ItemRecovery);
                Item.transform.position = new Vector2(this._transform.position.x, this._transform.position.y);
            }
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