using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score_text_Controller : MonoBehaviour
{
    //スコア加算用変数
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //scoreの表示
        GetComponent<Text>().text = "Score:" + score.ToString();
    }
    //GameOver判断
    public void GameOverJudge()
    {
        this.transform.position += new Vector3(0, 100,0);
    }
    //Humanスクリプトから呼ばれた際にスコア加算
    public void HumanScore()
    {
        score += 30;
    }
    //Enemy2スクリプトから呼ばれた際にスコア加算
    public void Enemy2Score()
    {
        score += 20;
    }
    //Enemy1･3スクリプトから呼ばれた際にスコア加算
    public void EnemyScore()
    {
        score += 10;
    }
}
