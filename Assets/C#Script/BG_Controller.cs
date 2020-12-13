using UnityEngine;
using System.Collections;

public class BG_Controller : MonoBehaviour
{

    // スクロール速度
    private float scrollSpeed = -3;
    // 背景終了位置
    private float deadLine = -20;
    // 背景開始位置
    private float startLine = 59.8f;
    //背景速度計算用変数
    private float Level = 1;
    //時間計算用変数
    private float delta;
    //transformをキャッシュする
    private Transform _transform;

    // Use this for initialization
    void Start()
    {
        //transformのキャッシュ
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // 移動速度を計算
        delta += Time.deltaTime;
        if(delta >= 30f)
        {
            Level += 0.1f;
            delta = 0;
        }
        // 背景を移動する
        _transform.Translate(this.scrollSpeed * this.Level * Time.deltaTime, 0, 0);

        // 画面外に出たら、画面右端に移動する
        if (_transform.position.x < this.deadLine)
        {
            _transform.position = new Vector3(this.startLine, 0, 200);
        }
    }
}
