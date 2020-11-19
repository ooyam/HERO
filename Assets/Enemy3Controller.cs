using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Controller : MonoBehaviour
{
    // 回転速度
    private float rotSpeed = -5f;

    // 移動速度
    private float ballSpeed = -3f;

    //時間計算用変数
    private float delta = 0;

    // Start is called before the first frame update
    void Start()
    {
        //回転を開始する角度を設定
        this.transform.Rotate(0, Random.Range(0, 360), 0);
    }

    // Update is called once per frame
    void Update()
    {
        //回転
        this.transform.Rotate(0, this.rotSpeed, 0);

        // 移動
        transform.Translate(this.ballSpeed * Time.deltaTime, 0, 0, Space.World);

        //画面左端を超えるとゲームオーバー
        if (this.transform.position.x <= -9)
        {
            Time.timeScale = 0;
            delta += Time.deltaTime;
            if (delta >= 2)
            {
                //破壊
                Destroy(this.gameObject);
            }
        }
    }
}
