using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Controller : MonoBehaviour
{
    //移動速度
    private float speed = 4.0f;
    //時間計算用変数
    private float delta;

    //スパイクボールを入れる
    public GameObject spikeball_bigPrefab;
    public GameObject spikeball_smallPrefab;

    //耐久値
    private float hp = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //定位置まで移動
        if (this.transform.position.y > 3.5)
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
        }
        else if (this.transform.position.y < -4)
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
        }

        //時間計算
        delta += Time.deltaTime;

        //スパイクボール生成
        if (this.transform.position.y > 0 && delta >= 2)
        {
            GameObject Spikeball_big = Instantiate(spikeball_bigPrefab);
            Spikeball_big.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
            delta = 0;
        }
        else if (this.transform.position.y < 0 && delta >= 2)
        {
            GameObject Spikeball_small = Instantiate(spikeball_smallPrefab);
            Spikeball_small.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
            delta = 0;
        }

        //耐久値が0になったら破壊
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
