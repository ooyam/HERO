using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human_controller : MonoBehaviour
{
    //落下速度
    private float speed = -2;
    //時間停止用変数
    private int stop = 0;
    //Playerのゲームオブジェクトを取得
    private GameObject Player;
    //Playerのアニメーションコンポーネントを入れる
    private Animator PlayerAnimator;
    //score_textのゲームオブジェクトを入れる
    private GameObject ScoreText;
    //GameOver_Textのゲームオブジェクトを入れる
    private GameObject GameOverText;
    //Playerアニメーション状態取得用変数
    private bool Catch;
    private bool CatchRun;

    // Start is called before the first frame update
    void Start()
    {
        //Playerゲームオブジェクトの取得
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
        // Playerアニメーションの状態取得
        Catch = PlayerAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Catch"));
        CatchRun = PlayerAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Catch-Run"));

        if (CatchRun == false)
        {
            //落下
            if (this.transform.position.y > -6)
            {
                transform.Translate(0, speed * Time.deltaTime, 0, Space.World);
            }
            //ゲームオーバー
            else
            {
                Destroy(this.gameObject);
                //時間停止
                Time.timeScale = 0;
                //ゲームオーバー画面の呼び出し
                GameOverText.GetComponent<GameOver_Text_Controller>().GameOverJudge();
                //score表示を消す
                ScoreText.GetComponent<score_text_Controller>().GameOverJudge();
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
        if(CatchRun == true)
        {
            this.transform.position = new Vector3(this.Player.transform.position.x - 0.1f, this.Player.transform.position.y + 0.1f, 0);
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }
    IEnumerator WaitTimeCoroutine()
    {
        for (int i = 1; i <= 3; i++)
        {
            GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 150);
            yield return new WaitForSecondsRealtime(0.2f);
            GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 50);
            yield return new WaitForSecondsRealtime(0.3f);
        }
        Time.timeScale = 1;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Rescue ship" && CatchRun == false)
        {
            //ポイントの加算(score_textの呼び出し)
            ScoreText.GetComponent<score_text_Controller>().HumanScore();
            GameOverText.GetComponent<GameOver_Text_Controller>().HumanScore();
            Destroy(this.gameObject);
        }
    }
}
