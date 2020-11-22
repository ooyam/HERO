using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human_controller : MonoBehaviour
{
    //落下速度
    private float speed = -4;
    //時間停止用変数
    private int stop = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //落下
        if (this.transform.position.y > -6)
        {
            transform.Translate(0, speed * Time.deltaTime, 0,Space.World);
        }
        //ゲームオーバー
        else
        {
            Destroy(this.gameObject);
            //時間停止
            Time.timeScale = 0;
            //ゲームオーバー画面

        }

        //画面下端に来るとオブジェクト点滅
        if (this.transform.position.y <= -5 && stop == 0)
        {
            //時間停止
            Time.timeScale = 0;
            //DestroyCoroutineを実行
            StartCoroutine(WaitTimeCoroutine());
            stop = 1;
        }
    }
    IEnumerator WaitTimeCoroutine()
    {
        for (int i = 1; i <= 3; i++)
        {
            GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 150);
            yield return new WaitForSecondsRealtime(0.2f);
            GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 50);
            yield return new WaitForSecondsRealtime(0.3f);
        }
        Time.timeScale = 1;
    }
}
