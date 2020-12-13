using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Controller : MonoBehaviour
{
    //移動速度
    private float speed = 4.0f;
    //時間計算用変数
    private float delta;
    //スパイクボール生成時間
    private float AttackSpeed = 2f;
    //Level取得用変数
    private float Level = 1f;
    //transformのキャッシュ
    private Transform _transform;
    //Rendererのキャッシュ
    private Renderer _renderer;

    //スパイクボールを入れる
    public GameObject SpikeSmall;
    //エフェクトを入れる
    public GameObject Effect;
    //SEを入れる
    private AudioSource SE;


    //耐久値
    private float hp = 30.0f;
    //接触検知用変数
    private bool Contact = false;
    //接触回数計算用変数
    private int Counter = 0;
    //Playerのゲームオブジェクトを入れる
    private GameObject Player;
    //score_textのゲームオブジェクト/スクリプトを入れる
    private GameObject ScoreText;
    private score_text_Controller ScoreTextScr;
    //GameOver_Textのゲームオブジェクト/スクリプトを入れる
    private GameObject GameOverText;
    private GameOver_Text_Controller GameOverTextScr;
    //EnemyGeneratorのゲームオブジェクト/スクリプトを入れる
    private GameObject EnemyGenerator;
    private EnemyGenerator GeneratorScr;
    //Playerのアニメーションコンポーネントを入れる
    private Animator PlayerAnimator;
    //Playerアニメーション状態取得用変数
    private bool Slide;
    private bool SlideStart;

    // Start is called before the first frame update
    void Start()
    {
        //transformのキャッシュ
        _transform = GetComponent<Transform>();
        //Rendererのキャッシュ
        _renderer = GetComponent<Renderer>();
        //Playerのゲームオブジェクトとアニメーターコンポーネントの取得
        this.Player = GameObject.Find("Player");
        this.PlayerAnimator = Player.GetComponent<Animator>();

        //score_textゲームオブジェクト/スクリプトの取得
        ScoreText = GameObject.Find("score_text");
        ScoreTextScr = ScoreText.GetComponent<score_text_Controller>();
        //score_textのLevelと同期させ､スパイクボールの生成時間を計算する
        this.Level = (ScoreTextScr.Level - 1) / 10f + 1;
        this.AttackSpeed = this.AttackSpeed / this.Level;
        //GameOver_Textゲームオブジェクト/スクリプトの取得
        GameOverText = GameObject.Find("GameOver_Text");
        GameOverTextScr = GameOverText.GetComponent<GameOver_Text_Controller>();
        //SEを取得
        SE = GetComponent<AudioSource>();

        //EnemyGeneratorゲームオブジェクト/スクリプトの取得
        EnemyGenerator = GameObject.Find("EnemyGenerator");
        GeneratorScr = EnemyGenerator.GetComponent<EnemyGenerator>();
        //自身の位置を取得しGeneratorへ共有
        if (this._transform.position.y >= 0)
        {
            GeneratorScr.UpTrue();
        }
        else if(this._transform.position.x >= 0)
        {
            GeneratorScr.UnderRightTrue();
        }
        else
        {
            GeneratorScr.UnderLeftTrue();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //定位置まで移動
        if (this._transform.position.y > 3.5)
        {
            _transform.Translate(0, -speed * Time.deltaTime, 0);
        }
        else if (this._transform.position.y < -4)
        {
            _transform.Translate(0, speed * Time.deltaTime, 0);
        }

        //時間計算
        delta += Time.deltaTime;

        //スパイクボール生成
        if (delta >= this.AttackSpeed)
        {
            GameObject Spikeball = Instantiate(SpikeSmall);
            Spikeball.transform.position = new Vector2(this._transform.position.x, this._transform.position.y);
            delta = 0;
        }

        // Playerアニメーションの状態取得
        Slide = PlayerAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Slide"));
        SlideStart = PlayerAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Slide-Start"));
        //耐久値が0になったら破壊
        if (Contact == true && Counter ==0)
        {
            //SEを出す
            SE.Play();
            delta = 0f;
            Counter++;
            this.hp -= 10f;
            if (hp <= 0)
            {
                //ポイントの加算(score_textの呼び出し)
                ScoreTextScr.Enemy2Score();
                GameOverTextScr.Enemy2Score();
                //Effectを呼び出す
                GameObject effect = Instantiate(Effect);
                effect.transform.position = new Vector2(this._transform.position.x, this._transform.position.y);
                //破壊
                Destroy(this.gameObject);
                //自身の位置を取得しGeneratorへ共有
                if (this._transform.position.y >= 0)
                {
                    GeneratorScr.UpFalse();
                }
                else if (this._transform.position.x >= 0)
                {
                    GeneratorScr.UnderRightFalse();
                }
                else
                {
                    GeneratorScr.UnderLeftFalse();
                }
            }
        }
        if (Counter >= 1)
        {
            //ダメージが入ると点滅(赤)
            _renderer.material.color = new Color32(255, 0, 0, 150);
            delta += Time.deltaTime;
            if (delta >= 0.3f)
            {
                _renderer.material.color = new Color32(255, 255, 255, 255);
                Contact = false;
                Counter = 0;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && (Slide == true || SlideStart == true))
        {
            Contact = true;
        }
    }
    //パーティクル当たり判定
    void OnParticleCollision(GameObject obj)
    {
        //Waveに接触した際は即破壊
        if(obj.gameObject.tag == "Wave")
        {
            this.hp -= 30;
        }
        Contact = true;
    }
}
