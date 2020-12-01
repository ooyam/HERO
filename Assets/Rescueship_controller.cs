using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rescueship_controller : MonoBehaviour
{
    //耐久値
    private float HP = 151.0f;
    //接触検知用変数
    private bool Contact;
    //時間計算用変数
    private float delta;
    //コルーチン重複防止用
    private int stop = 0;
    //エフェクト重複防止用
    private bool eff = false;

    //エフェクトのゲームオブジェクトを入れる
    public GameObject Effect;
    //Dethエフェクトのゲームオブジェクトを入れる
    public GameObject DethEffect;
    //GameOver_Textのゲームオブジェクトを入れる
    private GameObject GameOverText;
    //score_textのゲームオブジェクトを入れる
    private GameObject ScoreText;

    // Start is called before the first frame update
    void Start()
    {
        //GameOver_Textゲームオブジェクトの取得
        GameOverText = GameObject.Find("GameOver_Text");
        //score_textゲームオブジェクトの取得
        ScoreText = GameObject.Find("score_text");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.Contact == true && this.HP >= 50)
        {
            this.delta += Time.deltaTime;
            if (delta >= 0.3f)
            {
                GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 255);
                this.delta = 0;
                Contact = false;
            }
        }
        if (this.HP <= 50 && stop == 0)
        {
            this.delta += Time.deltaTime;
            if (this.delta <= 0.5f)
            {
                GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 255);
            }
            else
            {
                GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 255);
                if (this.delta >= 1f)
                {
                    this.delta = 0;
                }
            }
        }
        if (this.HP <= 30 && eff == false)
        {
            //エフェクトを呼び出す
            GameObject effect = Instantiate(Effect);
            effect.transform.position = new Vector3(this.transform.position.x + 1.5f, this.transform.position.y -0.5f,3f);
            eff = true;
            if(this.HP > 30)
            {
                eff = false;
            }
        }
        //ゲームオーバー
        if (this.HP <= 0 && stop == 0)
        {
            //時間停止
            Time.timeScale = 0;
            //DestroyCoroutineを実行
            StartCoroutine(WaitTimeCoroutine());
            stop = 1;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Red ball")
        {
            Contact = true;
            this.HP -= 10f;
            GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 255);
        }
    }
    IEnumerator WaitTimeCoroutine()
    {
        for (int i = 1; i <= 5; i++)
        {
            GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 255);
            yield return new WaitForSecondsRealtime(0.2f);
            GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 255);
            yield return new WaitForSecondsRealtime(0.2f);
        }
        
        for (int num = 0; num <= 1; num++)
        {
            //Dethエフェクトを呼び出す
            GameObject Detheffect = Instantiate(DethEffect);
            Detheffect.transform.position = new Vector3(this.transform.position.x + 1.5f, this.transform.position.y, 0);
            //破壊
            Destroy(this.gameObject);
            Time.timeScale = 1;
            //ゲームオーバー画面の呼び出し
            GameOverText.GetComponent<GameOver_Text_Controller>().ShipDethGameOver();
            //score表示を消す
            ScoreText.GetComponent<score_text_Controller>().GameOverJudge();
        }
    }
}
