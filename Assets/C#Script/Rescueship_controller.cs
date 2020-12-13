using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rescueship_controller : MonoBehaviour
{
    //耐久値
    public float HP = 151.0f;
    //接触検知用変数
    private bool Contact;
    //時間計算用変数
    private float delta;
    //コルーチン重複防止用
    private bool stop = false;
    //エフェクト重複防止用
    private bool eff = false;

    //エフェクトのゲームオブジェクトを入れる
    public GameObject Effect;
    //Dethエフェクトのゲームオブジェクトを入れる
    public GameObject DethEffect;
    //GameOver_Textのゲームオブジェクト/スクリプトを入れる
    private GameObject GameOverText;
    private GameOver_Text_Controller GameOverTextScr;
    //score_textのゲームオブジェクト/スクリプトを入れる
    private GameObject ScoreText;
    private score_text_Controller ScoreTextScr;
    //HPゲージのゲームオブジェクト/スクリプトを入れる
    private GameObject HpGauge;
    private RescueShipHP_Controller HpGaugeScr;
    //BGMのゲームオブジェクト/スクリプトを入れる
    private GameObject BGM;
    private BGM_Controller BGMScr;
    //Audioを入れる
    private AudioSource Audio;
    //ShipLightを入れる
    public AudioClip LightSE;
    //回復時の効果音を入れる
    public AudioClip RepairSE;
    //自身の色/Transformを取得
    private Renderer Renderer;
    private Transform _transform;
    //WaveEffectの有無確認用変数
    private int WaveCounts;
    //WaveEffectの再開用変数
    public bool WaveRestart;

    // Start is called before the first frame update
    void Start()
    {
        //GameOver_Textゲームオブジェクト/スクリプトの取得
        GameOverText = GameObject.Find("GameOver_Text");
        GameOverTextScr = GameOverText.GetComponent<GameOver_Text_Controller>();
        //score_textゲームオブジェクト/スクリプトの取得
        ScoreText = GameObject.Find("score_text");
        ScoreTextScr = ScoreText.GetComponent<score_text_Controller>();
        //HPゲージゲームオブジェクト/スクリプトの取得
        HpGauge = GameObject.Find("Rescue ship HP");
        HpGaugeScr = HpGauge.GetComponent<RescueShipHP_Controller>();
        //BGMControllerゲージゲームオブジェクト/スクリプトの取得
        BGM = GameObject.Find("BGMController");
        BGMScr = BGM.GetComponent<BGM_Controller>();
        //SEを取得
        Audio = GetComponent<AudioSource>();
        //自身の色/Transformを取得
        Renderer = GetComponent<Renderer>();
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        //WaveEffectの有無
        WaveCounts = GameObject.FindGameObjectsWithTag("WaveEffect").Length;

        if (this.Contact == true && this.HP >= 50)
        {
            this.delta += Time.deltaTime;
            if (delta >= 0.3f)
            {
                Renderer.material.color = new Color32(255, 255, 255, 255);
                this.delta = 0;
                Contact = false;
            }
        }
        if (this.HP <= 31 && stop == false)
        {
            this.delta += Time.deltaTime;
            if (this.delta <= 0.5f)
            {
                Renderer.material.color = new Color32(255, 0, 0, 255);
            }
            else
            {
                Renderer.material.color = new Color32(255, 255, 255, 255);
                if (this.delta >= 1f)
                {
                    this.delta = 0;
                    //RepairSEを出す
                    Audio.PlayOneShot(LightSE);
                }
            }
        }
        if (this.HP <= 31 && eff == false)
        {
            //エフェクトを呼び出す
            GameObject effect = Instantiate(Effect);
            effect.transform.position = new Vector3(this._transform.position.x + 1.5f, this._transform.position.y -0.5f,3f);
            eff = true;
        }
        //effリセット
        if (this.HP > 31 && eff == true)
        {
            eff = false;
        }
        //ゲームオーバー
        if (this.HP <= 0 && stop == false)
        {
            //時間停止
            Time.timeScale = 0;
            //DestroyCoroutineを実行
            StartCoroutine(WaitTimeCoroutine());
            //BGMの停止
            BGMScr.BGMStop();
            stop = true;
            WaveRestart = false;
        }
        //stopリセット
        if (this.HP > 0 && stop == true)
        {
            stop = false;
            //時間の再開
            Time.timeScale = 1;
            if (WaveCounts >= 1)
            {
                //WaveBGMの再開
                WaveRestart = true;
            }
            else
            {
                //BGMの再開
                BGMScr.BGMStart();
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Red ball")
        {
            Contact = true;
            this.HP -= 10;
            Renderer.material.color = new Color32(255, 0, 0, 255);
            //HPゲージを減らす
            HpGaugeScr.Scale();
        }
    }
    public void RepairButton()
    {
        //RepairSEを出す
        Audio.PlayOneShot(RepairSE);
        //体力回復
        if (this.HP <= 51)
        {
            this.HP += 100f;
        }
        else
        {
            float hp1 = this.HP + 100.0f;
            float hp2 = hp1 - 151.0f;
            this.HP = hp1 - hp2;
        }
        //HPゲージを増やす
        HpGaugeScr.Scale();
    }
    IEnumerator WaitTimeCoroutine()
    {
        //ゲームオーバー遅延
        if (this.HP <= 0)
        {
            for (int i = 1; i <= 3; i++)
            {
                Renderer.material.color = new Color32(255, 0, 0, 255);
                //SEを出す
                Audio.PlayOneShot(LightSE);
                yield return new WaitForSecondsRealtime(0.2f);
                Renderer.material.color = new Color32(255, 255, 255, 255);
                yield return new WaitForSecondsRealtime(0.2f);
                if(i == 3)
                {
                    GameOver();
                }
            }
        }
    }
    void GameOver()
    {
        if (this.HP <= 0)
        {
            //Dethエフェクトを呼び出す
            GameObject Detheffect = Instantiate(DethEffect);
            Detheffect.transform.position = new Vector3(this._transform.position.x + 1.5f, this._transform.position.y, 0);
            //破壊
            Destroy(this.gameObject);
            Time.timeScale = 1;
            //ゲームオーバー画面の呼び出し
            GameOverTextScr.GameOverJudge("Ship");
            //score表示を消す
            ScoreTextScr.GameOverJudge();
        }
    }
}
