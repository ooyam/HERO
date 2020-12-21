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
    private int GameOverText;
    //Rescue shipを入れる
    private GameObject Ship;
    //Rescue shipのスクリプトを入れる
    private Rescueship_controller ShipScr;
    //Rescue shipのHP監視用変数
    private float ShipHP;
    //BGM再開用変数
    private bool WaveRestart;

    // Start is called before the first frame update
    void Start()
    {
        //コンポーネントを取得する
        this.Audio = GetComponent<AudioSource>();
        //BGM_Controller/スクリプトを取得
        BGM = GameObject.Find("BGMController");
        BGMCon = BGM.GetComponent<BGM_Controller>();
        //GameOver_Textのオブジェクト/スクリプトを取得する
        GameOverObj = GameObject.Find("GameOver_Text");
        GameOverScr = GameOverObj.GetComponent<GameOver_Text_Controller>();
        //Rescue shipのオブジェクトを/スクリプト取得する
        Ship = GameObject.Find("Rescue ship");
        ShipScr = Ship.GetComponent<Rescueship_controller>();
        //BGM開始
        StartBGM();
    }

    // Update is called once per frame
    void Update()
    {
        //GameOverかどうか監視、Trueの場合はBGM中止
        GameOver = GameOverScr.GameOver;
        GameOverText = GameOverScr._Text;
        //Rescue shipのHPを監視、0以下の場合はBGM中止
        ShipHP = ShipScr.HP;
        //BGMを再開するか監視、Trueの場合はBGM再開
        WaveRestart = ShipScr.WaveRestart;
        if (GameOver == true || ShipHP <= 0)
        {
            Pose();
        }
        if (WaveRestart == true)
        {
            Restart();
        }
        if(GameOverText >= 1 && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            Destroy(this.gameObject);
        }
    }
    public void StartEffect()
    {
        //遅延(スタート関数の実行を待つため)
        Invoke("StartBGM", 0.1f);
    }
    void StartBGM()
    {
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
                //自身を破壊
                Destroy(this.gameObject);
                //通常用のBGMを再開
                BGMCon.BGMStart();
            }
        }
    }
    public void Cancel()
    {
        //自身を破壊
        Destroy(this.gameObject);
    }
    void Pose()
    {
        //BGM中断
        this.Audio.volume = 0;
    }
    void Restart()
    {
        //BGM再開
        this.Audio.volume = 0.4f;
    }
}
