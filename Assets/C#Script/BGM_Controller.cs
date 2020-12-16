using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Controller : MonoBehaviour
{
    //componentを入れる
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
    //WaveEffectの有無確認用変数
    private int WaveCounts;

    // Start is called before the first frame update
    void Start()
    {
        //componentを取得
        this.Audio = GetComponent<AudioSource>();
        //GameOver_Textのオブジェクトを取得する
        GameOverObj = GameObject.Find("GameOver_Text");
        //GameOver_Textのスクリプトを取得する
        GameOverScr = GameOverObj.GetComponent<GameOver_Text_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        //WaveEffectの有無
        WaveCounts = GameObject.FindGameObjectsWithTag("WaveEffect").Length;
        //GameOverかどうか監視、Trueの場合BGM中止
        GameOver = GameOverScr.GameOver;
        if ((GameOver == true || WaveCounts >= 1)&& this.Audio.volume != 0f)
        {
            BGMStop();
        }
    }
    public void BGMStop()
    {
            this.Audio.volume = 0;
    }
    public void BGMPlay()
    {
        this.Audio.Play();
    }
    public void BGMStart()
    {
        //VolumeCoroutineを実行
        StartCoroutine(VolumeCoroutine());
    }
    IEnumerator VolumeCoroutine()
    {
        //徐々にVolumeを上げる
        for (int i = 10; i >= 1; i--)
        {
            this.Audio.volume += 0.03f;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
