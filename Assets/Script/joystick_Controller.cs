using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joystick_Controller : MonoBehaviour
{
    //joystickのオブジェクトを入れる
    public GameObject JoyStick;
    //joystick Headのオブジェクトを入れる
    public GameObject JoyHead;
    //joystickのオブジェクトのインスタンスを入れる
    private GameObject JoyStickObj;
    //joystick Headのオブジェクトのインスタンスを入れる
    private GameObject JoyHeadObj;
    //オブジェクトの生成ができたかどうか判断
    private bool Object = false;

    //tapされた位置を入れる
    private Vector3 TapPos;
    //joystickの位置を入れる
    private Vector3 JoyStickPos;
    //joystickHeadの位置を入れる
    private Vector3 JoyHeadPos;
    //joystickHeadの目標位置を入れる
    private Vector3 Target;

    //joystickHeadの移動限界値
    private float JoyHeadRange = 0.4f;
    //補間の強さ(0なら追従しない､1で間隔なしで追従するらしい)
    private float space = 1f;

    //Plyaerのオブジェクト/スクリプト/transformを入れる
    private GameObject Player;
    private Player_Controller PlayerCom;
    private Transform PlayerTra;
    //左右の移動できる範囲
    private float WidthRange = 7.5f;
    //上下の移動できる範囲
    private float HeightRange = 4f;
    //Player.position取得用
    private Vector3 PlayerPos;

    // Start is called before the first frame update
    void Start()
    {
        //Plyaerのオブジェクト/スクリプト/transformを取得する
        Player = GameObject.Find("Player");
        PlayerCom = Player.GetComponent<Player_Controller>();
        PlayerTra = Player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //画面をtapしているとき
        if (Input.GetMouseButton(0))
        {
            //tapしている位置をスクリーン座標からワールド座標に変換し取得
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                TapPos = Camera.main.ScreenToWorldPoint(touch.position);
            }
            //画面左でtapしたとき､オブジェクトの生成と座標の取得を1フレームのみ行う
            if (TapPos.x < 0 && Input.GetMouseButtonDown(0))
            {
                //joystickを生成する
                JoyStickObj = Instantiate(JoyStick);
                JoyStickObj.transform.position = new Vector3(TapPos.x, TapPos.y, -5.0f);
                //JoyHeadを生成する
                JoyHeadObj = Instantiate(JoyHead);
                JoyHeadObj.transform.position = new Vector3(TapPos.x, TapPos.y, -5.5f);
                //JoyStickPosの座標を取得
                JoyStickPos = JoyStickObj.transform.position;
                //オブジェクト生成完了判断
                Object = true;
            }
            //オブジェクトが生成できた場合
            if (Object == true)
            {
                //TapPosの移動限度を定める
                TapPos.x = Mathf.Clamp(TapPos.x, JoyStickPos.x - JoyHeadRange, JoyStickPos.x + JoyHeadRange);
                TapPos.y = Mathf.Clamp(TapPos.y, JoyStickPos.y - JoyHeadRange, JoyStickPos.y + JoyHeadRange);
                //JoyHeadの移動先座標を代入
                Target = new Vector3(TapPos.x, TapPos.y, -5.5f);
                //JoyHeadを移動させる
                JoyHeadObj.transform.position = Vector3.Lerp(JoyHeadPos, Target, space);
                //JoyHeadPosの座標を取得
                JoyHeadPos = JoyHeadObj.transform.position;
                PlayerPos = PlayerTra.position;
                //右移動
                if (TapPos.x > JoyStickPos.x + 0.2 && TapPos.y < JoyStickPos.y + 0.4 && TapPos.y > JoyStickPos.y - 0.4)
                {
                    //体の向きを変える
                    PlayerTra.rotation = Quaternion.Euler(0, 0, 0);
                    if (this.WidthRange <= PlayerPos.x)
                    {
                        PlayerCom.DontRun();
                    }
                    else
                    {
                        PlayerCom.RightMove();
                    }
                }
                //左移動
                if(TapPos.x < JoyStickPos.x - 0.2 && TapPos.y < JoyStickPos.y + 0.4 && TapPos.y > JoyStickPos.y - 0.4)
                {
                    PlayerTra.rotation = Quaternion.Euler(0, 180, 0);
                    if (-this.WidthRange >= PlayerPos.x)
                    {
                        PlayerCom.DontRun();
                    }
                    else
                    {
                        PlayerCom.LeftMove();
                    }
                }
                //上移動
                if(TapPos.y > JoyStickPos.y + 0.2 && TapPos.x < JoyStickPos.x + 0.4 && TapPos.x > JoyStickPos.x - 0.4)
                {
                    PlayerTra.rotation = Quaternion.Euler(0, 0, 90);
                    if (this.HeightRange <= PlayerPos.y)
                    {
                        PlayerCom.DontRun();
                    }
                    else
                    {
                        PlayerCom.UpMove();
                    }
                }
                //下移動
                if(TapPos.y < JoyStickPos.y - 0.2 && TapPos.x < JoyStickPos.x + 0.4 && TapPos.x > JoyStickPos.x - 0.4)
                {
                    PlayerTra.rotation = Quaternion.Euler(0, 0, -90);
                    if (-this.HeightRange >= PlayerPos.y)
                    {
                        PlayerCom.DontRun();
                    }
                    else
                    {
                        PlayerCom.DownMove();
                    }
                }
                //右上移動
                if (TapPos.x >= JoyStickPos.x + 0.4 && TapPos.y >= JoyStickPos.y + 0.4)
                {
                    PlayerTra.rotation = Quaternion.Euler(0, 0, 45);
                    if (this.WidthRange <= PlayerPos.x && this.HeightRange <= PlayerPos.y)
                    {
                        PlayerCom.DontRun();
                    }
                    else if (this.WidthRange <= PlayerPos.x)
                    {
                        PlayerCom.UpMove();
                    }
                    else if (this.HeightRange <= PlayerPos.y)
                    {
                        PlayerCom.RightMove();
                    }
                    else
                    {
                        PlayerCom.RightUpMove();
                    }
                }
                //左上移動
                if (TapPos.x <= JoyStickPos.x - 0.4 && TapPos.y >= JoyStickPos.y + 0.4)
                {
                    PlayerTra.rotation = Quaternion.Euler(0, 180, 45);
                    if (-this.WidthRange >= PlayerPos.x && this.HeightRange <= PlayerPos.y)
                    {
                        PlayerCom.DontRun();
                    }
                    else if (-this.WidthRange >= PlayerPos.x)
                    {
                        PlayerCom.UpMove();
                    }
                    else if (this.HeightRange <= PlayerPos.y)
                    {
                        PlayerCom.LeftMove();
                    }
                    else
                    {
                        PlayerCom.LeftUpMove();
                    }
                }
                //右下移動
                if (TapPos.x >= JoyStickPos.x + 0.4 && TapPos.y <= JoyStickPos.y - 0.4)
                {
                    PlayerTra.rotation = Quaternion.Euler(0, 0, -45);
                    if (this.WidthRange <= PlayerPos.x && -this.HeightRange >= PlayerPos.y)
                    {
                        PlayerCom.DontRun();
                    }
                    else if (this.WidthRange <= PlayerPos.x)
                    {
                        PlayerCom.DownMove();
                    }
                    else if (-this.HeightRange >= PlayerPos.y)
                    {
                        PlayerCom.RightMove();
                    }
                    else
                    {
                        PlayerCom.RightDownMove();
                    }
                }
                //左下移動
                if (TapPos.x <= JoyStickPos.x - 0.4 && TapPos.y <= JoyStickPos.y - 0.4)
                {
                    PlayerTra.rotation = Quaternion.Euler(0, 180, -45);
                    if (-this.WidthRange >= PlayerPos.x && -this.HeightRange >= PlayerPos.y)
                    {
                        PlayerCom.DontRun();
                    }
                    else if (-this.WidthRange >= PlayerPos.x)
                    {
                        PlayerCom.DownMove();
                    }
                    else if (-this.HeightRange >= PlayerPos.y)
                    {
                        PlayerCom.LeftMove();
                    }
                    else
                    {
                        PlayerCom.LeftDownMove();
                    }
                }
            }
        }
        //指を離した際､オブジェクト破壊
        if (Input.GetMouseButtonUp(0) && Object == true)
        {
            Destroy(JoyStickObj);
            Destroy(JoyHeadObj);
            Object = false;
            PlayerCom.DontRun();
        }
    }
}
