using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rescueship_controller : MonoBehaviour
{
    //耐久値
    private float HP = 101.0f;
    //接触検知用変数
    private bool Contact;
    //時間計算用変数
    private float delta;
    //コルーチン重複防止用
    private int stop = 0;

    //GameOver_Textのゲームオブジェクトを入れる
    private GameObject GameOverText;

    // Start is called before the first frame update
    void Start()
    {
        //GameOver_Textゲームオブジェクトの取得
        GameOverText = GameObject.Find("GameOver_Text");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.Contact == true && this.HP >= 30)
        {
            this.delta += Time.deltaTime;
            if (delta >= 0.3f)
            {
                GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 255);
                this.delta = 0;
                Contact = false;
            }
        }
        if (this.HP <= 30 && stop == 0)
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
                    delta = 0;
                }
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
        Destroy(this.gameObject);
        yield return new WaitForSecondsRealtime(0.2f);
        //ゲームオーバー画面の呼び出し
        GameOverText.GetComponent<GameOver_Text_Controller>().GameOverJudge();
    }
}
