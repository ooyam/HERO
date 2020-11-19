using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour
{
    //x軸稼働限度
    private float XRange = 6;
    //x軸中央値
    private float XcenterRange = 2;
    //y軸稼働限度
    private float YRange = 2.14f;
    //x軸移動速度
    private float EnemySpeedX = 3;
    //y軸移動速度
    private float EnemySpeedY = 4;
    //初期位置ランダム用変数
    private int pos;
    //行動パターンランダム用変数
    private int Pattern;
    //直線運動用変数
    private float sin;
    //移動速度代入用変数
    private float speed;
    //初期移動停止用変数
    private int stop;
    //保持用変数
    private int keep;
    //初期位置記憶用変数
    private float defpos;

    //円運動の移動速度
    private float Rotspeed = 4f;
    //円を描く半径
    private float radius = 0.1f;
    //回転運動のx軸用変数
    float sinx;
    //回転運動のz軸用変数
    float cosy;

    //スパイクボールを入れる
    public GameObject spikeballPrefab;
    //時間計算用変数
    private float delta;

    // Start is called before the first frame update
    void Start()
    {
        //初期位置のランダム指定
        pos = Random.Range(-1, 5);
        //初期位置まで移動条件
        stop = 0;
        //行動パターンのランダム選出
        Pattern = Random.Range(1,5);

    }
    // Update is called once per frame
    void Update()
    {
        //初期位置まで移動
        if (this.transform.position.x > pos * 2 && stop == 0)
        {
            transform.Translate(-EnemySpeedY * Time.deltaTime, 0, 0);
        }

        if (this.transform.position.x <= pos * 2 || keep >= 1)
        {
            keep = 1;
            stop = 1;
            delta += Time.deltaTime;
            //パターン1
            if (Pattern == 1)
            {
                //スタートコルーチン
                StartCoroutine(LinearMotion(EnemySpeedY, pos * 2, sin * YRange));
            }
            //パターン2
            else if (Pattern == 2)
            {
                StartCoroutine(LinearMotion(EnemySpeedX, sin * XRange + XcenterRange, this.transform.position.y));
            }
            //パターン3
            else if (Pattern == 3)
            {
                //X軸の設定
                sinx = radius * Mathf.Sin(Time.time * Rotspeed);
                //y軸の設定
                cosy = radius * Mathf.Cos(Time.time * Rotspeed);
                //自分のいる位置から座標を動かす。
                this.transform.position = new Vector2(sinx + this.transform.position.x, cosy + this.transform.position.y);
            }
            //パターン4
            else if (Pattern == 4)
            {
                //X軸の設定
                sinx = radius * Mathf.Sin(Time.time * Rotspeed);
                //y軸の設定
                cosy = radius * Mathf.Cos(Time.time * Rotspeed);
                //自分のいる位置から座標を動かす。
                this.transform.position = new Vector2(this.transform.position.x + sinx, this.transform.position.y - cosy);
            }
            //1個/2秒スパイクボール生成
            if (delta >= 2)
            {
                GameObject Spikeball = Instantiate(spikeballPrefab);
                Spikeball.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
                delta = 0;
            }
        }
        
    }
    private IEnumerator LinearMotion(float speed, float MoveX, float MoveY)
    {
        //0.3秒待機
        yield return new WaitForSeconds(0.3f);
        //動作開始
        sin = Mathf.Sin(Time.time * speed );
        transform.position = new Vector2(MoveX, MoveY);
    }
}