using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    //アニメーションするためのコンポーネントを入れる
    private Animator myAnimator;
    //BoxColliderのコンポーネントを入れる
    private BoxCollider myColliderComponent;


    //Player体力
    private float HP = 100.0f;
    //移動速度
    private float speed;
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

    //GameOver_Textのゲームオブジェクトを入れる
    private GameObject GameOverText;
    //score_textのゲームオブジェクトを入れる
    private GameObject ScoreText;
    //Player_effectのゲームオブジェクトを入れる
    public GameObject PlayerEffect;
    //Player_effectのインスタンス用変数
    public GameObject Effect;

    // Start is called before the first frame update
    void Start()
    {
        //Animatorコンポーネントを取得
        this.myAnimator = GetComponent<Animator>();
        //GameOver_Textゲームオブジェクトの取得
        GameOverText = GameObject.Find("GameOver_Text");
        //score_textゲームオブジェクトの取得
        ScoreText = GameObject.Find("score_text");
        //BoxColliderのコンポーネント取得
        myColliderComponent = GetComponent<BoxCollider>();
        //transformをキャッシュしておく
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
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

        // スペースキーを押したとき(攻撃動作)
        if (Input.GetKeyDown(KeyCode.Space) && SlideStart != true && Slide != true && SlideEnd != true)
        {
            // Animatorコンポーネントを取得し、"Slide-start_trigger""Slide_trigger"をtrueにする
            this.myAnimator.SetTrigger("Slide-start_trigger");
            this.myAnimator.SetTrigger("Slide_trigger");
            //エフェクトを出力する
            Effect = Instantiate(PlayerEffect);
            Effect.transform.position = new Vector3(this.pos.x - 0.4f, this.pos.y - 0.67f, -5f);
            Effect.transform.rotation = Quaternion.Euler(this.rot.x, this.rot.y, this.rot.z);
            //自身のコンポーネントから当たり判定を変更する
            myColliderComponent.center = new Vector3(0f, -1f, 0f);
            myColliderComponent.size = new Vector3(2f, 3f, 20f);
        }
        //攻撃動作時常時稼働
        if (SlideStart == true || Slide == true && SlideEnd == false)
        {
            //画面端で画面外側を向いている際は､攻撃時前進しない
            //右端右向き
            if (this.pos.x >= WidthRange && (this.transform.rotation == Quaternion.Euler(0, 0, 0) || this.transform.rotation == Quaternion.Euler(0, 0, 45) || this.transform.rotation == Quaternion.Euler(0, 0, -45)))
            {
                Slide_End();
            }
            //左端左向き
            else if (this.pos.x <= -WidthRange && (this.transform.rotation == Quaternion.Euler(0, 180, 0) || this.transform.rotation == Quaternion.Euler(0, 180, 45) || this.transform.rotation == Quaternion.Euler(0, 180, -45)))
            {
                Slide_End();
            }
            //上端上向き
            else if (this.pos.y >= HeightRange && (this.transform.rotation == Quaternion.Euler(0, 0, 90) || this.transform.rotation == Quaternion.Euler(0, 180, 45) || this.transform.rotation == Quaternion.Euler(0, 0, 45)))
            {
                Slide_End();
            }
            //下端下向き
            else if (this.pos.y <= -HeightRange && (this.transform.rotation == Quaternion.Euler(0, 0, -90) || this.transform.rotation == Quaternion.Euler(0, 180, -45) || this.transform.rotation == Quaternion.Euler(0, 0, -45)))
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
            transform.Translate(Attackspeed * Time.deltaTime, 0, 0);
            //エフェクト移動
            if (EffectCounts >= 1)
            {
                Effect.transform.Translate(Attackspeed * Time.deltaTime, 0, 0);
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
                if (EffectCounts >= 1)
                {
                    //エフェクトを破壊する
                    Effect.GetComponent<Player_effect_Controller>().DestroyObj();
                }
            }
        }

        //Human救助動作
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Animatorコンポーネントを取得し、"Catch_trigger"をtrueにする
            this.myAnimator.SetTrigger("Catch_trigger");
        }
        //Humanを離す
        if(Input.GetKeyDown(KeyCode.V) && CatchRun == true)
        {
            // Animatorコンポーネントを取得し、"Catch_trigger"をtrueにする
            this.myAnimator.SetTrigger("Lose_trigger");
            this.myAnimator.SetBool("Catch-Run_bool", false);
        }

        //横方向の移動速度
        float inputVelocityX = 0;
        //縦方向の移動速度
        float inputVelocityY = 0;
        //CatchRun中は移動速度減
        this.speed = CatchRun ? 5.0f : 7.5f;

        //playerの移動(攻撃動作時・PlayerDeth時は移動不可にする)
        if (SlideStart == false && Slide == false && SlideEnd == false && Death == false && DeathEnd == false)
        {
            //左移動
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                //体の向きを変える
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
                if (-this.WidthRange < this.pos.x)
                {
                    //速度を代入
                    inputVelocityX = -this.speed;
                    // Animatorコンポーネントを取得し、"Run_trigger"をtrueにする
                    this.myAnimator.SetTrigger("Run_trigger");
                    this.myAnimator.SetBool("Idle_bool", false);
                }

                //左上
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    this.transform.rotation = Quaternion.Euler(0, 180, 45);
                    if (this.HeightRange > this.pos.y)
                    {
                        inputVelocityY = this.speed * 0.71f;
                        inputVelocityX *= 0.71f;
                    }
                }
                //左下
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    this.transform.rotation = Quaternion.Euler(0, 180, -45);
                    if (-this.HeightRange < this.pos.y)
                    {
                        inputVelocityY = -this.speed * 0.71f;
                        inputVelocityX *= 0.71f;
                    }
                }
            }
            //右移動
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                if (this.WidthRange > this.pos.x)
                {
                    inputVelocityX = this.speed;
                    this.myAnimator.SetTrigger("Run_trigger");
                    this.myAnimator.SetBool("Idle_bool", false);
                }

                //右上
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    this.transform.rotation = Quaternion.Euler(0, 0, 45);
                    if (this.HeightRange > this.pos.y)
                    {
                        inputVelocityY = this.speed * 0.71f;
                        inputVelocityX *= 0.71f;
                    }
                }
                //右下
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    this.transform.rotation = Quaternion.Euler(0, 0, -45);
                    if (-this.HeightRange < this.pos.y)
                    {
                        inputVelocityY = -this.speed * 0.71f;
                        inputVelocityX *= 0.71f;
                    }
                }
            }
            //上移動
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 90);
                if (this.HeightRange > this.pos.y)
                {
                    inputVelocityY = this.speed;
                    this.myAnimator.SetTrigger("Run_trigger");
                    this.myAnimator.SetBool("Idle_bool", false);
                }

                //左上
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    this.transform.rotation = Quaternion.Euler(0, 180, 45);
                    if (-this.WidthRange < this.pos.x)
                    {
                        inputVelocityX = -this.speed * 0.71f;
                        inputVelocityY *= 0.71f;
                    }
                }
                //右上
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    this.transform.rotation = Quaternion.Euler(0, 0, 45);
                    if (this.WidthRange > this.pos.x)
                    {
                        inputVelocityX = this.speed * 0.71f;
                        inputVelocityY *= 0.71f;
                    }
                }
            }
            //下移動
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                this.transform.rotation = Quaternion.Euler(0, 0, -90);
                if (-this.HeightRange < this.pos.y)
                {
                    inputVelocityY = -this.speed;
                    this.myAnimator.SetTrigger("Run_trigger");
                    this.myAnimator.SetBool("Idle_bool", false);
                }

                //左下
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    this.transform.rotation = Quaternion.Euler(0, 180, -45);
                    if (-this.WidthRange < this.pos.x)
                    {
                        inputVelocityX = -this.speed * 0.71f;
                        inputVelocityY *= 0.71f;
                    }
                }
                //右下
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    this.transform.rotation = Quaternion.Euler(0, 0, -45);
                    if (this.WidthRange > this.pos.x)
                    {
                        inputVelocityX = this.speed * 0.71f;
                        inputVelocityY *= 0.71f;
                    }
                }
            }
        }
        //速度を与えて移動させる
        this.transform.Translate(inputVelocityX * Time.deltaTime, inputVelocityY * Time.deltaTime, 0, Space.World);
        //Runをやめる
        if(Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            this.myAnimator.SetBool("Idle_bool",true);
        }
    }
    void Slide_End()
    {
        this.AttakMove = 0;
        this.myAnimator.SetTrigger("Slide-end_trigger");
        //当たり判定を元に戻す
        myColliderComponent.center = new Vector3(0.3f, -0.3f, 0f);
        myColliderComponent.size = new Vector3(1.6f, 1.8f, 20f);
        if (EffectCounts >= 1)
        {
            //エフェクトを破壊する
            Effect.GetComponent<Player_effect_Controller>().DestroyObj();
        }

    }

    void OnTriggerStay(Collider other)
    {
        //Humanをつかむ
        if (other.gameObject.tag == "Human" && Catch == true)
        {
            this.myAnimator.SetBool("Catch-Run_bool", true);
        }
        //ダメージを受ける
        if ((Catch == true || CatchRun == true) && (other.gameObject.tag == "White ball" || other.gameObject.tag == "Red ball"))
        {
            this.HP -= 10f;
            this.myAnimator.SetTrigger("Death_trigger"); 
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            //HP0でゲームオーバー
            if(this.HP <= 0)
            {
                this.myAnimator.SetBool("Death-end_bool", true);
                //時間停止
                Time.timeScale = 0;
                //ゲームオーバー画面の呼び出し
                GameOverText.GetComponent<GameOver_Text_Controller>().GameOverJudge();
                //score表示を消す
                ScoreText.GetComponent<score_text_Controller>().GameOverJudge();
            }
        }
    }
}
