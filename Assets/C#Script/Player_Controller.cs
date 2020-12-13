using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{
    //アニメーションするためのコンポーネントを入れる
    private Animator myAnimator;
    //BoxColliderのコンポーネントを入れる
    private BoxCollider myColliderComponent;
    //自身のAudioを入れる
    private AudioSource Audio;
    //Voiceを入れる
    public AudioClip Voice;
    //PinchVoiceを入れる
    public AudioClip Pinch;
    //CatchSEを入れる
    public AudioClip CatchSE;
    //RecoverySEを入れる
    public AudioClip RecoverySE;
    //CatchSEを1回だけ鳴らすための変数
    private bool CatchSeCount = false;
    //ItemSeを入れる
    public AudioClip ItemSe;

    //Player体力
    public int HP = 100;
    //移動速度
    private float speed = 7.5f;
    //横方向の移動速度
    float inputVelocityX = 0;
    //縦方向の移動速度
    float inputVelocityY = 0;
    //攻撃時の前進速度
    private float Attackspeed = 16.0f;
    //左右の移動できる範囲
    private float WidthRange = 7.5f;
    //上下の移動できる範囲
    private float HeightRange = 4f;
    //現在のposition用変数
    private Vector3 pos;
    //現在のrotation用変数
    private Vector3 rot;
    //攻撃時移動用
    private int AttakMove = 0;
    //時間計算用変数
    private float delta = 0;
    //WaveEffect時間計算用変数
    private float EaveEffectDelta = 0;
    //transformキャッシュ用
    private Transform _transform;
    //アニメーション状態取得用変数
    private bool Catch;
    private bool SlideStart;
    private bool Slide;
    private bool SlideEnd;
    private bool CatchRun;
    private bool Death;
    private bool DeathEnd;
    //PlayerEffectの有無確認用変数
    private int EffectCounts;

    //GameOver_Textのゲームオブジェクト/スクリプトを入れる
    private GameObject GameOverText;
    private GameOver_Text_Controller GameOverTextScr;
    //score_textのゲームオブジェクト/スクリプトを入れる
    private GameObject ScoreText;
    private score_text_Controller ScoreTextScr;
    //Player HPのゲームオブジェクト/スクリプトを入れる
    private GameObject PlayerHp;
    private Plyaer_HP_Controller PlayerHpScr;
    //Player_effectのゲームオブジェクトを入れる
    public GameObject PlayerEffect;
    //Player_effectのインスタンス用変数
    private GameObject Effect;

    //Wave_effectのゲームオブジェクトを入れる
    public GameObject WaveEffect;
    //Player_effectのインスタンス用変数
    private GameObject Wave;
    //Wave_effectのコンポーネントを入れる
    private WaveEffect_Controller WaveCon;
    //WaveEffectの有無確認用変数
    private int WaveCounts;

    // Start is called before the first frame update
    void Start()
    {
        //Animatorコンポーネントを取得
        this.myAnimator = GetComponent<Animator>();
        //GameOver_Textゲームオブジェクト/スクリプトの取得
        GameOverText = GameObject.Find("GameOver_Text");
        GameOverTextScr = GameOverText.GetComponent<GameOver_Text_Controller>();
        //score_textゲームオブジェクト/スクリプトの取得
        ScoreText = GameObject.Find("score_text");
        ScoreTextScr = ScoreText.GetComponent<score_text_Controller>();
        //Player HPゲームオブジェクト/スクリプトの取得
        PlayerHp = GameObject.Find("Player HP");
        PlayerHpScr = PlayerHp.GetComponent<Plyaer_HP_Controller>();
        //BoxColliderのコンポーネント取得
        myColliderComponent = GetComponent<BoxCollider>();
        //transformをキャッシュしておく
        _transform = transform;
        //Audioを取得
        Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            //現在値/向き把握用
            this.pos = _transform.position;
            this.rot = _transform.localEulerAngles;
            // アニメーション状態取得
            SlideStart = myAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Slide-start"));
            Slide = myAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Slide"));
            SlideEnd = myAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Slide-end"));
            Catch = myAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Catch"));
            CatchRun = myAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Catch-Run"));
            Death = myAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Death"));
            DeathEnd = myAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash.Equals(Animator.StringToHash("Death-end"));
            //PlayerEffectの有無
            EffectCounts = GameObject.FindGameObjectsWithTag("Player Effect").Length;
            //WaveEffectの有無
            WaveCounts = GameObject.FindGameObjectsWithTag("WaveEffect").Length;

            // スペースキーを押したとき(攻撃動作)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AttackButtonDown();
            }
            //攻撃動作時常時稼働
            if (SlideStart == true || Slide == true && SlideEnd == false)
            {
                //画面端で画面外側を向いている際は､攻撃時前進しない
                //右端右向き
                if (this.pos.x >= WidthRange && (this._transform.rotation == Quaternion.Euler(0, 0, 0) || this._transform.rotation == Quaternion.Euler(0, 0, 45) || this._transform.rotation == Quaternion.Euler(0, 0, -45)))
                {
                    Slide_End();
                }
                //左端左向き
                else if (this.pos.x <= -WidthRange && (this._transform.rotation == Quaternion.Euler(0, 180, 0) || this._transform.rotation == Quaternion.Euler(0, 180, 45) || this._transform.rotation == Quaternion.Euler(0, 180, -45)))
                {
                    Slide_End();
                }
                //上端上向き
                else if (this.pos.y >= HeightRange && (this._transform.rotation == Quaternion.Euler(0, 0, 90) || this._transform.rotation == Quaternion.Euler(0, 180, 45) || this._transform.rotation == Quaternion.Euler(0, 0, 45)))
                {
                    Slide_End();
                }
                //下端下向き
                else if (this.pos.y <= -HeightRange && (this._transform.rotation == Quaternion.Euler(0, 0, -90) || this._transform.rotation == Quaternion.Euler(0, 180, -45) || this._transform.rotation == Quaternion.Euler(0, 0, -45)))
                {
                    Slide_End();
                }
                //攻撃時前進する
                else
                {
                    this.AttakMove = 1;
                }
            }
            //攻撃時前進
            if (this.AttakMove == 1 && (SlideStart == true || Slide == true))
            {
                _transform.Translate(Attackspeed * Time.deltaTime, 0, 0);
                //エフェクト移動
                if (EffectCounts >= 1)
                {
                    Effect.transform.position = new Vector3(this.pos.x - 0.4f, this.pos.y - 0.67f, -5f);
                    Effect.transform.rotation = Quaternion.Euler(this.rot.x, this.rot.y, this.rot.z);
                }
                delta += Time.deltaTime;
                if (delta >= 0.25f)
                {
                    delta = 0;
                    this.AttakMove = 0;
                    this.myAnimator.SetTrigger("Slide-end_trigger");
                    //当たり判定を元に戻す
                    myColliderComponent.center = new Vector3(0.3f, -0.3f, 0f);
                    myColliderComponent.size = new Vector3(1.6f, 1.8f, 20f);
                }
            }

            //Human救助動作
            if (Input.GetKeyDown(KeyCode.C))
            {
                CatchButtonDown();
            }
            //Humanを離す
            if (Input.GetKeyDown(KeyCode.V))
            {
                ReleaseButtonDown();
            }

            //CatchRun中は移動速度減
            this.speed = CatchRun ? 5.0f : speed;

            //playerの移動(攻撃動作時・PlayerDeth時は移動不可にする)
            if (SlideStart == false && Slide == false && SlideEnd == false && Death == false && DeathEnd == false)
            {
                //右上移動
                if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow))
                {
                    //体の向きを変える
                    _transform.rotation = Quaternion.Euler(0, 0, 45);
                    if (this.WidthRange <= pos.x && this.HeightRange <= pos.y)
                    {
                        DontRun();
                    }
                    else if (this.WidthRange <= pos.x)
                    {
                        UpMove();
                    }
                    else if (this.HeightRange <= pos.y)
                    {
                        RightMove();
                    }
                    else
                    {
                        RightUpMove();
                    }
                }
                //左上移動
                else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow))
                {
                    _transform.rotation = Quaternion.Euler(0, 180, 45);
                    if (-this.WidthRange >= pos.x && this.HeightRange <= pos.y)
                    {
                        DontRun();
                    }
                    else if (-this.WidthRange >= pos.x)
                    {
                        UpMove();
                    }
                    else if (this.HeightRange <= pos.y)
                    {
                        LeftMove();
                    }
                    else
                    {
                        LeftUpMove();
                    }
                }
                //右下移動
                else if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow))
                {
                    _transform.rotation = Quaternion.Euler(0, 0, -45);
                    if (this.WidthRange <= pos.x && -this.HeightRange >= pos.y)
                    {
                        DontRun();
                    }
                    else if (this.WidthRange <= pos.x)
                    {
                        DownMove();
                    }
                    else if (-this.HeightRange >= pos.y)
                    {
                        RightMove();
                    }
                    else
                    {
                        RightDownMove();
                    }
                }
                //左下移動
                else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.DownArrow))
                {
                    _transform.rotation = Quaternion.Euler(0, 180, -45);
                    if (-this.WidthRange >= pos.x && -this.HeightRange >= pos.y)
                    {
                        DontRun();
                    }
                    else if (-this.WidthRange >= pos.x)
                    {
                        DownMove();
                    }
                    else if (-this.HeightRange >= pos.y)
                    {
                        LeftMove();
                    }
                    else
                    {
                        LeftDownMove();
                    }
                }
                //右移動
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    _transform.rotation = Quaternion.Euler(0, 0, 0);
                    if (this.WidthRange <= pos.x)
                    {
                        DontRun();
                    }
                    else
                    {
                        RightMove();
                    }
                }
                //左移動
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    _transform.rotation = Quaternion.Euler(0, 180, 0);
                    if (-this.WidthRange >= pos.x)
                    {
                        DontRun();
                    }
                    else
                    {
                        LeftMove();
                    }
                }
                //上移動
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    _transform.rotation = Quaternion.Euler(0, 0, 90);
                    if (this.HeightRange <= pos.y)
                    {
                        DontRun();
                    }
                    else
                    {
                        UpMove();
                    }
                }
                //下移動
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    _transform.rotation = Quaternion.Euler(0, 0, -90);
                    if (-this.HeightRange >= pos.y)
                    {
                        DontRun();
                    }
                    else
                    {
                        DownMove();
                    }
                }
            }
            //速度を与えて移動させる
            this._transform.Translate(inputVelocityX * Time.deltaTime, inputVelocityY * Time.deltaTime, 0, Space.World);
            //Runをやめる
            if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                DontRun();
            }
            //WaveEffectをPlayerに追従させる
            if(WaveCounts >= 1)
            {
                Wave.transform.position = new Vector3(this.pos.x + 0.3f, this.pos.y - 0.3f, 1);
                EaveEffectDelta += Time.deltaTime;
                if(EaveEffectDelta >= 15f)
                {
                    //ウェーブエフェクトのコンポーネント取得BGM切り替え
                    WaveCon.EndEffect();
                    EaveEffectDelta = 0;
                }
            }
            //CatchSeCountをリセットする
            if (Catch == false)
            {
                CatchSeCount = false;
            }
            //Wave発動時はCatchRun解除
            if (CatchRun == true && WaveCounts >= 1)
            {
                ReleaseButtonDown();
            }
        }
    }
    void Slide_End()
    {
        this.AttakMove = 0;
        this.myAnimator.SetTrigger("Slide-end_trigger");
        //当たり判定を元に戻す
        myColliderComponent.center = new Vector3(0.3f, -0.3f, 0f);
        myColliderComponent.size = new Vector3(1.6f, 1.8f, 20f);
    }

    public void RecoveryButton()
    {
        //RecoverySEを出す
        Audio.PlayOneShot(RecoverySE);
        //体力回復
        if (this.HP <= 50)
        {
            this.HP += 50;
        }
        else
        {
            int hp1 = this.HP + 50;
            int hp2 = hp1 - 100;
            this.HP = hp1 - hp2;
        }
        //Player HPを増やす
        PlayerHpScr.Scale();
    }
    public void WaveButton()
    {
        if (WaveCounts >= 1)
        {
            //ウェーブエフェクトの重複を防ぐ
            WaveCon.Cancel();
        }
        //ウェーブエフェクトを呼び出す
        Wave = Instantiate(WaveEffect);
        Wave.transform.position = new Vector3(this.pos.x, this.pos.y, -5.1f);
        //ウェーブエフェクトのコンポーネント取得
        WaveCon = Wave.GetComponent<WaveEffect_Controller>();
        EaveEffectDelta = 0;
    }
    public void AttackButtonDown()
    {
        if (SlideStart != true && Slide != true && SlideEnd != true && Catch != true)
        {
            //自身のコンポーネントから当たり判定を変更する
            myColliderComponent.center = new Vector3(-1.7f, -1f, 0f);
            myColliderComponent.size = new Vector3(5.5f, 4f, 50f);
            // Animatorコンポーネントを取得し、"Slide-start_trigger""Slide_trigger"をtrueにする
            this.myAnimator.Play("Slide-start", 0, 0.0f);
            this.myAnimator.Play("Slide", 0, 0.0f);
            //エフェクトを出力する
            Effect = Instantiate(PlayerEffect);
            Effect.transform.position = new Vector3(this.pos.x - 0.4f, this.pos.y - 0.67f, -5f);
            Effect.transform.rotation = Quaternion.Euler(this.rot.x, this.rot.y, this.rot.z);
        }
    }
    public void CatchButtonDown()
    {
        // Animatorコンポーネントを取得し、"Catch_trigger"をtrueにする
        this.myAnimator.SetTrigger("Catch_trigger");
    }
    public void ReleaseButtonDown()
    {
        if (CatchRun == true)
        {
            // Animatorコンポーネントを取得し、"Catch_trigger"をtrueにする
            this.myAnimator.SetTrigger("Lose_trigger");
            this.myAnimator.SetBool("Catch-Run_bool", false);
        }

    }
    void OnTriggerStay(Collider other)
    {
        //Humanをつかむ
        if (other.gameObject.tag == "Human" && Catch == true)
        {
            this.myAnimator.SetBool("Catch-Run_bool", true);
        }
        //CatchRunがtrueになった時､1度のみSEを鳴らす
        if (Catch == true && CatchSeCount == false && other.gameObject.tag == "Human")
        {
            Audio.PlayOneShot(CatchSE);
            CatchSeCount = true;
        }
        //ダメージを受ける
        if ((Catch == true || CatchRun == true) && (other.gameObject.tag == "White ball" || other.gameObject.tag == "Red ball"))
        {
            this.HP -= 10;
            if(HP == 30)
            {
                //声を出す
                Audio.PlayOneShot(Pinch);
            }
            else
            {
                //声を出す
                Audio.PlayOneShot(Voice);
            }
            //Player HPを減らす
            PlayerHpScr.Scale();
            this.myAnimator.SetTrigger("Death_trigger");
            this._transform.rotation = Quaternion.Euler(0, 0, 0);
            //HP0でゲームオーバー
            if (this.HP <= 0)
            {
                this.myAnimator.SetBool("Death-end_bool", true);
                //ゲームオーバー画面の呼び出し
                GameOverTextScr.GameOverJudge("Player");
                //score表示を消す
                ScoreTextScr.GameOverJudge();
            }
        }
    }
    public void RightMove()
    {
        if (SlideStart == false && Slide == false && SlideEnd == false && Death == false && DeathEnd == false)
        {
            //速度を代入
            inputVelocityX = this.speed;
            // Animatorコンポーネントを取得し、"Run_trigger"をtrueにする
            this.myAnimator.SetTrigger("Run_trigger");
            this.myAnimator.SetBool("Idle_bool", false);
            inputVelocityY = 0;
        }
        else
        {
            DontRun();
        }
    }
    public void LeftMove()
    {
        if (SlideStart == false && Slide == false && SlideEnd == false && Death == false && DeathEnd == false)
        {
            inputVelocityX = -this.speed;
            this.myAnimator.SetTrigger("Run_trigger");
            this.myAnimator.SetBool("Idle_bool", false);
            inputVelocityY = 0;
        }
        else
        {
            DontRun();
        }
    }
    public void UpMove()
    {
        if (SlideStart == false && Slide == false && SlideEnd == false && Death == false && DeathEnd == false)
        {
            inputVelocityY = this.speed;
            this.myAnimator.SetTrigger("Run_trigger");
            this.myAnimator.SetBool("Idle_bool", false);
            inputVelocityX = 0;
        }
        else
        {
            DontRun();
        }
    }
    public void DownMove()
    {
        if (SlideStart == false && Slide == false && SlideEnd == false && Death == false && DeathEnd == false)
        {
            inputVelocityY = -this.speed;
            this.myAnimator.SetTrigger("Run_trigger");
            this.myAnimator.SetBool("Idle_bool", false);
            inputVelocityX = 0;
        }
        else
        {
            DontRun();
        }
    }
    public void RightUpMove()
    {
        if (SlideStart == false && Slide == false && SlideEnd == false && Death == false && DeathEnd == false)
        {
            inputVelocityY = this.speed * 0.71f;
            inputVelocityX = this.speed * 0.71f;
            this.myAnimator.SetTrigger("Run_trigger");
            this.myAnimator.SetBool("Idle_bool", false);
        }
        else
        {
            DontRun();
        }
    }
    public void LeftUpMove()
    {
        if (SlideStart == false && Slide == false && SlideEnd == false && Death == false && DeathEnd == false)
        {
            inputVelocityX = -this.speed * 0.71f;
            inputVelocityY = this.speed * 0.71f;
            this.myAnimator.SetTrigger("Run_trigger");
            this.myAnimator.SetBool("Idle_bool", false);
        }
        else
        {
            DontRun();
        }
    }
    public void RightDownMove()
    {
        if (SlideStart == false && Slide == false && SlideEnd == false && Death == false && DeathEnd == false)
        {
            inputVelocityX = this.speed * 0.71f;
            inputVelocityY = -this.speed * 0.71f;
            this.myAnimator.SetTrigger("Run_trigger");
            this.myAnimator.SetBool("Idle_bool", false);
        }
        else
        {
            DontRun();
        }
    }
    public void LeftDownMove()
    {
        if (SlideStart == false && Slide == false && SlideEnd == false && Death == false && DeathEnd == false)
        {
            inputVelocityX = -this.speed * 0.71f;
            inputVelocityY = -this.speed * 0.71f;
            this.myAnimator.SetTrigger("Run_trigger");
            this.myAnimator.SetBool("Idle_bool", false);
        }
        else
        {
            DontRun();
        }
    }
    public void DontRun()
    {
        this.myAnimator.SetBool("Idle_bool", true);
        //移動速度を元に戻す
        inputVelocityX = 0;
        inputVelocityY = 0;
    }
    public void ItemSE()
    {
        this.Audio.PlayOneShot(ItemSe);
    }
}
