using UnityEngine;
using System.Collections;

public class Spikeball_Big_controller : MonoBehaviour
{
    // 回転速度
    private float rotSpeed = 1000f;

    // 玉の移動速度
    private float ballSpeed = -6;

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
        transform.Translate(this.ballSpeed * Time.deltaTime, 0, 0, Space.World);

        //画面外に出たら破壊
        if(this.transform.position.x < -10.5f || this.transform.position.x > 10.5f || this.transform.position.y < -6 || this.transform.position.y > 6)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Rescue ship")
        {
            Destroy(this.gameObject);
        }
    }
}