using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score_text_Controller : MonoBehaviour
{
    //スコア加算用変数
    public int score = 0;
    //Level取得用変数
    public float Level = 1f;
    //Transformのキャッシュ
    private RectTransform _transform;
    //Textのキャッシュ
    private Text _text;
    //Audioをキャッシュ
    private AudioSource Audio;
    //GameOver_Textのスクリプトを入れる
    private GameOver_Text_Controller GameOverScr;

    // Start is called before the first frame update
    void Start()
    {
        //Transformのキャッシュ
        _transform = GetComponent<RectTransform>();
        //Textのキャッシュ
        _text = GetComponent<Text>();
        //Audioのキャッシュ
        Audio = GetComponent<AudioSource>();
        //GameOver_Textのスクリプトを入れる
        GameOverScr = GameObject.Find("GameOver_Text").GetComponent<GameOver_Text_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        //scoreの表示
        _text.text = "Score:" + score.ToString();
    }
    //GameStart判断
    public void GameStart()
    {
        this._transform.anchoredPosition = new Vector3(85, -15, 0);
    }
    //GameOver判断
    public void GameOverJudge()
    {
        this._transform.anchoredPosition = new Vector3(85, 100,0);
    }
    //Humanスクリプトから呼ばれた際にスコア加算
    public void HumanScore()
    {
        score += 30;
        Audio.Play();
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
    //LevelUp
    public void LevelUp()
    {
        this.Level++;
        GameOverScr.LevelUp();
    }
}
