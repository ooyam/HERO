using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen_2_Controller : MonoBehaviour
{
    //遅延用変数
    private bool Delay = false;
    //BGMControllerのゲームオブジェクト/スクリプトを入れる
    private GameObject BGMCon;
    private BGM_Controller BGMConScr;
    //score_textのゲームオブジェクト/スクリプトを入れる
    private GameObject Score;
    private score_text_Controller ScoreScr;
    //Buttonのゲームオブジェクト/Transformを入れる
    private RectTransform AButtonTra;
    private RectTransform CButtonTra;
    private RectTransform RButtonTra;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("WaitCoroutine");
        //BGMControllerのゲームオブジェクト/スクリプト取得
        BGMCon = GameObject.Find("BGMController");
        BGMConScr = BGMCon.GetComponent<BGM_Controller>();
        //score_textのゲームオブジェクト/スクリプト取得
        Score = GameObject.Find("score_text");
        ScoreScr = Score.GetComponent<score_text_Controller>();
        //Buttonのゲームオブジェクト/Transformを入れる
        AButtonTra = GameObject.Find("AttackButton").GetComponent<RectTransform>();
        CButtonTra = GameObject.Find("CatchButton").GetComponent<RectTransform>();
        RButtonTra = GameObject.Find("ReleaseButton").GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Delay == true)
        {
            //自身を破壊
            Destroy(this.gameObject);
            //BGM開始
            BGMConScr.BGMPlay();
            //score表示
            ScoreScr.GameStart();
            //Button表示
            AButtonTra.anchoredPosition = new Vector3(-80,100,0);
            CButtonTra.anchoredPosition = new Vector3(-230,40,0);
            RButtonTra.anchoredPosition = new Vector3(-80,40,0);
            //ゲームスタート
            Time.timeScale = 1;
        }
    }
    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        Delay = true;
    }
}
