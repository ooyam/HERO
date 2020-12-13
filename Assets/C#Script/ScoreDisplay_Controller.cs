using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay_Controller : MonoBehaviour
{
    //Point/LevelBonusのtextを入れる
    private Text PointText;
    private Text BonusText;
    //GameOver_Textのスクリプトを入手
    private GameOver_Text_Controller GameOverScr;

    //Human/Enemy1/Enemy2/Enemy3を救出/破壊した総数
    private int HumanTotal;
    private int Enemy1Total;
    private int Enemy2Total;
    private int Enemy3Total;
    //最終レベル/スコア
    private float FinalLevel;
    private int FinalScore;

    // Start is called before the first frame update
    void Start()
    {
        //Point/LevelBonusのtextを取得
        PointText = GameObject.Find("Point").GetComponent<Text>();
        BonusText = GameObject.Find("LevelBonus").GetComponent<Text>();
        //GameOver_Textのスクリプトを入手
        GameOverScr = GameObject.Find("GameOver_Text").GetComponent<GameOver_Text_Controller>();
        //最終スコアを集計
        this.HumanTotal = GameOverScr.HumanTotal;
        this.Enemy1Total = GameOverScr.Enemy1Total;
        this.Enemy2Total = GameOverScr.Enemy2Total;
        this.Enemy3Total = GameOverScr.Enemy3Total;
        this.FinalLevel = GameOverScr.Level;
        this.FinalScore = GameOverScr.RustScore;
        //text内容の更新
        PointText.text = "10p  ×  " + this.Enemy1Total + "体\n10p  ×  " + this.Enemy2Total + "体\n20p ×  " + this.Enemy3Total + "体\n\n30p ×  " + this.HumanTotal + "人";
        BonusText.text = "Bonus : Level " + this.FinalLevel + "  ×  500p\n\nScore : " + (this.FinalScore + (this.FinalLevel * 500));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Aggregate()
    {
    }
}
