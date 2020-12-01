using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    //敵キャラを入れる
    public GameObject enemy1Prefab;
    public GameObject enemy2Prefab;
    public GameObject enemy3Prefab;

    // 時間計測用の変数
    private float[] delta = {0,0,0};

    // Start is called before the first frame update
    void Start()
    {
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

        //時間計算
        this.delta[0] += Time.deltaTime;
        this.delta[1] += Time.deltaTime;
        this.delta[2] += Time.deltaTime;

        //画面内に敵1が7体以下の状態で､3秒以上経過したとき
        if (Enemy1counts <= 7 && this.delta[0] > 3f)
        {
            this.delta[0] = 0;

            //敵1のランダム生成
            int Enemy1 = Random.Range(1, 11);
            if (Enemy1 <= 2)
            {
                //敵1を3体生成
                for (int i = -1; i <= 1; i++)
                {
                    //生成場所のランダム指定
                    float Enemy1pos = Random.Range(-2.14f, 2.14f);
                    GameObject Enemy1obj = Instantiate(enemy1Prefab);
                    Enemy1obj.transform.position = new Vector2(10, Enemy1pos);
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

        //画面内に敵2が1体以下の状態で､8秒以上経過したとき
        if (Enemy2counts <= 1 && this.delta[1] > 8f)
        {
            this.delta[1] = 0;

            //敵2を50%の確率で生成
            int Enemy2 = Random.Range(1, 3);
            if (Enemy2 == 1)
            {
                //敵2の存在の有無
                bool Enemy2search = GameObject.Find("Enemy2(Clone)");
                //生成場所のランダム指定
                int Enemy2pos = Random.Range(1, 3);

                //敵2未生成の場合
                if (Enemy2search == false)
                {
                    if (Enemy2pos == 1)
                    {
                        GameObject Enemy2obj = Instantiate(enemy2Prefab);
                        Enemy2obj.transform.position = new Vector2(7.2f, 5.8f);
                    }
                    else
                    {
                        GameObject Enemy2obj = Instantiate(enemy2Prefab);
                        Enemy2obj.transform.position = new Vector2(7.2f, 5.8f);
                    }
                }
                else
                {
                    //敵2の生成Y座標の取得
                    float Enemy2posY = GameObject.Find("Enemy2(Clone)").transform.position.y;

                    //敵2が画面右上に生成中の場合
                    if (Enemy2posY >= 0)
                    {
                        GameObject Enemy2obj = Instantiate(enemy2Prefab);
                        Enemy2obj.transform.position = new Vector2(1.5f, -6.5f);
                    }
                    //画面下中央に生成中の場合
                    else
                    {
                        GameObject Enemy2obj = Instantiate(enemy2Prefab);
                        Enemy2obj.transform.position = new Vector2(7.2f, 5.8f);
                    }
                }
                
                
            }
        }
        //画面内に敵3が1体以下の状態で､5秒以上経過したとき
        if(Enemy3counts <= 1 && this.delta[2] > 5f)
        {
            this.delta[2] = 0;

            //25%の確率で生成
            int Enemy3 = Random.Range(1, 5);
            if(Enemy3 == 1)
            {
                //生成場所のランダム指定
                float Enemy3pos = Random.Range(-2.14f, 1.25f);
                GameObject Enemy3obj = Instantiate(enemy3Prefab);
                Enemy3obj.transform.position = new Vector2(11, Enemy3pos);
            }
        }
    }
}
