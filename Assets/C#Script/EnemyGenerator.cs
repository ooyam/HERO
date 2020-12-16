using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    //敵キャラを入れる
    public GameObject enemy1Prefab;
    public GameObject enemy1_2Prefab;
    public GameObject enemy2Prefab;
    public GameObject enemy3Prefab;
    //Enemy2位置把握用変数
    private bool Enemy2Up = false;
    private bool Enemy2Right = false;
    private bool Enemy2Left = false;
    //score_textのスクリプトを入れる
    private score_text_Controller ScoreScr;
    //出現率計算用変数
    private float Level = 1;
    //Enemy出現時間
    private float[] Seconds = { 3f, 5f ,5f};
    //Enemy出現確率
    private float[] Probability = { 10f, 2f, 3f };
    // 時間計測用の変数
    private float[] delta = { 0, 0, 0, 0 };
    //Enemy2ランダム用変数
    private int Enemy2Random = 2;

    // Start is called before the first frame update
    void Start()
    {
        //score_textのスクリプトを取得
        ScoreScr = GameObject.Find("score_text").GetComponent<score_text_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        //敵1の数の把握
        int Enemy1counts = GameObject.FindGameObjectsWithTag("Enemy1").Length;
        //敵2の数の把握
        int Enemy2counts = GameObject.FindGameObjectsWithTag("Enemy2").Length;
        //敵3の数の把握
        int Enemy3counts = GameObject.FindGameObjectsWithTag("Enemy3").Length;

        //時間計算(Enemy1,2,3,Level)
        this.delta[0] += Time.deltaTime;
        this.delta[1] += Time.deltaTime;
        this.delta[2] += Time.deltaTime;
        this.delta[3] += Time.deltaTime;

        //画面内に敵1が7体以下の状態で､3秒以上経過したとき(Level==1の場合)
        if (Enemy1counts <= 7 && this.delta[0] > Seconds[0])
        {
            this.delta[0] = 0;

            //敵1のランダム生成
            float Enemy1 = Random.Range(0f, Probability[0]);
            if (Enemy1 <= 2f)
            {
                //20%の確率で敵1を3体生成(Level==1の場合)
                for (int i = -1; i <= 1; i++)
                {
                    //生成場所のランダム指定
                    float Enemy1pos = Random.Range(-2f, 2f);
                    int random = Random.Range(-1, 3);
                    //レベル6より画面上下からも出現するようにする
                    if (this.Level >= 1.20 && (random == 1 || random == -1))
                    {
                        GameObject Enemy1obj = Instantiate(enemy1_2Prefab);
                        Enemy1obj.transform.position = new Vector2(Enemy1pos * 2, 6.5f * random);
                    }
                    else
                    {
                        GameObject Enemy1obj = Instantiate(enemy1Prefab);
                        Enemy1obj.transform.position = new Vector2(10, Enemy1pos);
                    }
                }
            }
            else
            {
                //生成場所のランダム指定
                float Enemy1pos = Random.Range(-2.14f, 2.14f);
                // 敵1を1体生成
                GameObject Enemy1obj = Instantiate(enemy1Prefab);
                Enemy1obj.transform.position = new Vector2(10, Enemy1pos);
            }
        }
        //画面内に敵2が1体以下の状態で､5秒以上経過したとき(Level==1の場合)
            if (Enemy2counts < Enemy2Random && this.delta[1] > Seconds[1])
        {
            this.delta[1] = 0;

            //敵2を50%の確率で生成(Level==1の場合)
            float Enemy2 = Random.Range(0f, Probability[1]);
            if (Enemy2 < 1)
            {
                //生成場所のランダム指定
                int Enemy2pos = Random.Range(0, Enemy2Random);
                //生成場所の指定
                if ((Enemy2Up == false && Enemy2pos == 0) || (Enemy2Up == false && Enemy2Right == true && Enemy2Left == true))
                {
                    //Enemy2生成
                    GameObject Enemy2obj = Instantiate(enemy2Prefab);
                    Enemy2obj.transform.position = new Vector2(7.2f, 5.8f);
                }
                else if ((Enemy2Right == false && Enemy2pos == 1) || (Enemy2Right == false && Enemy2Up == true && Enemy2Left == true))
                {
                    GameObject Enemy2obj = Instantiate(enemy2Prefab);
                    Enemy2obj.transform.position = new Vector2(1.0f, -6.5f);
                }
                //Level6から3体出現の可能性あり
                else if ((Enemy2Left == false && Enemy2pos == 2) || (Enemy2Left == false && Enemy2Up == true && Enemy2Right == true))
                {
                    GameObject Enemy2obj = Instantiate(enemy2Prefab);
                    Enemy2obj.transform.position = new Vector2(-3.5f, -6.5f);
                }
            }
        }
        //画面内に敵3が1体以下の状態で､5秒以上経過したとき(Level==1の場合)
        if (Enemy3counts <= 1 && this.delta[2] > Seconds[2])
        {
            this.delta[2] = 0;

            //33%の確率で生成(Level==1の場合)
            float Enemy3 = Random.Range(0f, Probability[2]);
            if(Enemy3 < 1)
            {
                //生成場所のランダム指定
                float Enemy3pos = Random.Range(-2.14f, 1.25f);
                GameObject Enemy3obj = Instantiate(enemy3Prefab);
                Enemy3obj.transform.position = new Vector3(11, Enemy3pos, -2);
            }
        }
        //30秒毎にレベルを上げる
        if(delta[3] >= 30.0f)
        {
            //出現秒数/出現率リセット
            this.Seconds[0] = 2f;
            this.Seconds[1] = 5f;
            this.Probability[0] /= 10f;
            //Level毎にEnemy1.2出現時間を短くする
            this.Level += 0.05f;
            this.Seconds[0] /= Level;
            this.Seconds[1] /= Level;
            //Level毎にEnemy1出現確率を高くする
            this.Probability[0] /= Level;
            if(this.Level >= 1.25 && Enemy2Random != 3)
            {
                Enemy2Random = 3;
            }
            //時間リセット
            delta[3] = 0f;
            //他スクリプトに共有
            ScoreScr.LevelUp();
        }
    }
    public void UpTrue()
    {
        Enemy2Up = true;
    }
    public void UpFalse()
    {
        Enemy2Up = false;
    }
    public void UnderRightTrue()
    {
        Enemy2Right = true;
    }
    public void UnderRightFalse()
    {
        Enemy2Right = false;
    }
    public void UnderLeftTrue()
    {
        Enemy2Left = true;
    }
    public void UnderLeftFalse()
    {
        Enemy2Left = false;
    }
}
