using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human_controller : MonoBehaviour
{
    //落下速度
    private float speed = -1.0f;
    //時間停止用変数
    private int stop = 0;
    //Transformのキャッシュ
    private Transform _transform;
    //Rendererのキャッシュ
    private Renderer _renderer;
    //ParticleSystemのキャッシュ
    private ParticleSystem Particle;
    //Playerのゲームオブジェクト/アニメーションコンポーネント/Transformを入れる
    private GameObject Player;
    private Animator PlayerAnimator;
    private Transform PlayerTra;
    //score_textのゲームオブジェクト/スクリプトを入れる
    private GameObject ScoreText;
    private score_text_Controller ScoreTextScr;
    //GameOver_Textのゲームオブジェクト/スクリプトを入れる
    private GameObject GameOverText;
    private GameOver_Text_Controller GameOverTextScr;
    //Rescue shipのゲームオブジェクト/Transformを入れる
    private GameObject RescueShip;
    private Vector3 ShipTra;
    //Wave接触判定用変数
    private bool Contact;
    //Playerアニメーション状態取得用変数
    private bool Catch;
    private bool CatchRun;
    //AudioSourceのキャッシュ
    private AudioSource Audio;
    public AudioClip LightSE;

    // Start is called before the first frame update
    void Start()
    {
        //Transformのキャッシュ
        _transform = GetComponent<Transform>();
        //Rendererのキャッシュ
        _renderer = GetComponent<Renderer>();
        //ParticleSystemのキャッシュ
        Particle = GetComponent<ParticleSystem>();
        //Playerゲームオブジェクトの取得
        Player = GameObject.Find("Player");
        PlayerAnimator = Player.GetComponent<Animator>();
        PlayerTra = Player.GetComponent<Transform>();
        //score_textゲームオブジェクト/スクリプトの取得
        ScoreText = GameObject.Find("score_text");
        ScoreTextScr = ScoreText.GetComponent<score_text_Controller>();
        //GameOver_Textゲームオブジェクト/スクリプトの取得
        GameOverText = GameObject.Find("GameOver_Text");
        GameOverTextScr = GameOverText.GetComponent<GameOver_Text_Controller>();
        //Rescue shipゲームオブジェクト/Transformの取得
        RescueShip = GameObject.Find("Rescue ship");
        ShipTra = new Vector3 (-7.8f,3.1f,0);
        //SEを取得
        Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Playerアニメーションの状態取得
        Catch = PlayerAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Catch"));
        CatchRun = PlayerAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Catch-Run"));

        if (CatchRun == false && Contact == false)
        {
            //落下
            if (this._transform.position.y > -6)
            {
                _transform.Translate(0, speed * Time.deltaTime, 0, Space.World);
            }
            //ゲームオーバー
            else
            {
                Destroy(this.gameObject);
                //時間停止
                Time.timeScale = 0;
                //ゲームオーバー画面の呼び出し
                GameOverTextScr.GameOverJudge("Human");
                //score表示を消す
                ScoreTextScr.GameOverJudge();
            }

            //画面下端に来るとオブジェクト点滅
            if (this.transform.position.y <= -5 && stop == 0)
            {
                //時間停止
                Time.timeScale = 0;
                //DestroyCoroutineを実行
                StartCoroutine(WaitTimeCoroutine());
                stop = 1;
            }
        }

        //CatchRun状態
        if(CatchRun == true && Contact == false)
        {
            this._transform.position = new Vector3(this.PlayerTra.position.x - 0.1f, this.PlayerTra.position.y + 0.1f, 0);
            this._transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            this._transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        //Wave接触によりShipへ直行
        if(Contact == true)
        {
            this._transform.position = Vector3.Lerp(this._transform.position, ShipTra, 0.5f * Time.deltaTime);
        }
    }
    IEnumerator WaitTimeCoroutine()
    {
        for (int i = 1; i <= 3; i++)
        {
            _renderer.material.color = new Color32(255, 0, 0, 150);
            //SEを出す
            Audio.PlayOneShot(LightSE);
            yield return new WaitForSecondsRealtime(0.2f);
            _renderer.material.color = new Color32(255, 255, 255, 50);
            yield return new WaitForSecondsRealtime(0.3f);
        }
        Time.timeScale = 1;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Rescue ship" && CatchRun == false)
        {
            //ポイントの加算(score_textの呼び出し)
            ScoreTextScr.HumanScore();
            GameOverTextScr.HumanScore();
            Destroy(this.gameObject);
        }
    }
    //パーティクル当たり判定
    void OnParticleCollision(GameObject obj)
    {
        //Waveに接触した際はRescue shipに直行
        if (obj.gameObject.tag == "Wave")
        {
            Contact = true;
            //色を変え､パーティクルを呼び出す
            _renderer.material.color = new Color32(255, 255, 0, 100);
            Particle.Play();
        }
    }
}
