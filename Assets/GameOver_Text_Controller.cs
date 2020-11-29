using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver_Text_Controller : MonoBehaviour
{
    //GameOverの判断
    private bool GameOver;
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
    //score表示画面
    private bool Score;
    //最終score
    private int RustScore;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOver == true)
        {
            //GameOverの表示
            GetComponent<Text>().text = "Game Over\n\n\n";
            //シーンをロードする
            if (Input.GetKeyDown(KeyCode.Space) && Score == true)
            {
                //時間を戻す
                Time.timeScale = 1;
                //SampleSceneを読み込む
                SceneManager.LoadScene("SampleScene");
            }
            if (Input.GetKeyDown(KeyCode.Space) || Score == true)
            {
                GetComponent<Text>().fontSize = 35;
                GetComponent<Text>().color = new Color32(255, 255, 0, 255);
                GetComponent<Text>().text = "倒した敵の数\n\n           ×     " + Enemy1Total + "体\n\n           ×     " + Enemy2Total + "体\n\n           ×     " + Enemy3Total + "体\n\n救出した人の数   " + HumanTotal + "人\n\nScore : "+ RustScore;
                Score = true;
            }
        }
    }
    //Humanスクリプトから呼ばれた際にスコア加算
    public void GameOverJudge()
    {
        GameOver = true;
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
