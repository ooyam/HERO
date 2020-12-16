using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver_Text_Controller : MonoBehaviour
{
    //Textをキャッシュする
    private Text _text;
    //GameOverの判断
    public bool GameOver;
    //GameOver要因の判断
    private bool PlayerDeth;
    private bool ShipDeth;
    private bool HumanDeth;
    private bool Enemy3Escape;
    //text表示中の確認
    private bool _Text;
    public bool _Text1;
    //Componentの取得
    private Component myComponent;
    //Human/Enemy1/Enemy2/Enemy3を救出/破壊した総数
    public int HumanTotal = 0;
    public int Enemy1Total = 0;
    public int Enemy2Total = 0;
    public int Enemy3Total = 0;
    //最終score
    public int RustScore = 0;
    //タッチカウント用変数
    private int Touch = 0;
    //Level取得用変数
    public float Level = 1f;
    //時間計算用変数
    private float delta;

    //敵キャラを入れる
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    //Humanを入れる
    public GameObject Human;
    //BGのTransformを入れる
    private Transform BG_1;
    private Transform BG_2;
    private Transform BG_3;
    private Transform BG_4;
    //RescueShipを入れる
    public GameObject Ship;
    private Transform ShipHP;
    //Plyaerを入れる
    public GameObject Player;
    //GameOver要因のオブジェクト
    private GameObject FactorObj;
    //ScoreDisplayのオブジェクト/スクリプトを入れる
    public GameObject ScoreDisplay;
    private ScoreDisplay_Controller ScoreDisplayScr;

    //Butoonを入れる
    private GameObject AButton;
    private GameObject CButton;
    private GameObject RButton;
    private GameObject RecoveryButton;
    private GameObject RepairButton;
    private GameObject WaveButton;
    //Componentをキャッシュする
    private RectTransform AButtonTra;
    private RectTransform CButtonTra;
    private RectTransform RButtonTra;
    private RectTransform RecoveryButtonTra;
    private RectTransform RepairButtonTra;
    private RectTransform WaveButtonTra;
    //score_textのスクリプトを入れる
    private score_text_Controller ScoreScr;

    //AudioSourceを入れる
    private AudioSource Audio;
    //SEを入れる
    public AudioClip GameOverSE;
    public AudioClip TupSE;
    public AudioClip LevelSE;

    // Start is called before the first frame update
    void Start()
    {
        //Textのキャッシュ
        _text = GetComponent<Text>();
        //Butoonの取得
        AButton = GameObject.Find("AttackButton");
        CButton = GameObject.Find("CatchButton");
        RButton = GameObject.Find("ReleaseButton");
        RecoveryButton = GameObject.Find("RecoveryButton");
        RepairButton = GameObject.Find("RepairButton");
        WaveButton = GameObject.Find("WaveButton");
        //Componentの取得
        AButtonTra = AButton.GetComponent<RectTransform>();
        CButtonTra = CButton.GetComponent<RectTransform>();
        RButtonTra = RButton.GetComponent<RectTransform>();
        RecoveryButtonTra = RecoveryButton.GetComponent<RectTransform>();
        RepairButtonTra = RepairButton.GetComponent<RectTransform>();
        WaveButtonTra = WaveButton.GetComponent<RectTransform>();
        //score_textのスクリプトを取得
        ScoreScr = GameObject.Find("score_text").GetComponent<score_text_Controller>();
        //救助船/PlayerのTransformを取得
        ShipHP = GameObject.Find("Rescue ship MaxHP").GetComponent<Transform>();
        //ScoreDisplayのオブジェクト/スクリプトを入れる
        ScoreDisplayScr = ScoreDisplay.GetComponent<ScoreDisplay_Controller>();
        //BGのTransformを取得
        BG_1 = GameObject.Find("BG_1").GetComponent<Transform>();
        BG_2 = GameObject.Find("BG_2").GetComponent<Transform>();
        BG_3 = GameObject.Find("BG_3").GetComponent<Transform>();
        BG_4 = GameObject.Find("BG_4").GetComponent<Transform>();

        //AudioSourceを取得
        Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOver == true)
        {
            //Playerが倒されたor救助船が破壊された際は､1秒後に時間を止める
            if ((ShipDeth == true || PlayerDeth == true) && _Text == false)
            {
                Button();
                delta += Time.deltaTime;
                if (delta >= 1.0f)
                {
                    //SEを出す
                    Audio.PlayOneShot(GameOverSE);
                    //GameOverの表示
                    _text.color = new Color32(255, 0, 0, 230);
                    _text.text = "Game Over\n\n";
                    //GameOverを最低1.5秒表示する
                    StartCoroutine("GameOverCoroutine");
                    Time.timeScale = 0;
                }
            }
            else if((Enemy3Escape == true || HumanDeth == true) && _Text == false)
            {
                Button();
                //SEを出す
                Audio.PlayOneShot(GameOverSE);
                //GameOverの表示
                _text.color = new Color32(255, 0, 0, 230);
                _text.text = "Game Over\n\n";
                //GameOverを最低1.5秒表示する
                StartCoroutine("GameOverCoroutine");
            }
            //シーンをロードする
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && Touch >= 2)
            {
                //SEを出す
                Audio.PlayOneShot(TupSE);
                //SampleSceneを読み込む
                SceneManager.LoadScene("SampleScene");
            }
            //score用敵･人オブジェクトの表示
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && Touch == 1 && _Text1 == true)
            {
                _text.text = "";
                //生成したオブジェクトを破壊､ShipHPを元の場所に戻す
                Destroy(this.FactorObj);
                ShipHP.position = new Vector3(-7f, 4.2f, 0f);
                //scoreの取得
                this.RustScore = ScoreScr.score;
                //ScoreDisplayを生成しscoreを渡す
                GameObject scoreDesplay = Instantiate(ScoreDisplay);
                scoreDesplay.transform.position = new Vector3(0, 0, -6f);
                //SEを出す
                Audio.PlayOneShot(TupSE);
                StartCoroutine("GameOverCoroutine");

                Touch++;
            }
            //GameOver要因の表示
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && Touch == 0 && _Text1 == true)
            {
                //BG_1オブジェクトを前に出す
                BG_1.position = new Vector3(0, 0, -6f);
                BG_2.position = new Vector3(0, 0, -6f);
                BG_3.position = new Vector3(0, 0, -6f);
                BG_4.position = new Vector3(0, 0, -6f);
                //フォントサイズ変更
                _text.fontSize = 50;
                if (ShipDeth == true)
                {
                    _text.text = "救助船の耐久値が0になった\n\n\n";
                    FactorObj = Instantiate(Ship);
                    FactorObj.transform.position = new Vector3(0f, 0f, -7.0f);
                    ShipHP.position = new Vector3(0f, 1.2f, -7.0f);
                }
                else if (PlayerDeth == true)
                {
                    _text.text = "HEROが力尽きた\n\n\n";
                    FactorObj = Instantiate(Player);
                    FactorObj.transform.position = new Vector3(0f, 0f, -7.0f);
                }
                else if (Enemy3Escape == true)
                {
                    _text.text = "人が連れ去られた\n\n\n";
                    FactorObj = Instantiate(enemy3);
                    FactorObj.transform.position = new Vector3(0f, 0f, -7.0f);
                }
                else if (HumanDeth == true)
                {
                    _text.text = "救出失敗\n\n\n";
                    FactorObj = Instantiate(Human);
                    FactorObj.transform.position = new Vector3(0f, 0f, -7.0f);
                }
                StartCoroutine("GameOverCoroutine");
                Touch++;
            }
        }
    }
    IEnumerator GameOverCoroutine()
    {
        _Text = true;
        for (int i = 0; i <= 1; i++)
        {
            yield return new WaitForSecondsRealtime(1.5f);
            _Text1 = true;
        }
    }
    //ボタンを消す
    void Button()
    {
        AButtonTra.anchoredPosition = new Vector3(100, -100, 0);
        CButtonTra.anchoredPosition = new Vector3(100, -100, 0);
        RButtonTra.anchoredPosition = new Vector3(100, -100, 0);
        RecoveryButtonTra.anchoredPosition = new Vector3(0,100,0);
        RepairButtonTra.anchoredPosition = new Vector3(0, 100, 0);
        WaveButtonTra.anchoredPosition = new Vector3(0, 100, 0);
    }
    //GameOver判断
    public void GameOverJudge(string Obj)
    {
        GameOver = true;
        //GameOver要因を取得
        if(Obj == "Enemy3")
        {
            Enemy3Escape = true;
        }
        else if(Obj == "Human")
        {
            HumanDeth = true;
        }
        else if (Obj == "Ship")
        {
            ShipDeth = true;
        }
        else if (Obj == "Player")
        {
            PlayerDeth = true;
        }
    }
    //Humanスクリプトから呼ばれた際にスコア加算
    public void HumanScore()
    {
        HumanTotal ++;
    }
    //Enemy1スクリプトから呼ばれた際にスコア加算
    public void Enemy1Score()
    {
        Enemy1Total ++;
    }
    //Enemy2スクリプトから呼ばれた際にスコア加算
    public void Enemy2Score()
    {
        Enemy2Total ++;
    }
    //Enemy3スクリプトから呼ばれた際にスコア加算
    public void Enemy3Score()
    {
        Enemy3Total ++;
    }
    //LevelUp
    public void LevelUp()
    {
        this.Level = ScoreScr.Level;
        //TextColorCoroutineを実行
        StartCoroutine(TextColorCoroutine());
    }
    IEnumerator TextColorCoroutine()
    {
        Audio.PlayOneShot(LevelSE);
        for (float i = 1f; i >= 0; i -= 0.1f)
        {
            Color StartColor = new Color32(255, 255, 0, 230);
            Color EbdColor = new Color32(255, 255, 0, 0);
            //GameOverの表示
            _text.color = Color.Lerp(EbdColor, StartColor, i) ;
            _text.text = "Level "+ this.Level + "\n\n";
            yield return new WaitForSecondsRealtime(0.1f);
        }
        _text.color = new Color32(255, 255, 0, 0);
    }

}
