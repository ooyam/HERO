using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver_Text_Controller : MonoBehaviour
{
    //GameOverの判断
    private bool GameOver;
    //GameOverの判断
    private bool ShipDeth;
    //text表示中の確認
    private bool _Text;
    //Componentの取得
    private Component myComponent;
    //Humanを救出した総数
    private int HumanTotal;
    //Enemy1を救出した総数
    private int Enemy1Total;
    //Enemy2を救出した総数
    private int Enemy2Total;
    //Enemy3を救出した総数
    private int Enemy3Total;
    //タッチカウント用変数
    private int Touch = 0;
    //最終score
    private int RustScore;
    //時間計算用変数
    private float delta;

    //敵キャラを入れる
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    //Humanを入れる
    public GameObject Human;
    //BG_0を入れる
    public GameObject BG_0;

    //Butoonを入れる
    private GameObject AButton;
    private GameObject CButton;
    private GameObject RButton;
    //Componentをキャッシュする
    private Transform AButtonTra;
    private Transform CButtonTra;
    private Transform RButtonTra;

    // Start is called before the first frame update
    void Start()
    {
        //Butoonの取得
        AButton = GameObject.Find("AttackButton");
        CButton = GameObject.Find("CatchButton");
        RButton = GameObject.Find("ReleaseButton");
        //Componentの取得
        AButtonTra = AButton.GetComponent<Transform>();
        CButtonTra = CButton.GetComponent<Transform>();
        RButtonTra = RButton.GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameOver == true)
        {
            //シーンをロードする
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))&& Touch >= 1)
            {
                //時間を戻す
                Time.timeScale = 1;
                //SampleSceneを読み込む
                SceneManager.LoadScene("SampleScene");
            }
            //Playerが倒されたor救助船が破壊された際は､1秒後に時間を止める
            if (ShipDeth == true && _Text == false)
            {
                delta += Time.deltaTime;
                if (delta >= 1.0f)
                {
                    Time.timeScale = 0;
                    //GameOverの表示
                    GetComponent<Text>().text = "Game Over\n\n\n";
                    _Text = true;
                }
                //Buttonを見えなくする
                Buttan();
            }
            else
            {
                //GameOverの表示
                GetComponent<Text>().text = "Game Over\n\n\n";
                _Text = true;
                //Buttonを見えなくする
                Buttan();
            }
            //score用敵･人オブジェクトの表示
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && Touch == 0 && _Text == true)
            {
                //敵1オブジェクトの出力
                GameObject Enemy1obj = Instantiate(enemy1);
                Enemy1obj.transform.position = new Vector3(-2.2f, 2.3f, -7.0f);
                Enemy1obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                //敵2オブジェクトの出力
                GameObject Enemy2obj = Instantiate(enemy2);
                Enemy2obj.transform.position = new Vector3(-2.2f, 0.7f, -7.8f);
                Enemy2obj.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                //敵3オブジェクトの出力
                GameObject Enemy3obj = Instantiate(enemy3);
                Enemy3obj.transform.position = new Vector3(-2.2f, 1.55f, -7f);
                Enemy3obj.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
                //Humanオブジェクトの出力
                GameObject HumanObj = Instantiate(Human);
                HumanObj.transform.position = new Vector3(-2.1f, -2.5f, -7f);
                HumanObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                //BG_0オブジェクトの出力
                GameObject BGObj = Instantiate(BG_0);
                BGObj.transform.position = new Vector3(0, 0, -6f);

                Touch++;
            }
            //scoreの表示
            if (Touch == 1)
            {
                //Text表示
                GetComponent<Text>().fontSize = 35;
                GetComponent<Text>().color = new Color32(255, 255, 0, 255);
                GetComponent<Text>().text = "倒した敵の数\n\n      10p  ×  " + Enemy1Total + "体\n      10p  ×  " + Enemy3Total + "体\n      20p  ×  "
                                             + Enemy2Total + "体\n\n救出した人の数\n\n      30p  " + HumanTotal + "人\n------------------------------------\nScore : " + RustScore;
            }
        }
    }
    //Buttanを見えなくする
    void Buttan()
    {
        AButtonTra.position = new Vector3(100, -100, 0);
        CButtonTra.position = new Vector3(100, -100, 0);
        RButtonTra.position = new Vector3(100, -100, 0);
    }
    //GameOver判断
    public void GameOverJudge()
    {
        GameOver = true;
    }
    //救助船破壊によるGameOver判断
    public void ShipDethGameOver()
    {
        GameOver = true;
        ShipDeth = true;
    }
    //Humanスクリプトから呼ばれた際にスコア加算
    public void HumanScore()
    {
        HumanTotal ++;
        RustScore += 30;
    }
    //Enemy1スクリプトから呼ばれた際にスコア加算
    public void Enemy1Score()
    {
        Enemy1Total ++;
        RustScore += 10;
    }
    //Enemy2スクリプトから呼ばれた際にスコア加算
    public void Enemy2Score()
    {
        Enemy2Total ++;
        RustScore += 20;
    }
    //Enemy3スクリプトから呼ばれた際にスコア加算
    public void Enemy3Score()
    {
        Enemy3Total ++;
        RustScore += 10;
    }
}
