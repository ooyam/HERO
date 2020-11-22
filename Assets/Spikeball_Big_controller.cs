﻿using UnityEngine;
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
    }
}