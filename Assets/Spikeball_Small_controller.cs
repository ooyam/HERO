using UnityEngine;
using System.Collections;

public class Spikeball_Small_controller : MonoBehaviour
{
    // 回転速度
    private float rotSpeed = 1000f;

    // 玉のx軸移動速度
    private float ballSpeedX = -4.67f;
    // 玉のy軸移動速度
    private float ballSpeedY = 3.77f;

    // Use this for initialization
    void Start()
    {
        //回転を開始する角度を設定
        this.transform.Rotate(0, Random.Range(0, 360), 0);
    }

    // Update is called once per frame
    void Update()
    {
        //回転
        this.transform.Rotate(this.rotSpeed * Time.deltaTime, this.rotSpeed * Time.deltaTime, 0);

        // 玉を撃つ
        transform.Translate(this.ballSpeedX * Time.deltaTime, this.ballSpeedY * Time.deltaTime, 0, Space.World);
    }
}