using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Controller : MonoBehaviour
{
    // 回転速度
    private float rotSpeed = -200f;
    // 移動速度
    private float Speed = 3f;
    //Level取得用変数
    private float Level = 1f;
    //transformのキャッシュ
    private Transform _transform;
    //AudioSourceのキャッシュ
    private AudioSource Audio;

    //人1～4を入れる
    public GameObject Human1;
    public GameObject Human2;
    public GameObject Human3;
    public GameObject Human4;
    private int range;
    //gameobject取得用
    private GameObject human;
    //時間停止用変数
    private int stop = 0;

    // Materialを入れる
    private Material myMaterial;
    // ターゲットのデフォルトの色/変化後の色
    private Color DefaultColor = new Color32(20, 0, 0, 0);
    private Color ChangeColor = new Color32(255, 0, 0, 0);

    //接触検知用変数
    private bool Contact = false;
    //Playerのゲームオブジェクトを入れる
    private GameObject Player;
    //score_textのゲームオブジェクト/スクリプトを入れる
    private GameObject ScoreText;
    private score_text_Controller ScoreTextScr;
    //GameOver_Textのゲームオブジェクト/スクリプトを入れる
    private GameObject GameOverText;
    private GameOver_Text_Controller GameOverTextScr;
    //Playerのアニメーションコンポーネントを入れる
    private Animator PlayerAnimator;
    //エフェクトを入れる
    public GameObject Effect;
    //Playerアニメーション状態取得用変数
    private bool Slide;
    private bool SlideStart;

    // Start is called before the first frame update
    void Start()
    {
        //人のランダム生成
        range = Random.Range(1, 5);
        //transformのキャッシュ
        _transform = GetComponent<Transform>();

        //オブジェクトにアタッチしているMaterialを取得
        this.myMaterial = GetComponent<Renderer>().material;
        //AudioSourceを取得
        Audio = GetComponent<AudioSource>();

        //Playerのゲームオブジェクトとアニメーターコンポーネントの取得
        this.Player = GameObject.Find("Player");
        this.PlayerAnimator = Player.GetComponent<Animator>();
        //score_textゲームオブジェクト/スクリプトの取得
        ScoreText = GameObject.Find("score_text");
        ScoreTextScr = ScoreText.GetComponent<score_text_Controller>();
        //score_textのLevelと同期させ､スパイクボールの生成時間を計算する
        this.Level = (ScoreTextScr.Level - 1) / 20f + 1;
        this.Speed = this.Speed * this.Level;
        //GameOver_Textゲームオブジェクト/スクリプトの取得
        GameOverText = GameObject.Find("GameOver_Text");
        GameOverTextScr = GameOverText.GetComponent<GameOver_Text_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        //回転
        this._transform.Rotate(0, this.rotSpeed * Time.deltaTime, 0);

        // 移動
        if (this._transform.position.x >= -11)
        {
            _transform.Translate(-this.Speed * Time.deltaTime, 0, 0, Space.World);
        }
        //ゲームオーバー
        else
        {
            Destroy(this.gameObject);
            //時間停止
            Time.timeScale = 0;
            //ゲームオーバー画面の呼び出し
            GameOverTextScr.GameOverJudge("Enemy3");
            //score表示を消す
            ScoreTextScr.GameOverJudge();
        }

        //画面左端に来るとオブジェクト点滅
        if (this._transform.position.x <= -9)
        {
            if (stop == 0)
            {
                //時間停止
                Time.timeScale = 0;
                //WaitCoroutineを実行
                StartCoroutine(WaitTimeCoroutine());
                stop = 1;
            }
        }

        // Playerアニメーションの状態取得
        Slide = PlayerAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Slide"));
        SlideStart = PlayerAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Slide-Start"));
        //plyaer攻撃時に接触したら破壊
        if (Contact == true)
        {
            //Effectを呼び出す
            GameObject effect = Instantiate(Effect);
            effect.transform.position = new Vector2(this._transform.position.x, this._transform.position.y);
            //人を生成
            if (range == 1)
            {
                human = Instantiate(Human1);
            }
            else if (range == 2)
            {
                human = Instantiate(Human2);
            }
            else if (range == 3)
            {
                human = Instantiate(Human3);
            }
            else if (range == 4)
            {
                human = Instantiate(Human4);
            }
            human.transform.position = new Vector3(this._transform.position.x, this._transform.position.y,0);
            //ポイントの加算(score_textの呼び出し)
            ScoreTextScr.EnemyScore();
            GameOverTextScr.Enemy3Score();
            Destroy(this.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && (Slide == true || SlideStart == true) && this.transform.position.x < 9)
        {
            Contact = true;
        }
    }

    IEnumerator WaitTimeCoroutine()
    {
        for (int i = 0; i <= 2; i++)
        {
            if(Contact == true)
            {
                break;
            }
            //オブジェクトの点滅
            myMaterial.SetColor("_EmissionColor", this.ChangeColor);
            //SEを出す
            Audio.Play();
            yield return new WaitForSecondsRealtime(0.2f);
            myMaterial.SetColor("_EmissionColor", this.DefaultColor);
            yield return new WaitForSecondsRealtime(0.3f);
        }
        Time.timeScale = 1;
    }
    //パーティクル当たり判定
    void OnParticleCollision(GameObject obj)
    {
        //Waveに接触した際は即破壊
        if (obj.gameObject.tag == "Wave" && this.transform.position.x < 9)
        {
            Contact = true;
        }
    }
}
