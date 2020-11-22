using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Controller : MonoBehaviour
{
    // 回転速度
    private float rotSpeed = -200f;

    // 移動速度
    private float Speed = -3f;

    //耐久値
    private float hp = 10.0f;

    //人1～4を入れる
    public GameObject Human1;
    public GameObject Human2;
    public GameObject Human3;
    public GameObject Human4;
    private int range;
    //gameobject取得用
    private GameObject human;
    //時間停止用変数
    private int stop = 0;

    // Materialを入れる
    Material myMaterial;
    // ターゲットのデフォルトの色
    Color defaultColor = new Color32(20, 0, 0, 0);
    // Emissionの最小値
    private float minEmission = 4f;
    // Emissionの強度
    private float magEmission = 5f;
    //発行速度
    private float LightSpeed = 10f;
    private float degree = 180;

    // Start is called before the first frame update
    void Start()
    {
        //回転を開始する角度を設定
        this.transform.Rotate(0, Random.Range(0, 360), 0);
        //人のランダム生成
        range = Random.Range(1, 5);

        //オブジェクトにアタッチしているMaterialを取得
        this.myMaterial = GetComponent<Renderer>().material;
        //オブジェクトの最初の色を設定
        myMaterial.SetColor("_EmissionColor", this.defaultColor * minEmission);
    }

    // Update is called once per frame
    void Update()
    {
        //回転
        this.transform.Rotate(0, this.rotSpeed * Time.deltaTime, 0);

        // 移動
        if (this.transform.position.x >= -11)
        {
            transform.Translate(this.Speed * Time.deltaTime, 0, 0, Space.World);
        }
        //ゲームオーバー
        else
        {
            Destroy(this.gameObject);
            //時間停止
            Time.timeScale = 0;
            //ゲームオーバー画面

        }

        //画面左端に来るとオブジェクト点滅
        if (this.transform.position.x <= -9)
        {
            if (stop == 0)
            {
                //時間停止
                Time.timeScale = 0;
                //DestroyCoroutineを実行
                StartCoroutine(WaitTimeCoroutine());
                stop = 1;
            }

            if (Time.timeScale == 0)
            {
                // 光らせる強度を計算する
                Color emissionColor = this.defaultColor * (this.minEmission + Mathf.Sin(this.degree * Mathf.Deg2Rad) * this.magEmission);

                // エミッションに色を設定する
                myMaterial.SetColor("_EmissionColor", emissionColor);

                degree -= LightSpeed;
            }
        }

        //耐久値が0になったら破壊
        if (hp <= 0)
        {
            //人を生成
            if (range == 1)
            {
                human = Instantiate(Human1);
            } 
            else if (range == 2)
            {
                human = Instantiate(Human2);
            }
            else if (range == 3)
            {
                human = Instantiate(Human3);
            }
            else if (range == 4)
            {
                human = Instantiate(Human4);
            }
            human.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
            Destroy(this.gameObject);
        }
    }
    IEnumerator WaitTimeCoroutine()
    {
        yield return new WaitForSecondsRealtime(1.3f);
        Time.timeScale = 1;
    }
}
