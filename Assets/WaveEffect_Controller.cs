using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEffect_Controller : MonoBehaviour
{
    //コンポーネントを入れる
    private AudioSource Audio;

    //BGM_Controllerを入れる
    private GameObject BGM;
    //BGM_ControllerのComponentを入れる
    private BGM_Controller BGMCon;
    //GameOver_Textのオブジェクトを入れる
    private GameObject GameOverObj;
    //GameOver_Textのスクリプトを入れる
    private GameOver_Text_Controller GameOverScr;
    //GameOverの判断用変数
    private bool GameOver;

    // Start is called before the first frame update
    void Start()
    {
        //GameOver_Textのオブジェクトを取得する
        GameOverObj = GameObject.Find("GameOver_Text");
        //GameOver_Textのスクリプトを取得する
        GameOverScr = GameOverObj.GetComponent<GameOver_Text_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        //GameOverかどうか監視、Trueの場合はBGM中止
        GameOver = GameOverScr.GameOver;
        if (GameOver == true)
        {
            Cancel();
        }
    }
    public void StartEffect()
    {
        //コンポーネントを取得する
        this.Audio = GetComponent<AudioSource>();

        //BGM_Controllerを取得
        BGM = GameObject.Find("BGMController");
        //BGM_Controllerのスクリプトを取得
        BGMCon = BGM.GetComponent<BGM_Controller>();

        //通常用のBGMを消す
        BGMCon.BGMStop();
        //Wave用BGM開始
        this.Audio.Play();
    }
    public void EndEffect()
    {
        //VolumeCoroutineを実行
        StartCoroutine(VolumeCoroutine());
    }
    IEnumerator VolumeCoroutine()
    {
        //徐々にVolumeを下げる
        for (int i = 10; i >= 0; i--)
        {
            this.Audio.volume -= 0.04f;
            yield return new WaitForSecondsRealtime(0.2f);
            if (i <= 0)
            {
                //通常用のBGMを再開
                BGMCon.BGMStart();
                //自身を破壊
                Destroy(this.gameObject);
            }
        }
    }
    public void Cancel()
    {
        //自身を破壊(中止)
        Destroy(this.gameObject);
    }
}
