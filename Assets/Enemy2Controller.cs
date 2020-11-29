using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Controller : MonoBehaviour
{
    //移動速度
    private float speed = 4.0f;
    //時間計算用変数
    private float delta;

    //スパイクボールを入れる
    public GameObject spikeball_bigPrefab;
    public GameObject spikeball_smallPrefab;


    //耐久値
    private float hp = 30.0f;
    //接触検知用変数
    private bool Contact = false;
    //接触回数計算用変数
    private int Counter = 0;
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
        if (this.transform.position.y > 3.5)
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
        }
        else if (this.transform.position.y < -4)
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
        }

        //時間計算
        delta += Time.deltaTime;

        //スパイクボール生成
        if (this.transform.position.y > 0 && delta >= 2)
        {
            GameObject Spikeball_big = Instantiate(spikeball_bigPrefab);
            Spikeball_big.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
            delta = 0;
        }
        else if (this.transform.position.y < 0 && delta >= 2)
        {
            GameObject Spikeball_small = Instantiate(spikeball_smallPrefab);
            Spikeball_small.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
            delta = 0;
        }

        // Playerアニメーションの状態取得
        Slide = PlayerAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Slide"));
        SlideStart = PlayerAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Slide-Start"));
        //耐久値が0になったら破壊
        if (Contact == true && Counter ==0)
        {
            delta = 0f;
            Counter++;
            this.hp -= 10f;
            if (hp <= 0)
            {
                //ポイントの加算(score_textの呼び出し)
                ScoreText.GetComponent<score_text_Controller>().Enemy2Score();
                GameOverText.GetComponent<GameOver_Text_Controller>().Enemy2Score();
                Destroy(this.gameObject);
            }
        }
        if (Counter >= 1)
        {
            //ダメージが入ると点滅(赤)
            GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 150);
            delta += Time.deltaTime;
            if (delta >= 0.3f)
            {
                GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 255);
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
}
